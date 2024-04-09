using Azure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Permissions.Domain.Dtos;
using Permissions.Infrastructure.Commands;
using Permissions.Infrastructure.Queries;

namespace Permissions.API
{
  [ApiController]
  [Route("[controller]")]
  public class PermissionController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly ILogger<PermissionController> _logger;
    public PermissionController(IMediator mediator, ILogger<PermissionController> logger)
    {
      _mediator = mediator;
      _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<PermissionDto>> GetPermissions()
    {
      var response = await _mediator.Send(new GetPermissionsQuery());
      _logger.LogInformation($"SERILOG-GetPermissions-{response.Count()} Permisos Consultados");
      return response;
    }

    [HttpPost]
    public async Task<ActionResult<PermissionDto>> RequestPermission(RequestPermissionCommand command)
    {
      var permission = await _mediator.Send(command);
      _logger.LogInformation($"SERILOG-RequestPermission-{permission.EmployeeSurname} Asociado a PermisoType {permission.PermissionType}");
      return CreatedAtAction(nameof(GetPermissions), permission);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PermissionDto>> ModifyPermission(int id, ModifyPermissionCommand command)
    {
      if (id != command.Id)
      {
        return BadRequest("Id mismatch");
      }

      var permission = await _mediator.Send(command);

      if (permission == null)
      {
        return NotFound("Permission not found");
      }

      _logger.LogInformation($"SERILOG - Permission modified - Id: {permission.Id}, EmployeeSurname: {permission.EmployeeSurname}, PermissionType: {permission.PermissionType}");

      return Ok(permission);
    }

  }

}
