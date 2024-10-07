using System.ComponentModel.DataAnnotations;

namespace Company.Route.PL.ViewModels.Account
{
    public class RoleViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        public string RoleName { get; set; }    
    }
}
