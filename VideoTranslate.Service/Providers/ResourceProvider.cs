using VideoTranslate.Shared.Abstractions.Providers;

namespace VideoTranslate.Service.Providers
{
    public class ResourceProvider : IResourceProvider
    {
        public string GetTextResourceById(string resourceId, string culture = "en-US")
        {
            return resourceId;
        }
    }
}
