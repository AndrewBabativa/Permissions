using Xunit;
using Moq;
using System.Collections.Generic;
using Permissions.Domain.Dtos;
using Permissions.Infrastructure.Commands;
using Permissions.Infrastructure.Queries;
using Microsoft.Extensions.Logging;
using Permissions.API;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class PermissionControllerTests
{
  private readonly Mock<IMediator> _mediatorMock;
  private readonly Mock<ILogger<PermissionController>> _loggerMock;

  public PermissionControllerTests()
  {
    _mediatorMock = new Mock<IMediator>();
    _loggerMock = new Mock<ILogger<PermissionController>>();
  }

  [Fact]
  public async Task GetPermissions()
  {

    var permissions = new List<PermissionDto> { new PermissionDto { Id = 1, EmployeeSurname = "Smith" } };
    _mediatorMock.Setup(m => m.Send(It.IsAny<GetPermissionsQuery>(), default)).ReturnsAsync(permissions);
    var controller = new PermissionController(_mediatorMock.Object, _loggerMock.Object);
    var actionResult = await controller.GetPermissions();
    var result = actionResult;

    Assert.Equal(permissions, result);
  }

  [Fact]
  public async Task RequestPermission()
  {
    // Arrange
    var command = new RequestPermissionCommand("EmployeeForename", "EmployeeSurname", 1, DateTime.Now);
    var permissionDto = new PermissionDto { EmployeeForename = command.EmployeeForename, EmployeeSurname = command.EmployeeSurname };

    _mediatorMock.Setup(m => m.Send(command, default(CancellationToken))).ReturnsAsync(permissionDto);
    var controller = new PermissionController(_mediatorMock.Object, _loggerMock.Object);

    // Act
    var result = await controller.RequestPermission(command);

    // Assert
    var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
    var returnValue = Assert.IsAssignableFrom<PermissionDto>(createdAtActionResult.Value);
    Assert.Equal(command.EmployeeForename, returnValue.EmployeeForename);
    Assert.Equal(command.EmployeeSurname, returnValue.EmployeeSurname);
  }

  [Fact]
  public async Task ModifyPermission()
  {
    // Arrange
    var id = 1;
    var command = new ModifyPermissionCommand(id, "John", "Doe", 1, DateTime.Now);
    var permissionDto = new PermissionDto
    {
      Id = id,
      EmployeeForename = command.EmployeeForename,
      EmployeeSurname = command.EmployeeSurname,
      PermissionsType = command.PermissionsType
    };

    _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(permissionDto);
    var controller = new PermissionController(_mediatorMock.Object, _loggerMock.Object);

    // Act
    var result = await controller.ModifyPermission(id, command);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var returnValue = Assert.IsAssignableFrom<PermissionDto>(okResult.Value);
    Assert.Equal(command.EmployeeForename, returnValue.EmployeeForename);
    Assert.Equal(command.EmployeeSurname, returnValue.EmployeeSurname);
    Assert.Equal(command.PermissionsType, returnValue.PermissionsType);
  }

}
