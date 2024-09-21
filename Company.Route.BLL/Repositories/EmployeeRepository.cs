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


        //private readonly AppDbContext _context;
        public EmployeeRepository( AppDbContext context ) : base(context)
        {
            //_context = context;
        }

        public IEnumerable<Employee> GetByName( string name )
        {
            return _context.Employees.Where(E => E.Name.ToLower().Contains(name.ToLower()))
                                     .Include(E => E.WorkFor)
                                     .ToList();
        }



        #region Before Applying Generic Repo
        //public IEnumerable<Employee> GetAll()
        //{
        //    return _context.Employees.ToList();
        //}

        //public Employee GetById( int id )
        //{
        //    return _context.Employees.Find(id);
        //}

        //public int Add( Employee entity )
        //{
        //    _context.Employees.Add(entity);
        //    return _context.SaveChanges();
        //}


        //public int Update( Employee entity )
        //{
        //    _context.Employees.Update(entity);
        //    return _context.SaveChanges();
        //}


        //public int Delete( Employee entity )
        //{
        //    _context.Employees.Remove(entity);
        //    return _context.SaveChanges();
        //} 
        #endregion


    }

}
