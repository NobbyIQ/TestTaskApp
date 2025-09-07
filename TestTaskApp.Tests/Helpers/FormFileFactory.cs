using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Text;


namespace TestTaskApp.Tests.Helpers
{
    public class FormFileFactory
    {
        public static FormFileForTests Create(string content, string filename = "test.txt")
        {
            return new FormFileForTests(content, filename);
        }

        public static MultipartFormDataContent CreateFileContent(string content, string fileName = "test.txt")
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            var fileContent = new ByteArrayContent(bytes);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/plain");

            var multipartFormDataContent = new MultipartFormDataContent();
            multipartFormDataContent.Add(fileContent, "file", fileName);
            return multipartFormDataContent;
        }
    }
}
