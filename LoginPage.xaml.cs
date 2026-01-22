using odev3gorsel.Services;

namespace odev3gorsel
{
    public partial class LoginPage : ContentPage
    {
        private readonly FirebaseService _firebaseService = new FirebaseService();

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Boşlukları temizleyerek kontrol et
                string email = LoginEmail?.Text?.Trim() ?? "";
                string password = LoginPassword?.Text ?? "";

                if (string.IsNullOrWhiteSpace(email) || password.Length < 6)
                {
                    await DisplayAlert("Hata", "Email girin ve şifrenin en az 6 karakter olduğundan emin olun.", "Tamam");
                    return;
                }

                var user = await _firebaseService.LoginUser(email, password);

                if (user != null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        if (Application.Current != null)
                            Application.Current.MainPage = new AppShell();
                    });
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Giriş Hatası", "Bilgiler hatalı veya hesap bulunamadı. Lütfen önce kaydolun.", "Tamam");
            }
        }

        private async void BtnGoToRegister_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}