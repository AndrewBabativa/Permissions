using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Permissions.Domain.Dtos;
using Permissions.Domain.Entities;
using Permissions.Infrastructure;
using Permissions.Infrastructure.Queries;

namespace Permissions.Application.Handlers
{
  public class GetPermissionHandler : IRequestHandler<GetPermissionsQuery, IEnumerable<PermissionDto>>
  {
    private readonly UnitOfWork _unitOfWork;
    private readonly ElasticsearchService<Permission> _elasticsearchService;
    private readonly KafkaProducer _kafkaProducer;

    public GetPermissionHandler(UnitOfWork unitOfWork, ElasticsearchService<Permission> elasticsearchService, KafkaProducer kafkaProducer)
    {
      _unitOfWork = unitOfWork;
      _elasticsearchService = elasticsearchService;
      _kafkaProducer = kafkaProducer;
    }
    public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
      var permissions = await _unitOfWork.GetRepository<Permission>().GetAllAsync();
      var message = JsonConvert.SerializeObject(new { Id = Guid.NewGuid(), Name = "get" });
      await _kafkaProducer.ProduceMessageAsync("permissions-topic", message);

      return permissions.Select(p => new PermissionDto
      {
        Id = p.Id,
        EmployeeForename = p.EmployeeForename,
        EmployeeSurname = p.EmployeeSurname,
        PermissionsDate = p.PermissionsDate,
        PermissionsType = p.PermissionsType,
      });
    }
  }
}
