using MediatR;
using Permissions.Domain.Dtos;

namespace Permissions.Infrastructure.Queries
{
  public record GetPermissionsQuery : IRequest<IEnumerable<PermissionDto>>;
}
