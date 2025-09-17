using System.Text.Json;

namespace APIComunication
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiKey = "4d8bb2dd0950ef5660bd55e2e3195971";

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnGetWeatherClicked(object sender, EventArgs e)
        {
            string city = CityEntry.Text;

            if (string.IsNullOrEmpty(city))
            {
                await DisplayAlert("Error", "Please enter a city name.", "OK");
                return;
            }

            string url = $"https://api.weatherstack.com/current?access_key={_apiKey}&query={city}";

            using (HttpClient client = new HttpClient())
            { 
                try
                {
                    string response = await client.GetStringAsync(url);
                    using (JsonDocument doc = JsonDocument.Parse(response))
                    {
                        var root = doc.RootElement;
                        var temp = root.GetProperty("current").GetProperty("temperature").GetInt32();
                        var weatherDescArray = root.GetProperty("current").GetProperty("weather_descriptions");
                        var weather_desc = weatherDescArray[0].GetString();
                        var windSpeed = root.GetProperty("current").GetProperty("wind_speed").GetInt32();

                        // Get weather_icons array and use the first icon
                        var weatherIconsArray = root.GetProperty("current").GetProperty("weather_icons");
                        var weatherIconUrl = weatherIconsArray[0].GetString();

                        Console.WriteLine($"Temperature: {temp}°C");
                        Console.WriteLine($"Weather Description: {weather_desc}");
                        Console.WriteLine($"Wind Speed: {windSpeed} km/h");
                        Console.WriteLine($"Weather Icon: {weatherIconUrl}");

                        LocationLabel.Text = city;
                        TempLabel.Text = $"{temp}°C";
                        WeatherLabel.Text = weather_desc;
                        WindLabel.Text = $"{windSpeed} km/h";

                        // Set the icon image source
                        WeatherIcon.Source = weatherIconUrl;
                        WeatherIcon.IsVisible = true;
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Something went wrong: {ex.Message}", "OK");
                }
            }
        }
    }
}
