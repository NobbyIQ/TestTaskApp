using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestTaskApp.Tests.Helpers;

namespace TestTaskApp.Tests
{
    public class AdPlatformIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AdPlatformIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task UploadAndSearchShouldWorkCorrectly()
        {
            using var fileContent = FormFileFactory.CreateFileContent("Яндекс.Директ:/ru\r\nРевдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik\r\nГазета уральских москвичей:/ru/msk,/ru/permobl,/ru/chelobl\r\nКрутая реклама:/ru/svrd", "AdPlatfoems.txt");

            var uploadResponse = await _client.PostAsync("/api/upload", fileContent);

            Assert.Equal(HttpStatusCode.OK, uploadResponse.StatusCode);

            var searchResponse = await _client.GetAsync("/api/search?location=/ru");
            Assert.Equal(HttpStatusCode.OK, searchResponse.StatusCode);
            var content = await searchResponse.Content.ReadAsStringAsync();

            Assert.Contains("Яндекс.Директ", content);
        }

        [Fact]
        public async Task UploadFileShouldReturnBadRequestForMissingFile()
        {
            using var fileContent = new MultipartFormDataContent();
            var uploadResponse = await _client.PostAsync("/api/upload", fileContent);

            Assert.Equal(HttpStatusCode.BadRequest, uploadResponse.StatusCode);
        }

        [Fact]
        public async Task UploadFileShouldReturnBadRequestForInvalidFile()
        {
            using var fileContent = FormFileFactory.CreateFileContent("", "EmptyFile.txt");
            var uploadResponse = await _client.PostAsync("/api/upload", fileContent);

            Assert.Equal(HttpStatusCode.BadRequest, uploadResponse.StatusCode);
        }

        [Fact]
        public async Task SearchShouldReturnBadRequestForMissingLocation()
        {
            var searchResponse = await _client.GetAsync("/api/search");
            Assert.Equal(HttpStatusCode.BadRequest, searchResponse.StatusCode);
        }

        [Fact]
        public async Task SearchShouldReturnNotFoundForUnknownLocation()
        {
            using var fileContent = FormFileFactory.CreateFileContent("Яндекс.Директ:/ru", "AdPlatforms.txt");

            await _client.PostAsync("/api/upload", fileContent);

            var searchResponse = await _client.GetAsync("/api/search?location=/unknown");
            Assert.Equal(HttpStatusCode.NotFound, searchResponse.StatusCode);
        }
    }
}
