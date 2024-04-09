using Azure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Permissions.Domain.Dtos;
using Permissions.Domain.Entities;
using Permissions.Infrastructure.Commands;
using Permissions.Infrastructure.Queries;

namespace Permissions.API
{
  [ApiController]
  [Route("[controller]")]
  public class PermissionTypeController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly ILogger<PermissionController> _logger;
    public PermissionTypeController(IMediator mediator, ILogger<PermissionController> logger)
    {
      _mediator = mediator;
      _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<PermissionTypeDto>> Get()
    {
      var response = await _mediator.Send(new GetPermissionTypeQuery());
      _logger.LogInformation($"SERILOG-GetPermissions-{response.Count()} Permisos Consultados");
      return response;
    }

    [HttpPost]
    public async Task<ActionResult<PermissionTypeDto>> Create(CreatePermissionTypeCommand command)
    {
      var permissionType = await _mediator.Send(command);
      _logger.LogInformation($"SERILOG-CreatePermissionType-{permissionType.Description}");
      return CreatedAtAction(nameof(Get), permissionType);
    }
  }
}
