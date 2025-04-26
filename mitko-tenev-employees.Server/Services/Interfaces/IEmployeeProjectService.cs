namespace mitko_tenev_employees.Server.Services.Interfaces
{
    using mitko_tenev_employees.Server.Models;

    public interface IEmployeeProjectService
    {
        IEnumerable<CommonProject> FindLongestWorkingPair(IEnumerable<EmployeeProject> parsedEmployeeProjects);
    }
}