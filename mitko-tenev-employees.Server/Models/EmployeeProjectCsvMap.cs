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
            Map(m => m.DateTo).Name("DateTo").Optional();
        }
    }
}
