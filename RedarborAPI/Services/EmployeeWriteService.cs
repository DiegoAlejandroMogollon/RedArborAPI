public class EmployeeWriteService : IEmployeeWriteService
{
    private readonly ApplicationDbContext _dbContext;

    public EmployeeWriteService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

public async Task<EmployeeDto> AddEmployeeAsync(EmployeeDto employeeDto)
{
    var employee = new Employee
{
        companyid = employeeDto.companyid,
        email = employeeDto.email,
        fax = employeeDto.fax,
        name = employeeDto.name,
        lastlogin = employeeDto.lastlogin?.ToUniversalTime(), 
        password = employeeDto.password,
        portalid = employeeDto.portalid,
        roleid = employeeDto.roleid,
        statusid = employeeDto.statusid,
        telephone = employeeDto.telephone,
        username = employeeDto.username,
        deletedon = employeeDto.deletedon?.ToUniversalTime(), 
        updatedon = employeeDto.updatedon?.ToUniversalTime(),
        createdon = employeeDto.createdon?.ToUniversalTime() ?? DateTime.UtcNow, 
    };
    _dbContext.Employee.Add(employee);
    await _dbContext.SaveChangesAsync();

    employeeDto.id = employee.id;
    return employeeDto;
}


public async Task<bool> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateEmployeeDto)
{
    var employee = await _dbContext.Employee.FindAsync(id);
    if (employee == null)
    {
        return false;
    }

    // Actualizar solo los campos proporcionados
    if (updateEmployeeDto.companyid.HasValue)
        employee.companyid = updateEmployeeDto.companyid.Value;
    if (!string.IsNullOrEmpty(updateEmployeeDto.email))
        employee.email = updateEmployeeDto.email;
    if (!string.IsNullOrEmpty(updateEmployeeDto.fax))
        employee.fax = updateEmployeeDto.fax;
    if (!string.IsNullOrEmpty(updateEmployeeDto.name))
        employee.name = updateEmployeeDto.name;
    if (!string.IsNullOrEmpty(updateEmployeeDto.password))
        employee.password = updateEmployeeDto.password;
    if (updateEmployeeDto.portalId.HasValue)
        employee.portalid = updateEmployeeDto.portalId.Value;
    if (updateEmployeeDto.roleId.HasValue)
        employee.roleid = updateEmployeeDto.roleId.Value;
    if (updateEmployeeDto.statusId.HasValue)
        employee.statusid = updateEmployeeDto.statusId.Value;
    if (!string.IsNullOrEmpty(updateEmployeeDto.telephone))
        employee.telephone = updateEmployeeDto.telephone;
    if (!string.IsNullOrEmpty(updateEmployeeDto.username))
        employee.username = updateEmployeeDto.username;

    employee.updatedon = DateTime.UtcNow;

    await _dbContext.SaveChangesAsync();
    return true;
}



    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        var employee = await _dbContext.Employee.FindAsync(id);
        if (employee == null)
        {
            return false;
        }

        _dbContext.Employee.Remove(employee);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ChangeEmployeeStatusAsync(int id, int status)
{
    var employee = await _dbContext.Employee.FindAsync(id);
    if (employee == null)
    {
        return false;
    }

    employee.statusid = status;

    if (status == 0) 
    {
        employee.deletedon = DateTime.UtcNow;
    }
    else if (status == 1) 
    {
        employee.deletedon = null;
    }

    employee.updatedon = DateTime.UtcNow; 

    await _dbContext.SaveChangesAsync();
    return true;
}

}
