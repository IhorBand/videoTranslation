using Microsoft.AspNetCore.Mvc;

namespace VideoTranslate.Controllers
{
    public class Flow
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? StartMessageId { get; set; }
        public List<ExpectedStep> ExpectedSteps { get; set; }
    }

    public class ExpectedStep
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public Guid FlowId { get; set; }
        public List<ExpectedStepMessage> ExpectedStepMessages { get; set; }
    }

    public class ExpectedStepMessage
    {
        public int Id { get; set; }
        public int ExpectedStepId { get; set; }
        public int MessageId { get; set; }
        public int? Order { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetHeh")]
        public WeatherForecast GetHeh()
        {
            return new WeatherForecast() { 
                Date = DateTime.UtcNow,
                TemperatureC = 1,
                Summary = "-Шановний,не пийте воду з цього ставка,туди лайно з всього села стікає. - Што ви гаварітє ? Гаварітє па рускі, я вас нє панімаю. - Я кажу двома руками черпай!"
            };
        }

        [HttpGet("Flow")]
        public List<Flow> GetFlow()
        {
            return new List<Flow>()
            {
                new Flow() {
                    Id = new Guid("38890a3c-45f5-40a2-8512-a9952381dd28"),
                    Name = "Flow1",
                    StartMessageId = null,
                    ExpectedSteps = new List<ExpectedStep>()
                },
                new Flow() {
                    Id = Guid.NewGuid(),
                    Name = "Flow2",
                    StartMessageId = null,
                    ExpectedSteps = new List<ExpectedStep>()
                }
            };
        }

        [HttpGet("ExpectedSteps")]
        public List<ExpectedStep> GetExpectedSteps([FromQuery(Name = "flowid")] Guid flowid )
        {
            if (flowid == new Guid("38890a3c-45f5-40a2-8512-a9952381dd28"))
            {
                return new List<ExpectedStep>()
                {
                    new ExpectedStep() { Id = 1, Name = "Step1Test", ParentId = null, FlowId = new Guid("38890a3c-45f5-40a2-8512-a9952381dd28"), ExpectedStepMessages = new List<ExpectedStepMessage>()},
                    new ExpectedStep() { Id = 2, Name = "Step2Test", ParentId = 1, FlowId = new Guid("38890a3c-45f5-40a2-8512-a9952381dd28"), ExpectedStepMessages = new List<ExpectedStepMessage>()}
                };
            }
            return new List<ExpectedStep>();
        }
    }
}