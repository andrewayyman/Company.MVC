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

        private protected readonly AppDbContext _context; // to make it accessible within any children 
        public GenericRepository( AppDbContext context )
        {
            _context = context;
        }


        public async Task <IEnumerable<T>> GetAllAsync()
        {
            // Must use eager loading "Include" to get the fk
            if(typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await _context.Employees.Include(E=>E.WorkFor).ToListAsync();
            }
            return await _context.Set<T>().ToListAsync();
        }

        public async  Task<T> GetByIdAsync( int id )
        {

            return await _context.Set<T>().FindAsync(id);
        }

        public async Task AddAsync( T entity )
        {
            await _context.Set<T>().AddAsync(entity);
            //return _context.SaveChanges(); // No need we will save it in the unit of work
        }

        public void Update( T entity )
        {
            _context.Set<T>().Update(entity);
            //return _context.SaveChanges();
        }

        public void Delete( T entity )
        {
            _context.Set<T>().Remove(entity);
            //return _context.SaveChanges();
        }


    }
}
