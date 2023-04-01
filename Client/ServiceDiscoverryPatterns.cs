using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Client
{
    public static class ServiceDiscoverryPatterns
    {
        // плох, что если что-то случилось с хостом http://localhost:5003 ,то все порушится
        public static void PointToPointPattern()
        {
            while (true)
            {
                HttpClient apiClient;

                apiClient = new HttpClient
                {
                    BaseAddress = new Uri($"http://localhost:5003")
                };

                apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Console.WriteLine($"Making request to {apiClient.BaseAddress}");

                try
                {
                    var response = apiClient.GetAsync("WeatherForecast").Result;

                    var payload = response.Content.ReadAsStringAsync().Result;

                    Console.WriteLine($"Payload: {payload}\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"No response from {apiClient.BaseAddress} couse {ex.Message}");
                }
            }
        }

        // добавляем несколько адресов, к которым можно обращаться (файл Start.bat с запуском с разных портов)
        public static void LocalRegistryPattern()
        {
            string[] LocalRegistry = new string[] { "http://localhost:5004", "http://localhost:5005", "http://localhost:5006", "http://localhost:5007" };

            while (true)
            {
                foreach (string url in LocalRegistry)
                {
                    HttpClient apiClient;

                    apiClient = new HttpClient
                    {
                        BaseAddress = new Uri(url)
                    };

                    apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    Console.WriteLine($"Making request to {apiClient.BaseAddress}");

                    try
                    {
                        var response = apiClient.GetAsync("WeatherForecast").Result;

                        var payload = response.Content.ReadAsStringAsync().Result;

                        Console.WriteLine($"Payload: {payload}\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"No response from {apiClient.BaseAddress} couse {ex.Message}");
                    }

                }
            }
        }

        public static void SelfRegistrationPattern()
        {
            List<SelfRegistrationData> services = new();

            while (true)
            {

                HttpClient apiClient;

                apiClient = new HttpClient
                {
                    BaseAddress = new Uri("http://localhost:5020")
                };

                apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Console.WriteLine($"Making request to {apiClient.BaseAddress}");

                try
                {
                    services = apiClient.GetFromJsonAsync<List<SelfRegistrationData>>("SimpleZooKeeper").Result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"No response from {apiClient.BaseAddress} couse {ex.Message}");
                }

                if (services == null) continue;

                apiClient = new HttpClient
                {
                    BaseAddress = new Uri(services[0].Url)
                };

                apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Console.WriteLine($"Making request to {apiClient.BaseAddress}");

                try
                {
                    var response = apiClient.GetAsync("WeatherForecast").Result;

                    var payload = response.Content.ReadAsStringAsync().Result;

                    Console.WriteLine($"Payload: {payload}\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"No response from {apiClient.BaseAddress} couse {ex.Message}");
                }
            }
        }
    }
}
