using odev3gorsel.Services;

namespace odev3gorsel
{
    public partial class RegisterPage : ContentPage
    {
        private readonly FirebaseService _firebaseService = new FirebaseService();

        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void BtnRegister_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EntEmail?.Text) || string.IsNullOrWhiteSpace(EntPassword?.Text))
            {
                await DisplayAlert("Uyarı", "Lütfen tüm alanları doldurun!", "Tamam");
                return;
            }

            try
            {
                await _firebaseService.RegisterUser(EntEmail.Text, EntPassword.Text);
                await DisplayAlert("Başarılı", "Kayıt işlemi tamamlandı.", "Tamam");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Kayıt başarısız: " + ex.Message, "Tamam");
            }
        }

        private async void BtnGoToLogin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}