using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTranslate.Shared.Abstractions.Providers
{
    public interface IResourceProvider
    {
        string GetTextResourceById(string resourceId, string culture = "en-US");
    }
}
