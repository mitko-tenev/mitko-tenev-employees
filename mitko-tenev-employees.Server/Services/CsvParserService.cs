namespace mitko_tenev_employees.Server.Services
{
    using CsvHelper.Configuration;
    using CsvHelper;
    using mitko_tenev_employees.Server.Models;
    using System.Globalization;
    using mitko_tenev_employees.Server.Services.Interfaces;
    using CsvHelper.TypeConversion;
    using mitko_tenev_employees.Server.Exceptions;
    using mitko_tenev_employees.Server.Utils;

    public class CsvParserService : ICsvParserService
    {
        /**
         * Parses passed csv file. 
         * Uses comma as default delimiter and trims spaces by default.
         * Also populates DateTo property with current date if it contains NULL value.
         * Throws InvalidCsvException for invalid CSVs.
         */
        public List<EmployeeProject> ParseCsvFile(IFormFile file)
        {
            try
            {
                List<EmployeeProject> employeeProjects = new();

                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = ",",
                    TrimOptions = TrimOptions.Trim,
                }))
                {
                    csv.Context.RegisterClassMap<EmployeeProjectCsvMap>();
                    employeeProjects = csv.GetRecords<EmployeeProject>().ToList();
                }

                // Set NULL DateTo values to current date
                SetCurentDateForRecordsWithEmptyDateTo(employeeProjects);

                return employeeProjects;
            }
            catch (Exception ex) when (ex is TypeConverterException || ex is MissingFieldException)
            {
                throw new InvalidCsvException(ErrorUtils.INVALID_CSV_PROVIDED);
            }
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
