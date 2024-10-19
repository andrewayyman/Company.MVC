using Company.Route.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        // ----------  IN GENERIC REPO CLASS ----------------- // 


        // ------------- ADD Any specifed method for the employee clas u need ---------------------- //

        Task<IEnumerable<Employee>> GetByNameAsync( string name );

    }
}
