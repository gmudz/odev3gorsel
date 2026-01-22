using System.Text.Json;

namespace odev3gorsel
{
    public partial class HavaDurumuPage : ContentPage
    {
        private List<string> _cities = new List<string>();
        private readonly string _citiesFilePath;

        public HavaDurumuPage()
        {
            InitializeComponent();
            _citiesFilePath = Path.Combine(FileSystem.AppDataDirectory, "saved_cities.json");
            LoadCitiesFromJson();
            DisplayCities();
        }

        private void LoadCitiesFromJson()
        {
            try
            {
                if (File.Exists(_citiesFilePath))
                {
                    string json = File.ReadAllText(_citiesFilePath);
                    _cities = JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
                }
                else
                {
                    _cities = new List<string> { "BARTIN", "ANKARA", "ISTANBUL" };
                    SaveCitiesToJson();
                }
            }
            catch
            {
                _cities = new List<string> { "BARTIN", "ANKARA", "ISTANBUL" };
            }
        }

        private void SaveCitiesToJson()
        {
            try
            {
                string json = JsonSerializer.Serialize(_cities);
                File.WriteAllText(_citiesFilePath, json);
            }
            catch { }
        }

        private void DisplayCities()
        {
            CitiesStack.Children.Clear();

            foreach (var city in _cities)
            {
                var cityView = CreateCityView(city);
                CitiesStack.Children.Add(cityView);
            }
        }

        private View CreateCityView(string cityName)
        {
            string normalizedName = NormalizeTurkishChars(cityName);
            
            // MGM URL for weather image
            string url = $"https://www.mgm.gov.tr/sunum/tahmin-klasik-5070.aspx?m={normalizedName}&basla=1&bitir=5&rC=111&rZ=fff";

            var frame = new Frame
            {
                BackgroundColor = Application.Current?.RequestedTheme == AppTheme.Dark 
                    ? Color.FromArgb("#2d2d2d") : Colors.White,
                CornerRadius = 10,
                Padding = 0,
                HasShadow = true
            };

            var mainStack = new VerticalStackLayout { Spacing = 0 };

            // Header with city name and delete button
            var headerGrid = new Grid
            {
                BackgroundColor = Application.Current?.RequestedTheme == AppTheme.Dark 
                    ? Color.FromArgb("#004D40") : Color.FromArgb("#00897B"),
                Padding = new Thickness(10, 8),
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };

            var cityLabel = new Label
            {
                Text = cityName.ToUpper(),
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White,
                VerticalOptions = LayoutOptions.Center
            };

            var deleteButton = new Button
            {
                Text = "X",
                BackgroundColor = Colors.Red,
                TextColor = Colors.White,
                WidthRequest = 32,
                HeightRequest = 32,
                CornerRadius = 5,
                FontSize = 12,
                Padding = 0,
                CommandParameter = cityName
            };
            deleteButton.Clicked += DeleteCity_Clicked;

            headerGrid.Add(cityLabel, 0, 0);
            headerGrid.Add(deleteButton, 1, 0);

            // WebView for weather
            var webView = new WebView
            {
                Source = url,
                HeightRequest = 140,
                BackgroundColor = Colors.White
            };

            mainStack.Children.Add(headerGrid);
            mainStack.Children.Add(webView);
            frame.Content = mainStack;

            return frame;
        }

        private string NormalizeTurkishChars(string text)
        {
            return text
                .Replace("ç", "c").Replace("Ç", "C")
                .Replace("ğ", "g").Replace("Ğ", "G")
                .Replace("ı", "i").Replace("I", "I")
                .Replace("İ", "I")
                .Replace("ö", "o").Replace("Ö", "O")
                .Replace("ş", "s").Replace("Ş", "S")
                .Replace("ü", "u").Replace("Ü", "U")
                .ToUpper();
        }

        private async void AddCity_Clicked(object sender, EventArgs e)
        {
            string city = await DisplayPromptAsync(
                "Sehir Ekle", 
                "Sehir adini girin:",
                "Ekle", 
                "Iptal",
                placeholder: "Ornek: IZMIR");

            if (!string.IsNullOrWhiteSpace(city))
            {
                string normalizedCity = NormalizeTurkishChars(city.Trim());
                
                if (!_cities.Contains(normalizedCity))
                {
                    _cities.Add(normalizedCity);
                    SaveCitiesToJson();
                    DisplayCities();
                }
                else
                {
                    await DisplayAlert("Uyari", "Bu sehir zaten ekli!", "Tamam");
                }
            }
        }

        private async void DeleteCity_Clicked(object? sender, EventArgs e)
        {
            var button = sender as Button;
            var cityName = button?.CommandParameter?.ToString();

            if (!string.IsNullOrEmpty(cityName))
            {
                bool confirm = await DisplayAlert("Sil", $"{cityName} silinsin mi?", "Evet", "Hayir");
                
                if (confirm)
                {
                    _cities.Remove(cityName);
                    SaveCitiesToJson();
                    DisplayCities();
                }
            }
        }

        private void Refresh_Clicked(object sender, EventArgs e)
        {
            DisplayCities();
        }
    }
}