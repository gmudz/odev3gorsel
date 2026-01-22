using odev3gorsel.Models;

namespace odev3gorsel
{
    public partial class HaberDetayPage : ContentPage
    {
        Haber _secilenHaber;
        public HaberDetayPage(Haber haber)
        {
            InitializeComponent();
            _secilenHaber = haber;
            HaberWeb.Source = haber.Link;
        }

        private async void Share_Clicked(object sender, EventArgs e)
        {
            await Share.Default.RequestAsync(new ShareTextRequest
            {
                Uri = _secilenHaber.Link,
                Title = _secilenHaber.Title
            });
        }
    }
}