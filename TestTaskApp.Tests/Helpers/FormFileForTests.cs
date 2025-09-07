using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskApp.Tests.Helpers
{
    public class FormFileForTests : IDisposable
    {
        private readonly MemoryStream _stream;
        public IFormFile File { get; }

        public FormFileForTests(string content, string fileName = "test.txt")
        {
            _stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            File = new FormFile(_stream, 0, _stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain"
            };
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}
