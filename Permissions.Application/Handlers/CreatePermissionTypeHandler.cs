using MediatR;
using Permissions.Infrastructure;
using Permissions.Domain.Entities;
using Permissions.Domain.Dtos;
using Permissions.Infrastructure.Commands;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Permissions.Application.Handlers
{
  public class CreatePermissionTypeHandler
      : IRequestHandler<CreatePermissionTypeCommand, PermissionTypeDto>
  {
    private readonly UnitOfWork _unitOfWork;
    private readonly ElasticsearchService<PermissionType> _elasticsearchService;
    private readonly KafkaProducer _kafkaProducer;

    public CreatePermissionTypeHandler(UnitOfWork unitOfWork, ElasticsearchService<PermissionType> elasticsearchService, KafkaProducer kafkaProducer)
    {
      _unitOfWork = unitOfWork;
      _elasticsearchService = elasticsearchService;
      _kafkaProducer = kafkaProducer;
    }

    public async Task<PermissionTypeDto> Handle(CreatePermissionTypeCommand request, CancellationToken cancellationToken)
    {
      try
      {
        var permissionTypeRepository = _unitOfWork.GetRepository<PermissionType>();
        var permissionType = new PermissionType
        {
          Description = request.Description
        };

        permissionTypeRepository.Add(permissionType);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _elasticsearchService.CreateDocumentAsync(permissionType);
        var message = JsonConvert.SerializeObject(new { Id = Guid.NewGuid(), Name = "create" });
        await _kafkaProducer.ProduceMessageAsync("permissions-topic", message);

        return new PermissionTypeDto
        {
          Description = request.Description
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
