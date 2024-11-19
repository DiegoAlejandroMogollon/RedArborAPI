public interface IEmployeeReadService
{
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();

    Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
}
