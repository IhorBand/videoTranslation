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

        [HttpGet("GetHehYura")]
        public WeatherForecast GetHehYura()
        {
            return new WeatherForecast()
            {
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
                    Name = "Andrew's Flow",
                    StartMessageId = null,
                    ExpectedSteps = new List<ExpectedStep>()
                },
                new Flow() {
                    Id = new Guid("443eec7a-e0d9-47d5-8d51-e84a7bb13e29"),
                    Name = "Roman's Flow",
                    StartMessageId = null,
                    ExpectedSteps = new List<ExpectedStep>()
                },
                new Flow() {
                    Id = new Guid("1edaefba-21cc-481a-9798-b7dcc404685f"),
                    Name = "Ihor's Flow",
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
            else if (flowid == new Guid("443eec7a-e0d9-47d5-8d51-e84a7bb13e29"))
            {
                return new List<ExpectedStep>()
                {
                    new ExpectedStep() { Id = 1, Name = "Step1Test", ParentId = null, FlowId = new Guid("443eec7a-e0d9-47d5-8d51-e84a7bb13e29"), ExpectedStepMessages = new List<ExpectedStepMessage>()},
                    new ExpectedStep() { Id = 2, Name = "Step2Test", ParentId = 1, FlowId = new Guid("443eec7a-e0d9-47d5-8d51-e84a7bb13e29"), ExpectedStepMessages = new List<ExpectedStepMessage>()},
                    new ExpectedStep() { Id = 3, Name = "Step 3", ParentId = 2, FlowId = new Guid("443eec7a-e0d9-47d5-8d51-e84a7bb13e29"), ExpectedStepMessages = new List<ExpectedStepMessage>()},
                    new ExpectedStep() { Id = 4, Name = "Step 4", ParentId = 2, FlowId = new Guid("443eec7a-e0d9-47d5-8d51-e84a7bb13e29"), ExpectedStepMessages = new List<ExpectedStepMessage>()},
                    new ExpectedStep() { Id = 5, Name = "Step 5", ParentId = 3, FlowId = new Guid("443eec7a-e0d9-47d5-8d51-e84a7bb13e29"), ExpectedStepMessages = new List<ExpectedStepMessage>()},
                    new ExpectedStep() { Id = 6, Name = "Step 6", ParentId = 4, FlowId = new Guid("443eec7a-e0d9-47d5-8d51-e84a7bb13e29"), ExpectedStepMessages = new List<ExpectedStepMessage>()},
                };
            }
            else if (flowid == new Guid("1edaefba-21cc-481a-9798-b7dcc404685f"))
            {
                return new List<ExpectedStep>()
                {
                    new ExpectedStep() { Id = 1, Name = "Step1Test", ParentId = null, FlowId = new Guid("1edaefba-21cc-481a-9798-b7dcc404685f"), ExpectedStepMessages = new List<ExpectedStepMessage>()},
                    new ExpectedStep() { Id = 2, Name = "Step2Test", ParentId = 1, FlowId = new Guid("1edaefba-21cc-481a-9798-b7dcc404685f"), ExpectedStepMessages = new List<ExpectedStepMessage>()},
                    new ExpectedStep() { Id = 3, Name = "Step2Test", ParentId = 2, FlowId = new Guid("1edaefba-21cc-481a-9798-b7dcc404685f"), ExpectedStepMessages = new List<ExpectedStepMessage>()},
                    new ExpectedStep() { Id = 4, Name = "Step2Test", ParentId = 3, FlowId = new Guid("1edaefba-21cc-481a-9798-b7dcc404685f"), ExpectedStepMessages = new List<ExpectedStepMessage>()},
                    new ExpectedStep() { Id = 5, Name = "Step2Test", ParentId = 3, FlowId = new Guid("1edaefba-21cc-481a-9798-b7dcc404685f"), ExpectedStepMessages = new List<ExpectedStepMessage>()},
                    new ExpectedStep() { Id = 6, Name = "Step2Test", ParentId = 4, FlowId = new Guid("1edaefba-21cc-481a-9798-b7dcc404685f"), ExpectedStepMessages = new List<ExpectedStepMessage>()},
                    new ExpectedStep() { Id = 7, Name = "Step2Test", ParentId = 5, FlowId = new Guid("1edaefba-21cc-481a-9798-b7dcc404685f"), ExpectedStepMessages = new List<ExpectedStepMessage>()},
                    new ExpectedStep() { Id = 8, Name = "Step2Test", ParentId = 5, FlowId = new Guid("1edaefba-21cc-481a-9798-b7dcc404685f"), ExpectedStepMessages = new List<ExpectedStepMessage>()},
                };
            }
            return new List<ExpectedStep>();
        }
    }
}