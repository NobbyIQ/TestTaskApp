using Microsoft.Extensions.FileProviders;
using TestTaskApp.Models;

namespace TestTaskApp.Services
{
    public interface IAdPlatformsService
    {
        public Task LoadNewAdPlatforms(IFormFile file);
        public List<string>? GetAdPlatformsByLocation(string location);
    }
}
