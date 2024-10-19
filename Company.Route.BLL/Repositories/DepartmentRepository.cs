using Company.Route.BLL.Interfaces;
using Company.Route.DAL.Data.Contexts;
using Company.Route.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route.BLL.Repositories
{

    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {




        public DepartmentRepository( AppDbContext context ) : base(context) 
        {                
            
        }



    }
}
