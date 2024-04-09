using Confluent.Kafka;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Permissions.Domain.Dtos;
using Permissions.Domain.Entities;
using Permissions.Infrastructure;
using Permissions.Infrastructure.Commands;

namespace Permissions.Application.Handlers
{
  public class ModifyPermissionHandler : IRequestHandler<ModifyPermissionCommand, PermissionDto>
  {
    private readonly UnitOfWork _unitOfWork;
    private readonly ElasticsearchService<Permission> _elasticsearchService;
    private readonly KafkaProducer _kafkaProducer;

    public ModifyPermissionHandler(UnitOfWork unitOfWork, ElasticsearchService<Permission> elasticsearchService, KafkaProducer kafkaProducer)
    {
      _unitOfWork = unitOfWork;
      _elasticsearchService = elasticsearchService;
      _kafkaProducer = kafkaProducer;
    }

    public async Task<PermissionDto> Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
    {
      try
      {
        var permission = await _unitOfWork.GetRepository<Permission>().GetByIdAsync(request.Id);
        if (permission == null)
        {
          throw new Exception("El permiso especificado no existe.");
        }

        var permissionType = await _unitOfWork.GetRepository<PermissionType>().GetByIdAsync(request.PermissionsType);
        if (permissionType == null)
        {
          throw new Exception("El tipo de permiso especificado no es válido.");
        }

        permission.PermissionsDate = DateTime.Now;
        permission.EmployeeForename = request.EmployeeForename;
        permission.EmployeeSurname = request.EmployeeSurname;
        permission.PermissionsType = request.PermissionsType;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _elasticsearchService.CreateDocumentAsync(permission);
        var message = JsonConvert.SerializeObject(new { Id = Guid.NewGuid(), Name = "modify" });
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
