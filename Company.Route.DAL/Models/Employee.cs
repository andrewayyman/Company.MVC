using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route.DAL.Models
{
    public class Employee : BaseEntity
    {

        //public int Id { get; set; }

        [Required(ErrorMessage ="Name IS Required !")]
        public string Name { get; set; }

        [Range(25,60 , ErrorMessage ="Age must be betweeen 25 and 60 ")]
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
        public bool IsDeleted { get; set; }
        public DateTime HiringDate { get; set; }
        public DateTime DateOfCreation { get; set; } = DateTime.Now;



    }
}
