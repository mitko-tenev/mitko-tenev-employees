namespace mitko_tenev_employees.Server.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using mitko_tenev_employees.Server.Exceptions;
    using mitko_tenev_employees.Server.Models;
    using mitko_tenev_employees.Server.Services.Interfaces;
    using mitko_tenev_employees.Server.Utils;

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
                return BadRequest(new { error = ErrorUtils.EMPTY_CSV_PROVIDED });
            }

            try
            {
                IEnumerable<EmployeeProject> parsedEmployeeProjects = this._csvParserService.ParseCsvFile(file);
                IEnumerable<CommonProject> result = this._employeeProjectService.FindLongestWorkingPair(parsedEmployeeProjects);

                return Ok(result);
            }
            catch (InvalidCsvException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"{ErrorUtils.SERVER_ERROR_OCCURRED}: {ex.Message}" } );
            }
        }
    }
}
