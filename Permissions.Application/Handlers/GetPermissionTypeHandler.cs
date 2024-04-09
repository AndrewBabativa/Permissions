using MediatR;
using Newtonsoft.Json;
using Permissions.Domain.Dtos;
using Permissions.Domain.Entities;
using Permissions.Infrastructure;
using Permissions.Infrastructure.Queries;

namespace Permissions.Application.Handlers
{
  public class GetPermissionTypeHandler : IRequestHandler<GetPermissionTypeQuery, IEnumerable<PermissionTypeDto>>
  {
    private readonly UnitOfWork _unitOfWork;
    private readonly KafkaProducer _kafkaProducer;

    public GetPermissionTypeHandler(UnitOfWork unitOfWork, KafkaProducer kafkaProducer)
    {
      _unitOfWork = unitOfWork;
      _kafkaProducer = kafkaProducer;
    }

    public async Task<IEnumerable<PermissionTypeDto>> Handle(GetPermissionTypeQuery request, CancellationToken cancellationToken)
    {
      var permissions = await _unitOfWork.GetRepository<PermissionType>().GetAllAsync();
      var message = JsonConvert.SerializeObject(new { Id = Guid.NewGuid(), Name = "get" });
      await _kafkaProducer.ProduceMessageAsync("permissions-topic", message);

      return permissions.Select(p => new PermissionTypeDto
      {
        Id = p.Id,
        Description = p.Description
      });
    }
  }
}
