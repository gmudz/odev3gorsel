using Newtonsoft.Json;

namespace odev3gorsel
{
    public partial class HavaDurumuPage : ContentPage
    {
        private const string ApiKey = "8d7c9730dc973af7578440d323f52bbc";
        private const string CitiesFileName = "saved_cities.json"; // JSON dosya adı

        public HavaDurumuPage()
        {
            InitializeComponent();
            LoadSavedCity();
        }

        private async void GetWeather_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CityEntry.Text))
                await FetchWeatherData(CityEntry.Text);
        }

        // Türkçe karakterleri dönüştürme fonksiyonu
        private string ConvertTurkishCharacters(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            // Türkçe karakter dönüşümleri
            var replacements = new Dictionary<char, char>
            {
                {'ş', 's'}, {'Ş', 'S'},
                {'ğ', 'g'}, {'Ğ', 'G'},
                {'ı', 'i'}, {'İ', 'I'},
                {'ö', 'o'}, {'Ö', 'O'},
                {'ü', 'u'}, {'Ü', 'U'},
                {'ç', 'c'}, {'Ç', 'C'}
            };

            var result = text.ToCharArray();
            for (int i = 0; i < result.Length; i++)
            {
                if (replacements.ContainsKey(result[i]))
                {
                    result[i] = replacements[result[i]];
                }
            }

            return new string(result);
        }

        private async Task FetchWeatherData(string city)
        {
            try
            {
                using HttpClient client = new HttpClient();
                
                // Türkçe karakterleri dönüştür
                string convertedCity = ConvertTurkishCharacters(city);
                
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={convertedCity}&appid={ApiKey}&units=metric&lang=tr";

                var response = await client.GetStringAsync(url);
                var data = JsonConvert.DeserializeObject<dynamic>(response);

                CityLabel.Text = data?.name;
                TempLabel.Text = $"{data?.main?.temp}°C";
                DescLabel.Text = data?.weather[0]?.description;

                // Şehri JSON dosyasına kaydet
                await SaveCityToJson(city);
            }
            catch (Exception)
            {
                await DisplayAlert("Hata", "Şehir bulunamadı veya API henüz aktifleşmedi.", "Tamam");
            }
        }

        // Şehri JSON dosyasına kaydet
        private async Task SaveCityToJson(string city)
        {
            try
            {
                string filePath = Path.Combine(FileSystem.AppDataDirectory, CitiesFileName);
                
                List<string> cities = new List<string>();
                
                // Mevcut dosyayı oku
                if (File.Exists(filePath))
                {
                    string existingJson = await File.ReadAllTextAsync(filePath);
                    cities = JsonConvert.DeserializeObject<List<string>>(existingJson) ?? new List<string>();
                }

                // Şehir listede yoksa ekle
                if (!cities.Contains(city, StringComparer.OrdinalIgnoreCase))
                {
                    cities.Add(city);
                }

                // JSON olarak kaydet
                string json = JsonConvert.SerializeObject(cities, Formatting.Indented);
                await File.WriteAllTextAsync(filePath, json);

                // Ayrıca son aranan şehri kaydet
                Preferences.Set("LastSearchCity", city);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Şehir kaydedilemedi: {ex.Message}");
            }
        }

        // Kaydedilmiş şehri yükle
        private async void LoadSavedCity()
        {
            try
            {
                string filePath = Path.Combine(FileSystem.AppDataDirectory, CitiesFileName);
                
                string lastCity = "Bartın"; // Varsayılan şehir

                // JSON dosyasından son şehri oku
                if (File.Exists(filePath))
                {
                    string json = await File.ReadAllTextAsync(filePath);
                    var cities = JsonConvert.DeserializeObject<List<string>>(json);
                    
                    if (cities != null && cities.Count > 0)
                    {
                        lastCity = cities.Last(); // Son eklenen şehri al
                    }
                }
                else
                {
                    // Eğer dosya yoksa Preferences'tan oku
                    lastCity = Preferences.Get("LastSearchCity", "Bartın");
                }

                CityEntry.Text = lastCity;
                await FetchWeatherData(lastCity);
            }
            catch (Exception)
            {
                CityEntry.Text = "Bartın";
                await FetchWeatherData("Bartın");
            }
        }
    }
}