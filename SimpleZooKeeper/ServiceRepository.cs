using System.Net.Http.Headers;

namespace SimpleZooKeeper
{
    public class ServiceRepository
    {
        private List<SelfRegistrationData> selfRegistrationDatasRepository;
        public ServiceRepository()
        {
            selfRegistrationDatasRepository = new List<SelfRegistrationData>();
        }

        public SelfRegistrationData AddService(SelfRegistrationData selfRegistrationData)
        {
            selfRegistrationData.Created = DateTime.Now;

            selfRegistrationDatasRepository.Add(selfRegistrationData);

            return selfRegistrationData;
        }

        public void RemoveService(SelfRegistrationData selfRegistrationData)
        {
            selfRegistrationDatasRepository.RemoveAll(x => x.Name == selfRegistrationData.Name);
        }

        public IEnumerable<SelfRegistrationData> GetAllServices() => selfRegistrationDatasRepository;
        public SelfRegistrationData? GetOne() => selfRegistrationDatasRepository.FirstOrDefault(x => x.Created == selfRegistrationDatasRepository.Max(x => x.Created));

        public void UpdateServices()
        {

            foreach (var service in selfRegistrationDatasRepository)
            {
                HttpClient apiClient;

                apiClient = new HttpClient
                {
                    BaseAddress = new Uri(service.Url)
                };

                apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Console.WriteLine($"Making request to {apiClient.BaseAddress}");

                try
                {
                    var response = apiClient.GetAsync("WeatherForecast").Result;
                }
                catch (Exception ex)
                {
                    selfRegistrationDatasRepository.Remove(service);
                }

            }
        }
    }
}
