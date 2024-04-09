using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Permissions.Domain.Dtos
{
    public class PermissionDto
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public string EmployeeForename { get; set; }

    [Required]
    public string EmployeeSurname { get; set; }

    [ForeignKey("PermissionType")]
    public int PermissionsType { get; set; }

    public DateTime PermissionsDate { get; set; }

    public PermissionTypeDto PermissionType { get; set; }
  }
}
