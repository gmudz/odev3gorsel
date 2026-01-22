namespace odev3gorsel
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Uygulama modern MAUI standartlarına uygun olarak başlatılır
            return new Window(new NavigationPage(new LoginPage()));
        }
    }
}