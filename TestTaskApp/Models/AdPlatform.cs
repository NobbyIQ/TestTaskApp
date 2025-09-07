
using System.ComponentModel.DataAnnotations;

namespace TestTaskApp.Models
{
    public class AdPlatform
    {
        public string PlatformName { get; set; } = string.Empty;
        public List<string> Locations { get; set; } = new();
    }
}
