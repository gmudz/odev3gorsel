using Newtonsoft.Json;
using odev3gorsel.Models;
using odev3gorsel.Services;
using System.Collections.ObjectModel;

namespace odev3gorsel
{
    public partial class YapilacaklarPage : ContentPage
    {
        public ObservableCollection<TodoItem> TodoList { get; set; } = new ObservableCollection<TodoItem>();
        private readonly FirebaseService _firebaseService = new FirebaseService();

        public YapilacaklarPage()
        {
            InitializeComponent();
            TodoCollection.ItemsSource = TodoList;
            LoadTodosFromFirebase();

            // MessagingCenter abonelikleri
            MessagingCenter.Subscribe<TodoDetayPage, TodoItem>(this, "TodoAdded", async (sender, item) =>
            {
                await SaveTodoToFirebase(item);
                TodoList.Add(item);
            });

            MessagingCenter.Subscribe<TodoDetayPage, TodoItem>(this, "TodoUpdated", async (sender, item) =>
            {
                await UpdateTodoInFirebase(item);
                var index = TodoList.ToList().FindIndex(t => t.Id == item.Id);
                if (index >= 0)
                {
                    TodoList.RemoveAt(index);
                    TodoList.Insert(index, item);
                }
            });
        }

        // Yeni görev ekleme - Detay sayfasına git
        private async void AddNew_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new TodoDetayPage());
        }

        // Görev düzenleme - Detay sayfasına git
        private async void TodoItem_Tapped(object sender, TappedEventArgs e)
        {
            if (e.Parameter is TodoItem item)
            {
                await Shell.Current.Navigation.PushAsync(new TodoDetayPage(item));
            }
        }

        // Silme Butonu - "Silinsin mi?" onayı
        private async void DeleteTodo_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var item = btn?.CommandParameter as TodoItem;

            if (item != null)
            {
                // "Silinsin mi?" onay diyaloğu
                bool confirm = await DisplayAlert(
                    "Silinsin mi?", 
                    "Silmeyi onaylıyor musunuz?", 
                    "OK", 
                    "CANCEL");

                if (confirm)
                {
                    await DeleteTodoFromFirebase(item);
                    TodoList.Remove(item);
                }
            }
        }

        // Checkbox değişikliği
        private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var item = (checkBox?.BindingContext) as TodoItem;

            if (item != null)
            {
                item.IsCompleted = e.Value;
                await UpdateTodoInFirebase(item);
            }
        }

        // ============ FIREBASE İŞLEMLERİ ============

        // Firebase'den görevleri yükle
        private async void LoadTodosFromFirebase()
        {
            try
            {
                var items = await _firebaseService.GetAllTodos();

                TodoList.Clear();
                foreach (var item in items)
                {
                    TodoList.Add(item);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Görevler yüklenemedi: " + ex.Message, "Tamam");
            }
        }

        // Firebase'e yeni görev kaydet
        private async Task SaveTodoToFirebase(TodoItem item)
        {
            try
            {
                item.Id = await _firebaseService.AddTodo(item);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Görev kaydedilemedi: " + ex.Message, "Tamam");
            }
        }

        // Firebase'de görev güncelle
        private async Task UpdateTodoInFirebase(TodoItem item)
        {
            try
            {
                await _firebaseService.UpdateTodo(item);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Görev güncellenemedi: " + ex.Message, "Tamam");
            }
        }

        // Firebase'den görev sil
        private async Task DeleteTodoFromFirebase(TodoItem item)
        {
            try
            {
                await _firebaseService.DeleteTodo(item.Id);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Görev silinemedi: " + ex.Message, "Tamam");
            }
        }

        // Sayfa kapatıldığında abonelikleri kaldır
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // MessagingCenter.Unsubscribe kullanmayın çünkü sayfa tekrar açılabilir
        }
    }
}