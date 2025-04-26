namespace mitko_tenev_employees.Server.Models
{
    public class EmployeeProject
    {
        public int EmpID { get; set; }
        public int ProjectID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
