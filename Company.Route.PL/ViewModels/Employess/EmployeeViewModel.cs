using Company.Route.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Company.Route.PL.ViewModels.Employess
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name IS Required !")]
        public string Name { get; set; }

        [Range(25, 60, ErrorMessage = "Age must be betweeen 25 and 60 ")]
        public int? Age { get; set; }
        [RegularExpression(@"^\d{1,}-[a-zA-Z]+-[a-zA-Z]+-[a-zA-Z]+$", ErrorMessage = "Address must follow the pattern 123-street-city-country.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Salary IS Required !")]
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime HiringDate { get; set; }

        // Relations
        [DisplayName("Department")]
        public Department? WorkFor { get; set; } // navigation prop
        public int? WorkForID { get; set; }     // foreign key

        public IFormFile? Image { get; set; }
        public string? ImageName { get; set; }

    }
}
