using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTranslate.Shared.DTO.Configuration
{
    public class RabbitMQConfiguration
    {
        public string HostName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
