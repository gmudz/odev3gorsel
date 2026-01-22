using Newtonsoft.Json.Linq;
using odev3gorsel.Models;
using System.Collections.ObjectModel;

namespace odev3gorsel
{
    public partial class HaberlerPage : ContentPage
    {
        public ObservableCollection<Haber> Haberler { get; set; } = new ObservableCollection<Haber>();

        public HaberlerPage()
        {
            InitializeComponent();
            HaberList.ItemsSource = Haberler;
            LoadHaberler("manset");
        }

        private async void LoadHaberler(string kategori)
        {
            try
            {
                using HttpClient client = new HttpClient();
                string rssUrl = $"https://www.trthaber.com/{kategori}_articles.rss";
                string apiUrl = $"https://api.rss2json.com/v1/api.json?rss_url={rssUrl}";

                var response = await client.GetStringAsync(apiUrl);
                var data = JObject.Parse(response);

                Haberler.Clear();
                foreach (var item in data["items"])
                {
                    Haberler.Add(new Haber
                    {
                        Title = item["title"]?.ToString() ?? "",
                        Link = item["link"]?.ToString() ?? "",
                        Thumbnail = item["thumbnail"]?.ToString() ?? "dotnet_bot.png",
                        PubDate = item["pubDate"]?.ToString() ?? ""
                    });
                }
            }
            catch (Exception ex) 
            { 
                await DisplayAlert("Hata", ex.Message, "Tamam"); 
            }
        }

        private void Kategori_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.CommandParameter != null)
            {
                LoadHaberler(button.CommandParameter.ToString() ?? "manset");
            }
        }

        // Haber tiklandiginda detay sayfasina git
        private async void Haber_Tapped(object sender, TappedEventArgs e)
        {
            if (e.Parameter is Haber secilenHaber)
            {
                await Navigation.PushAsync(new HaberDetayPage(secilenHaber));
            }
        }
    }
}