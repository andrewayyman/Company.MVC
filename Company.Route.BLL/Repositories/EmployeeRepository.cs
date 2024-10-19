using Company.Route.BLL.Interfaces;
using Company.Route.DAL.Data.Contexts;
using Company.Route.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        //We already inherit this from the ctor of generic repo then we chain in the ctor of tha parent to inject 


        public EmployeeRepository( AppDbContext context ) : base(context)
        {
            //_context = context;
        }

        public async Task<IEnumerable<Employee>> GetByNameAsync( string name )
        {
            return await _context.Employees.Where(E => E.Name.ToLower().Contains(name.ToLower()))
                                     .Include(E => E.WorkFor)
                                     .ToListAsync();
        }



 

    }

}
