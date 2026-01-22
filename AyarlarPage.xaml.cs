namespace odev3gorsel;

public partial class AyarlarPage : ContentPage
{
    public AyarlarPage()
    {
        InitializeComponent();
    }

    private void Theme_Clicked(object sender, EventArgs e)
    {
        string theme = (sender as Button)?.CommandParameter?.ToString() ?? "Default";

        if (Application.Current != null)
        {
            Application.Current.UserAppTheme = theme switch
            {
                "Dark" => AppTheme.Dark,
                "Light" => AppTheme.Light,
                _ => AppTheme.Unspecified
            };
        }
    }
}