using MediatR;
using Permissions.Domain.Dtos;

namespace Permissions.Infrastructure.Commands
{
  public record RequestPermissionCommand(string EmployeeForename,
                                         string EmployeeSurname,
                                         int PermissionsType,
                                         DateTime PermissionsDate) : IRequest<PermissionDto>;

}
