namespace mitko_tenev_employees.Server.Tests.Utils
{
    using Microsoft.AspNetCore.Http;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class FormFileMockFactory
    {
        internal static IFormFile GetFormFileMock(string content, string fileName)
        {
            IFormFile formFileMock = Substitute.For<IFormFile>();
            //var content = "test";
            //var fileName = "test.csv";

            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();

            ms.Position = 0;
            formFileMock.OpenReadStream().Returns(ms);
            formFileMock.FileName.Returns(fileName);
            formFileMock.Length.Returns(ms.Length);

            return formFileMock;
        }
    }
}
