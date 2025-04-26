namespace mitko_tenev_employees.Server.Tests.Controllers
{
    using Microsoft.AspNetCore.Http;
    using mitko_tenev_employees.Server.Controllers;
    using NSubstitute;
    using System.IO;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;

    public class EmployeeProjectControllerTest
    {
        private readonly EmployeeProjectController _sut;

        public EmployeeProjectControllerTest()
        {
            this._sut = new EmployeeProjectController();
        }

        [Fact]
        public async Task Upload()
        {
            var formFileMock = Substitute.For<IFormFile>();
            var content = "test";
            var fileName = "test.csv";

            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();

            ms.Position = 0;
            formFileMock.OpenReadStream().Returns(ms);
            formFileMock.FileName.Returns(fileName);
            formFileMock.Length.Returns(ms.Length);

            var actual = await this._sut.Upload(formFileMock);

            actual.Should().BeOfType<OkResult>();
        }
    }
}
