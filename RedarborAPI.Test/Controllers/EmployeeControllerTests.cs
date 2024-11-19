using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class EmployeeControllerTest
{
    private readonly Mock<IEmployeeReadService> _mockReadService;
    private readonly Mock<IEmployeeWriteService> _mockWriteService;
    private readonly EmployeeController _controller;

    public EmployeeControllerTest()
    {
        _mockReadService = new Mock<IEmployeeReadService>();
        _mockWriteService = new Mock<IEmployeeWriteService>();
        _controller = new EmployeeController(_mockReadService.Object, _mockWriteService.Object);
    }

    [Fact]
    public async Task GetAllEmployees_ShouldReturnOk_WhenEmployeesExist()
    {
        // Arrange
        var employees = new List<EmployeeDto>
        {
            new EmployeeDto { id = 1, name = "John Doe" }
        };
        _mockReadService.Setup(s => s.GetAllEmployeesAsync()).ReturnsAsync(employees);

        // Act
        var result = await _controller.GetAllEmployees();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task AddEmployee_ShouldReturnCreated_WhenEmployeeIsValid()
    {
        // Arrange
        var employeeDto = new EmployeeDto { name = "John Doe", email = "john@example.com", companyid = 1 };
        _mockWriteService.Setup(s => s.AddEmployeeAsync(It.IsAny<EmployeeDto>())).ReturnsAsync(employeeDto);

        // Act
        var result = await _controller.AddEmployee(employeeDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
        Assert.Equal(employeeDto, createdResult.Value);
    }
}
