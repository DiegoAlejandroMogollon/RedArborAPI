using Dapper;
using Microsoft.EntityFrameworkCore;
public class EmployeeReadService : IEmployeeReadService
{
    private readonly ApplicationDbContext _dbContext;

    public EmployeeReadService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        // Usar Dapper para las lecturas
        using var connection = _dbContext.Database.GetDbConnection();
        const string query = "SELECT * FROM Employee"; // Suponiendo que la tabla se llama 'Employees'
        return await connection.QueryAsync<EmployeeDto>(query);
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
    {
        using var connection = _dbContext.Database.GetDbConnection();
        const string query = "SELECT * FROM Employee WHERE Id = @Id";
        return await connection.QueryFirstOrDefaultAsync<EmployeeDto>(query, new { Id = id });
    }
}
