using System.ComponentModel.DataAnnotations;

namespace Permissions.Domain.Dtos
{
  public class PermissionTypeDto
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public string Description { get; set; }
  }
}
