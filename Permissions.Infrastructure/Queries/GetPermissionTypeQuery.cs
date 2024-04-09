using MediatR;
using Permissions.Domain.Dtos;

namespace Permissions.Infrastructure.Queries
{
  public record GetPermissionTypeQuery : IRequest<IEnumerable<PermissionTypeDto>>;
}
