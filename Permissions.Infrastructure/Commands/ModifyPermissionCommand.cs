using MediatR;
using Permissions.Domain.Dtos;

namespace Permissions.Infrastructure.Commands
{
  public record ModifyPermissionCommand(int Id,
                                        string EmployeeForename,
                                        string EmployeeSurname,
                                        int PermissionsType,
                                        DateTime PermissionsDate) : IRequest<PermissionDto>;

}
