namespace mitko_tenev_employees.Server.Services
{
    using CsvHelper.Configuration;
    using CsvHelper;
    using mitko_tenev_employees.Server.Models;
    using System.Globalization;
    using mitko_tenev_employees.Server.Services.Interfaces;

    public class CsvParserService : ICsvParserService
    {
        public List<EmployeeProject> ParseCsvFile(IFormFile file)
        {
            List<EmployeeProject> employeeProjects = new();

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ","
            }))
            {
                csv.Context.RegisterClassMap<EmployeeProjectCsvMap>();
                employeeProjects = csv.GetRecords<EmployeeProject>().ToList();
            }

            // Set NULL DateTo values to current date
            SetCurentDateForRecordsWithEmptyDateTo(employeeProjects);

            return employeeProjects;
        }

        private void SetCurentDateForRecordsWithEmptyDateTo(IEnumerable<EmployeeProject> employeeProjects)
        {
            var today = DateTime.Today;
            foreach (var project in employeeProjects)
            {
                if (!project.DateTo.HasValue)
                {
                    project.DateTo = today;
                }
            }
        }
    }
}
