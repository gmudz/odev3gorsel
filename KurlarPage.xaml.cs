using Newtonsoft.Json.Linq;
using odev3gorsel.Models;
using System.Collections.ObjectModel;

namespace odev3gorsel
{
    public partial class KurlarPage : ContentPage
    {
        public ObservableCollection<Doviz> Dovizler { get; set; } = new ObservableCollection<Doviz>();

        public KurlarPage()
        {
            InitializeComponent();
            KurlarList.ItemsSource = Dovizler;
            LoadKurlar();
        }

        private async void LoadKurlar()
        {
            LoadingIndicator.IsVisible = true; // ActivityIndicator Başlat (Puan Kriteri) [cite: 129]

            try
            {
                using HttpClient client = new HttpClient();
                var response = await client.GetStringAsync("https://finans.truncgil.com/today.json");
                var data = JObject.Parse(response);

                Dovizler.Clear();
                // JSON verisini ayıklama
                foreach (var item in data)
                {
                    if (item.Key == "Update_Date") continue;

                    Dovizler.Add(new Doviz
                    {
                        Isim = item.Key,
                        Alis = item.Value["Alış"]?.ToString() ?? "0",
                        Satis = item.Value["Satış"]?.ToString() ?? "0",
                        Fark = item.Value["Değişim"]?.ToString() ?? "%0"
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Kurlar yüklenemedi: " + ex.Message, "Tamam");
            }
            finally
            {
                LoadingIndicator.IsVisible = false; // ActivityIndicator Durdur [cite: 129]
            }
        }
    }
}