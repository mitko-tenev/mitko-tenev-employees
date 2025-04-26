namespace mitko_tenev_employees.Server.Services.Interfaces
{
    using mitko_tenev_employees.Server.Models;

    public interface ICsvParserService
    {
        List<EmployeeProject> ParseCsvFile(IFormFile file);
    }
}