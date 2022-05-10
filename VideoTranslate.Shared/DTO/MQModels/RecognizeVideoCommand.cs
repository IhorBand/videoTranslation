using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTranslate.Shared.DTO.MQModels
{
    public class RecognizeVideoCommand
    {
        public Guid VideoId { get; set; }
        public string OriginalLanguage { get; set; }
    }
}
