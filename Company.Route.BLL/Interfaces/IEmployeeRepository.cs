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
        // ---------- NOW IN GENERIC REPO CLASS ----------------- // 
        //IEnumerable<Employee> GetAll();
        //Employee GetById( int id );
        //int Add( Employee entity );
        //int Update( Employee entity );
        //int Delete( Employee entity );

        // ------------- ADD Any specifed method for the employee clas u need ---------------------- //

        Task<IEnumerable<Employee>> GetByNameAsync( string name );

    }
}
