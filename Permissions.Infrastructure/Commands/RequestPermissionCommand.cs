using MediatR;
using Permissions.Domain.Dtos;

namespace Permissions.Infrastructure.Commands
{
  public record CreatePermissionTypeCommand(string Description) : IRequest<PermissionTypeDto>;

}
