using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Permissions.Domain.Entities
{
    public class Permission
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

        public PermissionType PermissionType { get; set; }
    }
}
