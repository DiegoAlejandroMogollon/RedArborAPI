public interface IEmployeeWriteService
{
    Task<EmployeeDto> AddEmployeeAsync(EmployeeDto employeeDto);

    Task<bool> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateEmployeeDto);

    Task<bool> DeleteEmployeeAsync(int id);

    Task<bool> ChangeEmployeeStatusAsync(int id,int status);
}
