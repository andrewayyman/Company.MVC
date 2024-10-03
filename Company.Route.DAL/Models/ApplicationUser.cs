using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route.DAL.Models
{
    // this class is to represent our user instead of identityuser 
    // InCase if u want to add more porperties to ur user then we make that and inherits from identityuser the rest of the properties


    public class ApplicationUser : IdentityUser
    {
        public bool IsAgree { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }   


    }
}
