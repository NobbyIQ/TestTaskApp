using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TestTaskApp.Services;
using TestTaskApp.Tests.Helpers;
using Xunit;

namespace TestTaskApp.Tests
{
    public class AdPlatformServiceTests
    {
        [Fact]
        public async Task LoadNewAdPlatforms_ShouldLoadValidData()
        {
            var file = FormFileFactory.Create("������.������:/ru\r\n���������� �������:/ru/svrd/revda,/ru/svrd/pervik\r\n������ ��������� ���������:/ru/msk,/ru/permobl,/ru/chelobl\r\n������ �������:/ru/svrd", "ad_platforms.txt");
            var service = new AdPlatformService(NullLogger<AdPlatformService>.Instance);

            await service.LoadNewAdPlatforms(file.File);

            var result = service.GetAdPlatformsByLocation("/ru");

            Assert.NotNull(result);
            Assert.Contains("������.������", result);
        }

        [Fact]
        public async Task LoadNewAdPlatforms_ShouldIgnoreIncorrectLines()
        {
            var file = FormFileFactory.Create("������.������:/ru\r\n���������� �������:");
            var service = new AdPlatformService(NullLogger<AdPlatformService>.Instance);

            await service.LoadNewAdPlatforms(file.File);
            var result = service.GetAdPlatformsByLocation("/ru");

            Assert.Single(result);
            Assert.Contains("������.������", result);

        }

        [Fact]
        public async Task LoadNewAdPlatforms_ShouldReturnMultipleMatches()
        {
            var file = FormFileFactory.Create("������.������:/ru\r\n���������� �������:/ru/svrd/revda,/ru/svrd/pervik\r\n������ ��������� ���������:/ru/msk,/ru/permobl,/ru/chelobl\r\n������ �������:/ru/svrd", "ad_platforms.txt");
            var service = new AdPlatformService(NullLogger<AdPlatformService>.Instance);

            await service.LoadNewAdPlatforms(file.File);
            var result = service.GetAdPlatformsByLocation("/ru/svrd/revda");

            Assert.Contains("���������� �������", result);
            Assert.Contains("������ �������", result);
            Assert.Contains("������.������", result);
        }

        [Fact]
        public async Task LoadNewAdPlatforms_ShouldReturnEmpty_WhenNotFound()
        {
            var file = FormFileFactory.Create("������.������:/ru\r\n���������� �������:/ru/svrd/revda,/ru/svrd/pervik\r\n������ ��������� ���������:/ru/msk,/ru/permobl,/ru/chelobl\r\n������ �������:/ru/svrd", "ad_platforms.txt");
            var service = new AdPlatformService(NullLogger<AdPlatformService>.Instance);

            await service.LoadNewAdPlatforms(file.File);
            var result = service.GetAdPlatformsByLocation("/tst");

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task LoadNewAdPlatforms_CaseInsensetive()
        {
            var file = FormFileFactory.Create("������.������:/ru\r\n���������� �������:/ru/svrd/revda,/ru/svrd/pervik\r\n������ ��������� ���������:/ru/msk,/ru/permobl,/ru/chelobl\r\n������ �������:/ru/svrd", "ad_platforms.txt");
            var service = new AdPlatformService(NullLogger<AdPlatformService>.Instance);

            await service.LoadNewAdPlatforms(file.File);
            var result = service.GetAdPlatformsByLocation("/RU");

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}