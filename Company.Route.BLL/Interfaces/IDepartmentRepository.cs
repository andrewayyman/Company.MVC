using Company.Route.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route.BLL.Interfaces
{
    // This Repository is used to perform CRUD operations on Department Table
    // Generic Repository
    public interface IDepartmentRepository  : IGenericRepository<Department>
    {
        // ---------- NOW IN GENERIC REPO CLASS ----------------- // 
        //IEnumerable<Department> GetAll();
        //Department GetById( int id );
        //int Add( Department entity );
        //int Update( Department entity );
        //int Delete( Department entity );


    }
}
