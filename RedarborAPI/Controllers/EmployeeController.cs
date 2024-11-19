using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
     private readonly IEmployeeReadService _employeeReadService;
    private readonly IEmployeeWriteService _employeeWriteService;

    public EmployeeController(IEmployeeReadService employeeReadService, IEmployeeWriteService employeeWriteService)
    {
        _employeeReadService = employeeReadService;
        _employeeWriteService = employeeWriteService;
    }

   /// <summary>
/// Obtiene todos los empleados.
/// </summary>
/// <returns>Lista de todos los empleados.</returns>
/// <response code="200">Devuelve una lista de empleados</response>
/// <response code="404">Si no hay empleados disponibles</response>
[HttpGet]
public async Task<IActionResult> GetAllEmployees()
{
    var employees = await _employeeReadService.GetAllEmployeesAsync();

    if (employees == null || !employees.Any())
    {
        return NotFound("No hay empleados disponibles.");
    }

    return Ok(employees);
}

   /// <summary>
/// Obtiene un empleado por su ID.
/// </summary>
/// <param name="id">El ID del empleado.</param>
/// <returns>El empleado correspondiente al ID, o un error 404 si no se encuentra.</returns>
/// <response code="200">Devuelve el empleado encontrado</response>
/// <response code="404">Si no se encuentra el empleado</response>


[HttpGet("{id:int}")]
public async Task<IActionResult> GetEmployeeById(int id)
{
    var employee = await _employeeReadService.GetEmployeeByIdAsync(id);

    if (employee == null)
    {
        return NotFound($"No se encontró un empleado con el ID {id}.");
    }

    return Ok(employee);
}
    /// <summary>
    /// Crea un nuevo empleado.
    /// </summary>
    /// <param name="employeeDto">DTO que contiene la información del nuevo empleado.</param>
    /// <returns>El empleado recién creado.</returns>
    /// <response code="201">Empleado creado exitosamente</response>
    /// <response code="400">Si el modelo enviado no es válido</response>
    [HttpPost]
public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto employeeDto)
{

    if (employeeDto.companyid <= 0 || 
        string.IsNullOrWhiteSpace(employeeDto.email) || 
        string.IsNullOrWhiteSpace(employeeDto.password) || 
        employeeDto.portalid <= 0 || 
        employeeDto.roleid <= 0 || 
        employeeDto.statusid <= 0 || 
        string.IsNullOrWhiteSpace(employeeDto.username))
    {
        return BadRequest("Todos los campos obligatorios deben tener un valor válido: CompanyId, Email, Password, PortalId, RoleId, StatusId y Username no pueden ser nulos ni vacíos.");
    }
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    var createdEmployee = await _employeeWriteService.AddEmployeeAsync(employeeDto);
    return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.id }, createdEmployee);
}

    /// <summary>
    /// Actualiza un empleado existente.
    /// </summary>
    /// <param name="id">El ID del empleado que se desea actualizar.</param>
    /// <param name="employeeDto">DTO con la información actualizada del empleado.</param>
    /// <returns>Resultado de la operación de actualización.</returns>
    /// <response code="204">Empleado actualizado exitosamente</response>
    /// <response code="400">Si el modelo enviado no es válido</response>
    /// <response code="404">Si no se encuentra el empleado para actualizar</response>
    [HttpPut("{id:int}")]
public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto updateEmployeeDto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    var updated = await _employeeWriteService.UpdateEmployeeAsync(id, updateEmployeeDto);
    if (!updated)
    {
        return NotFound();
    }

    return NoContent();
}

    /// <summary>
    /// Elimina un empleado por su ID.
    /// </summary>
    /// <param name="id">El ID del empleado a eliminar.</param>
    /// <returns>Resultado de la operación de eliminación.</returns>
    /// <response code="204">Empleado eliminado exitosamente</response>
    /// <response code="404">Si no se encuentra el empleado para eliminar</response>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var deleted = await _employeeWriteService.DeleteEmployeeAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

/// <summary>
    /// Actualizar estado de un empleado por su ID.
    /// </summary>
    /// <param name="id">El ID del empleado a actualizar.</param>
    /// <returns>Resultado de la operación de actualización de estado.</returns>
    /// <response code="204">Actualizado exitosamente</response>
    /// <response code="404">Si no se encuentra el empleado para eliminar</response>
[HttpPut("change-status/{id:int}")]
public async Task<IActionResult> ChangeEmployeeStatus(int id, [FromBody] int status)
{
    if (status != 0 && status != 1) // Verificamos que el status sea 0 o 1
    {
        return BadRequest("El estado debe ser 0 (Inactivo) o 1 (Activo).");
    }

    var updated = await _employeeWriteService.ChangeEmployeeStatusAsync(id, status);
    if (!updated)
    {
        return NotFound();
    }

    return NoContent();
}
}
