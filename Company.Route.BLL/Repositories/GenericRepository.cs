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
    // now we make the Repository in only Can be valid if the class is type of BaseEntity(Employee,Department)
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {

        private readonly AppDbContext _context;
        public GenericRepository( AppDbContext context )
        {
            _context = context;
        }


        public IEnumerable<T> GetAll()
        {
            // Must use eager loading "Include" to get the fk
            if(typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) _context.Employees.Include(E=>E.WorkFor).ToList();
            }
            return _context.Set<T>().ToList();
        }

        public T GetById( int id )
        {

            return _context.Set<T>().Find(id);
        }

        public int Add( T entity )
        {
            _context.Set<T>().Add(entity);
            return _context.SaveChanges();
        }

        public int Update( T entity )
        {
            _context.Set<T>().Update(entity);
            return _context.SaveChanges();
        }

        public int Delete( T entity )
        {
            _context.Set<T>().Remove(entity);
            return _context.SaveChanges();
        }


    }
}
