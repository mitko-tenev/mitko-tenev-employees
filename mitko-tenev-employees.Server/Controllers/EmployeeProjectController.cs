namespace mitko_tenev_employees.Server.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using mitko_tenev_employees.Server.Models;
    using mitko_tenev_employees.Server.Services.Interfaces;

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeProjectController : ControllerBase
    {
        private readonly ICsvParserService _csvParserService;
        private readonly IEmployeeProjectService _employeeProjectService;

        public EmployeeProjectController(ICsvParserService csvParserService, IEmployeeProjectService employeeProjectService)
        {
            this._csvParserService = csvParserService;
            this._employeeProjectService = employeeProjectService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Uploaded CSV file is empty");
            }

            try
            {
                IEnumerable<EmployeeProject> parsedEmployeeProjects = this._csvParserService.ParseCsvFile(file);
                IEnumerable<CommonProject> result = this._employeeProjectService.FindLongestWorkingPair(parsedEmployeeProjects);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error occurred: {ex.Message}");
            }
        }
    }
}
