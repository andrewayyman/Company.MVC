using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route.BLL.Interfaces
{
    // Generic Repo class is to write the crud once and make the other repos that needs crud implements it and override another sepecific methods if needed
    public interface IGenericRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById( int id );
        void Add( T entity );
        void Update( T entity );
        void Delete( T entity );
    }
}
