using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTranslate.Shared.DTO
{
    public enum VideoType : byte
    {
        Original = 1,
        StreamedConverted = 2,
        Converted = 3
    }
}
