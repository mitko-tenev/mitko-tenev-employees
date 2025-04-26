namespace mitko_tenev_employees.Server.Models
{
    using CsvHelper.Configuration;

    public class EmployeeProjectCsvMap : ClassMap<EmployeeProject>
    {
        public EmployeeProjectCsvMap()
        {
            Map(m => m.EmpID).Name("EmpID");
            Map(m => m.ProjectID).Name("ProjectID");
            Map(m => m.DateFrom).Name("DateFrom");
            Map(m => m.DateTo).Name("DateTo").Convert(args =>
            {
                // Check if the string value is "NULL" and convert accordingly
                var value = args.Row.GetField("DateTo");

                return string.IsNullOrEmpty(value) || value.Trim().ToUpper() == "NULL" ? (DateTime?)null : DateTime.Parse(value);
            });
        }
    }
}
