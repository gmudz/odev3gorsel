using odev3gorsel.Models;

namespace odev3gorsel
{
    public partial class TodoDetayPage : ContentPage
    {
        private TodoItem? _todoItem;
        private bool _isEdit = false;

        public TodoDetayPage()
        {
            InitializeComponent();
            _isEdit = false;
            TamamBtn.Text = "Ekle";
            TarihPicker.Date = DateTime.Now;
            SaatPicker.Time = DateTime.Now.TimeOfDay;
        }

        public TodoDetayPage(TodoItem item)
        {
            InitializeComponent();
            _todoItem = item;
            _isEdit = true;
            TamamBtn.Text = "Guncelle";
            GorevEntry.Text = item.TaskName;
            DetayEditor.Text = item.Detail;
            TarihPicker.Date = item.Date;
            SaatPicker.Time = item.Time;
        }

        private async void Tamam_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GorevEntry.Text))
            {
                await DisplayAlert("Uyari", "Gorev giriniz", "OK");
                return;
            }
            var result = new TodoItem
            {
                Id = _isEdit ? _todoItem!.Id : Guid.NewGuid().ToString(),
                TaskName = GorevEntry.Text,
                Detail = DetayEditor.Text ?? "",
                Date = TarihPicker.Date,
                Time = SaatPicker.Time,
                IsCompleted = _isEdit ? _todoItem!.IsCompleted : false
            };
            MessagingCenter.Send(this, _isEdit ? "TodoUpdated" : "TodoAdded", result);
            await Shell.Current.Navigation.PopAsync();
        }

        private async void Iptal_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();
        }
    }
}