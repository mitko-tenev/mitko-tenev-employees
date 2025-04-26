namespace mitko_tenev_employees.Server.Models
{
    using CsvHelper.Configuration;
    using mitko_tenev_employees.Server.Utils;
    using System.Globalization;

    public class EmployeeProjectCsvMap : ClassMap<EmployeeProject>
    {
        public EmployeeProjectCsvMap()
        {
            Map(m => m.EmpID).Name("EmpID");
            Map(m => m.ProjectID).Name("ProjectID");
            Map(m => m.DateFrom).Name("DateFrom").TypeConverterOption.Format(DateUtils.dateParseFormats);
            Map(m => m.DateTo).Name("DateTo").Convert(args =>
            {
                var value = args.Row.GetField("DateTo");

                return string.IsNullOrEmpty(value) || value.Trim().ToUpper() == "NULL" 
                    ? (DateTime?)null 
                    : DateTime.ParseExact(value, DateUtils.dateParseFormats, CultureInfo.InvariantCulture, DateTimeStyles.None);
            });
        }
    }
}
