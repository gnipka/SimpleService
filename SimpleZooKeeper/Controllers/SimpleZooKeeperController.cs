using Microsoft.AspNetCore.Mvc;

namespace SimpleZooKeeper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SimpleZooKeeperController : ControllerBase
    {
        private readonly ILogger<SimpleZooKeeperController> _logger;
        private readonly ServiceRepository serviceRepository;

        public SimpleZooKeeperController(ILogger<SimpleZooKeeperController> logger, ServiceRepository serviceRepository)
        {
            _logger = logger;
            this.serviceRepository = serviceRepository;
        }

        [HttpGet]
        public IEnumerable<SelfRegistrationData> Get()
        {
            //serviceRepository.UpdateServices();

            return serviceRepository.GetAllServices();
        }

        [HttpGet]
        public SelfRegistrationData GetOne()
        {
            //serviceRepository.UpdateServices();

            return serviceRepository.GetOne();
        }

        [HttpPost]
        public SelfRegistrationData RegistryService([FromBody] SelfRegistrationData selfRegistrationData)
        {
            return serviceRepository.AddService(selfRegistrationData);
            return new SelfRegistrationData();
        }
    }
}