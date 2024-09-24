using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route.DAL.Models
{
    // Represent model in db 
    public class Employee : BaseEntity
    {

        //public int Id { get; set; }

        public string Name { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; } 
        public decimal Salary { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false; // soft delete
        public DateTime HiringDate { get; set; }
        public DateTime DateOfCreation { get; set; } = DateTime.Now;

        // Relations
        public Department? WorkFor { get; set; } // navigation prop
        public int? WorkForID { get; set; }     // foreign key

        // Image 
        public string? ImageName { get; set; }



    }
}
