using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Concurrent;
using TestTaskApp.Models;

namespace TestTaskApp.Services
{
    public class AdPlatformService : IAdPlatformsService
    {
        private ConcurrentDictionary<string, List<string>> _storage = new(StringComparer.OrdinalIgnoreCase);
        private ILogger<AdPlatformService> _logger;

        public AdPlatformService(ILogger<AdPlatformService> logger)
        {
            _logger = logger;
        }

        public async Task LoadNewAdPlatforms(IFormFile file)
        {
            var tempStorage = new ConcurrentDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            _logger.LogInformation($"Загружаем новые локации и рекламные площадки из файла: {file.FileName}");
            using var stream = new StreamReader(file.OpenReadStream());
            string? line;
            while ((line = await stream.ReadLineAsync()) != null)
            {
                var parts = line.Split(':');
                if (parts.Length != 2)
                {
                    _logger.LogWarning($"Ошибка обработки строки {line}");
                    continue;
                }
                var platformName = parts[0].Trim();
                List<string> locations = parts[1]
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(l => l.Trim())
                    .ToList();

                foreach (var location in locations)
                {
                    if (!tempStorage.TryGetValue(location, out var adPlatforms))
                    {
                        adPlatforms = new List<string>();
                        tempStorage[location] = adPlatforms;
                    }
                    adPlatforms.Add(platformName);
                }
                _logger.LogInformation($"Обработка строки {line} прошла успешно");
            }

            _storage = tempStorage;

            _logger.LogInformation($"Файл {file.FileName} успешно загружен. Всего локаций: {_storage.Count}");
        }

        public List<string>? GetAdPlatformsByLocation(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                _logger.LogError("Не указана локация в строке поиска");
                return null;
            }

            var result = new HashSet<string>();

            var parts = location.Trim().Split("/", StringSplitOptions.RemoveEmptyEntries);

            for (int i = parts.Length; i > 0; i--)
            {
                var prefix = "/" + string.Join("/", parts.Take(i));
                if (_storage.TryGetValue(prefix, out var adPlatforms))
                {
                    foreach(var p in adPlatforms)
                    {
                        result.Add(p);
                    }
                }
            }
            _logger.LogInformation($"Поиск по локации {location} завершен");
            return result.ToList();
        }
    }
}
