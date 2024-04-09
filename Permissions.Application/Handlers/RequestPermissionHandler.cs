using MediatR;
using Permissions.Infrastructure;
using Permissions.Domain.Entities;
using Permissions.Domain.Dtos;
using Permissions.Infrastructure.Commands;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Confluent.Kafka;

namespace Permissions.Application.Handlers
{
  public class RequestPermissionHandler
      : IRequestHandler<RequestPermissionCommand, PermissionDto>
  {
    private readonly UnitOfWork _unitOfWork;
    private readonly ElasticsearchService<Permission> _elasticsearchService;
    private readonly KafkaProducer _kafkaProducer;

    public RequestPermissionHandler(UnitOfWork unitOfWork, ElasticsearchService<Permission> elasticsearchService, KafkaProducer kafkaProducer)
    {
      _unitOfWork = unitOfWork;
      _elasticsearchService = elasticsearchService;
      _kafkaProducer = kafkaProducer;
    }

    public async Task<PermissionDto> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
    {
      try
      {
        var permissionType = await _unitOfWork.GetRepository<PermissionType>().GetByIdAsync(request.PermissionsType);

        if (permissionType == null)
        {
          throw new Exception("El tipo de permiso especificado no existe.");
        }

        var permissionRepository = _unitOfWork.GetRepository<Permission>();
        var permission = new Permission
        {
          EmployeeForename = request.EmployeeForename,
          EmployeeSurname = request.EmployeeSurname,
          PermissionsDate = DateTime.Now,
          PermissionsType = request.PermissionsType
        };

        permissionRepository.Add(permission);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _elasticsearchService.CreateDocumentAsync(permission);
        var message = JsonConvert.SerializeObject(new { Id = Guid.NewGuid(), Name = "request" });
        await _kafkaProducer.ProduceMessageAsync("permissions-topic", message);

        return new PermissionDto
        {
          Id = permission.Id,
          EmployeeForename = permission.EmployeeForename,
          EmployeeSurname = permission.EmployeeSurname,
          PermissionsDate = permission.PermissionsDate,
          PermissionsType = permission.PermissionsType
        };
      }
      catch (DbUpdateException ex)
      {
        if (ex.InnerException is SqlException sqlException && sqlException.Number == 547)
        {
          throw new Exception("El tipo de permiso especificado no es válido.");
        }
        throw;
      }
      catch (Exception ex)
      {
        throw new Exception("Error inesperado: " + ex.Message);
      }
    }
  }
}
