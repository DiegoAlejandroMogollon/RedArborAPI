using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

public class EmployeeReadServiceTest
{
    private readonly Mock<DbConnection> _mockConnection;
    private readonly Mock<ApplicationDbContext> _mockDbContext;
    private readonly EmployeeReadService _service;

    public EmployeeReadServiceTest()
    {
        // Configuración del mock para DbConnection
        _mockConnection = new Mock<DbConnection>();
        _mockDbContext = new Mock<ApplicationDbContext>();

        // Configuramos para que GetDbConnection devuelva la conexión mockeada
        _mockDbContext.Setup(db => db.Database.GetDbConnection()).Returns(_mockConnection.Object);

        // Instanciamos el servicio bajo prueba
        _service = new EmployeeReadService(_mockDbContext.Object);
    }

    [Fact]
    public async Task GetAllEmployeesAsync_ShouldReturnAllEmployees()
    {
        // Arrange
        var expectedEmployees = new List<EmployeeDto>
        {
            new EmployeeDto { id = 1 },
            new EmployeeDto { id = 2 }
        };

        // Configuramos el mock para que devuelva los empleados esperados
        _mockConnection
            .Setup(conn => conn.QueryAsync<EmployeeDto>(It.IsAny<string>(), null, null, null, null))
            .ReturnsAsync(expectedEmployees);

        // Act
        var result = await _service.GetAllEmployeesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedEmployees.Count, result.Count());
        Assert.Equal(expectedEmployees.First().id, result.First().id);
    }

    [Fact]
    public async Task GetEmployeeByIdAsync_ShouldReturnEmployee_WhenEmployeeExists()
    {
        var expectedEmployee = new EmployeeDto { id = 1 };

        _mockConnection
            .Setup(conn => conn.QueryFirstOrDefaultAsync<EmployeeDto>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
            .ReturnsAsync(expectedEmployee);


        var result = await _service.GetEmployeeByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(expectedEmployee.id, result.id);
    }

    [Fact]
    public async Task GetEmployeeByIdAsync_ShouldReturnNull_WhenEmployeeDoesNotExist()
    {

        _mockConnection
            .Setup(conn => conn.QueryFirstOrDefaultAsync<EmployeeDto>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
            .ReturnsAsync((EmployeeDto?)null);

        var result = await _service.GetEmployeeByIdAsync(99); 

        Assert.Null(result);
    }
}
