using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;

public class EmployeeWriteServiceTest
{
    private readonly Mock<ApplicationDbContext> _mockDbContext;
    private readonly Mock<DbSet<Employee>> _mockEmployeeSet;
    private readonly EmployeeWriteService _service;

    public EmployeeWriteServiceTest()
    {
        _mockDbContext = new Mock<ApplicationDbContext>();
        _mockEmployeeSet = new Mock<DbSet<Employee>>();

        _mockDbContext.Setup(db => db.Employee).Returns(_mockEmployeeSet.Object);

        _service = new EmployeeWriteService(_mockDbContext.Object);
    }

    [Fact]
    public async Task AddEmployeeAsync_ShouldAddEmployeeAndReturnDto()
    {
        // Arrange
        var newEmployeeDto = new EmployeeDto
        {
            companyid = 1,
            createdon = new DateTime(2000, 1, 1),
            deletedon = new DateTime(2000, 1, 1),
            email = "test1@test.test.tmp",
            fax = "000.000.000",
            name = "test1",
            lastlogin = new DateTime(2000, 1, 1),
            password = "test",
            portalid = 1,
            roleid = 1,
            statusid = 1,
            telephone = "000.000.000",
            updatedon = new DateTime(2000, 1, 1),
            username = "test1"
        };

        var addedEmployee = new Employee { id = 1 };

        _mockEmployeeSet.Setup(m => m.Add(It.IsAny<Employee>())).Callback<Employee>(e => e.id = addedEmployee.id);

        // Act
        var result = await _service.AddEmployeeAsync(newEmployeeDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(addedEmployee.id, result.id);
        Assert.Equal(newEmployeeDto.companyid, result.companyid);
        Assert.Equal(newEmployeeDto.createdon, result.createdon);
        Assert.Equal(newEmployeeDto.deletedon, result.deletedon);
        Assert.Equal(newEmployeeDto.email, result.email);
        Assert.Equal(newEmployeeDto.fax, result.fax);
        Assert.Equal(newEmployeeDto.name, result.name);
        Assert.Equal(newEmployeeDto.lastlogin, result.lastlogin);
        Assert.Equal(newEmployeeDto.password, result.password);
        Assert.Equal(newEmployeeDto.portalid, result.portalid);
        Assert.Equal(newEmployeeDto.roleid, result.roleid);
        Assert.Equal(newEmployeeDto.statusid, result.statusid);
        Assert.Equal(newEmployeeDto.telephone, result.telephone);
        Assert.Equal(newEmployeeDto.updatedon, result.updatedon);
        Assert.Equal(newEmployeeDto.username, result.username);

        _mockEmployeeSet.Verify(m => m.Add(It.IsAny<Employee>()), Times.Once);
        _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
    }
     [Fact]
    public async Task UpdateEmployeeAsync_ShouldUpdateEmployee_WhenEmployeeExists()
    {
        // Arrange
        var existingEmployee = new Employee { id = 1, name = "Old Name", email = "old@test.com" };

        _mockEmployeeSet.Setup(m => m.FindAsync(1)).ReturnsAsync(existingEmployee);

        var updateDto = new UpdateEmployeeDto { name = "New Name", email = "new@test.com" };

        // Act
        var result = await _service.UpdateEmployeeAsync(1, updateDto);

        // Assert
        Assert.True(result);
        Assert.Equal(updateDto.name, existingEmployee.name);
        Assert.Equal(updateDto.email, existingEmployee.email);
        _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task UpdateEmployeeAsync_ShouldReturnFalse_WhenEmployeeDoesNotExist()
    {
        // Arrange
        _mockEmployeeSet.Setup(m => m.FindAsync(It.IsAny<int>())).ReturnsAsync((Employee?)null);

        var updateDto = new UpdateEmployeeDto { name = "New Name" };

        // Act
        var result = await _service.UpdateEmployeeAsync(1, updateDto);

        // Assert
        Assert.False(result);
        _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task DeleteEmployeeAsync_ShouldDeleteEmployee_WhenEmployeeExists()
    {
        // Arrange
        var existingEmployee = new Employee { id = 1 };

        _mockEmployeeSet.Setup(m => m.FindAsync(1)).ReturnsAsync(existingEmployee);

        // Act
        var result = await _service.DeleteEmployeeAsync(1);

        // Assert
        Assert.True(result);
        _mockEmployeeSet.Verify(m => m.Remove(existingEmployee), Times.Once);
        _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteEmployeeAsync_ShouldReturnFalse_WhenEmployeeDoesNotExist()
    {
        // Arrange
        _mockEmployeeSet.Setup(m => m.FindAsync(It.IsAny<int>())).ReturnsAsync((Employee?)null);

        // Act
        var result = await _service.DeleteEmployeeAsync(1);

        // Assert
        Assert.False(result);
        _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task ChangeEmployeeStatusAsync_ShouldUpdateStatusAndReturnTrue_WhenEmployeeExists()
    {
        // Arrange
        var existingEmployee = new Employee { id = 1, statusid = 1 };

        _mockEmployeeSet.Setup(m => m.FindAsync(1)).ReturnsAsync(existingEmployee);

        // Act
        var result = await _service.ChangeEmployeeStatusAsync(1, 0);

        // Assert
        Assert.True(result);
        Assert.Equal(0, existingEmployee.statusid);
        Assert.NotNull(existingEmployee.deletedon);
        _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task ChangeEmployeeStatusAsync_ShouldReturnFalse_WhenEmployeeDoesNotExist()
    {
        // Arrange
        _mockEmployeeSet.Setup(m => m.FindAsync(It.IsAny<int>())).ReturnsAsync((Employee?)null);

        // Act
        var result = await _service.ChangeEmployeeStatusAsync(1, 0);

        // Assert
        Assert.False(result);
        _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Never);
    }
}
