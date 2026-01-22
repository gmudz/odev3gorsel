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
        private TodoItem? _editingItem = null; // Düzenleme modu için

        public YapilacaklarPage()
        {
            InitializeComponent();
            TodoCollection.ItemsSource = TodoList;
            LoadTodosFromFirebase();
        }

        // Görev Ekleme veya Güncelleme
        private async void AddTodo_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TodoEntry.Text)) 
            {
                await DisplayAlert("Uyarı", "Lütfen görev başlığı girin!", "Tamam");
                return;
            }

            if (_editingItem != null)
            {
                // Güncelleme modu
                _editingItem.TaskName = TodoEntry.Text;
                _editingItem.Detail = DetailEntry.Text ?? "";
                _editingItem.Date = TaskDatePicker.Date;
                _editingItem.Time = TaskTimePicker.Time;

                try
                {
                    await _firebaseService.UpdateTodo(_editingItem);
                    
                    // Listeyi yenile
                    var index = TodoList.IndexOf(_editingItem);
                    if (index >= 0)
                    {
                        TodoList.RemoveAt(index);
                        TodoList.Insert(index, _editingItem);
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Hata", "Görev güncellenemedi: " + ex.Message, "Tamam");
                }
                
                _editingItem = null;
            }
            else
            {
                // Yeni ekleme
                var newItem = new TodoItem 
                { 
                    TaskName = TodoEntry.Text,
                    Detail = DetailEntry.Text ?? "",
                    Date = TaskDatePicker.Date,
                    Time = TaskTimePicker.Time,
                    IsCompleted = false
                };

                try
                {
                    newItem.Id = await _firebaseService.AddTodo(newItem);
                    TodoList.Add(newItem);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Hata", "Görev kaydedilemedi: " + ex.Message, "Tamam");
                }
            }

            ClearInputs();
        }

        // Düzenleme Butonu
        private void EditTodo_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var item = btn?.CommandParameter as TodoItem;

            if (item != null)
            {
                _editingItem = item;
                TodoEntry.Text = item.TaskName;
                DetailEntry.Text = item.Detail;
                TaskDatePicker.Date = item.Date;
                TaskTimePicker.Time = item.Time;
            }
        }

        // Silme Butonu - Onay ile
        private async void DeleteTodo_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var item = btn?.CommandParameter as TodoItem;

            if (item != null)
            {
                // Silme onayı iste
                bool confirm = await DisplayAlert(
                    "Silme Onayı", 
                    $"'{item.TaskName}' görevini silmek istediğinize emin misiniz?", 
                    "Evet, Sil", 
                    "İptal");

                if (confirm)
                {
                    try
                    {
                        await _firebaseService.DeleteTodo(item.Id);
                        TodoList.Remove(item);
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Hata", "Görev silinemedi: " + ex.Message, "Tamam");
                    }
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
                try
                {
                    await _firebaseService.UpdateTodo(item);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Hata", "Görev güncellenemedi: " + ex.Message, "Tamam");
                }
            }
        }

        // Input alanlarını temizle
        private void ClearInputs()
        {
            TodoEntry.Text = string.Empty;
            DetailEntry.Text = string.Empty;
            TaskDatePicker.Date = DateTime.Now;
            TaskTimePicker.Time = DateTime.Now.TimeOfDay;
        }

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
    }
}