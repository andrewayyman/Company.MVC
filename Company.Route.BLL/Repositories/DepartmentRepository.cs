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
    // THis Repository is used to perform CRUD operations on Department Table
    // It's implements IDepartmentRepository interface
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context; // NULL , readonly means it can be initialized only once [Constructor] cannot be changed , why not const? because const need to be initialized at the time of declaration
        public DepartmentRepository(AppDbContext context ) // dependency injection , Ask CLR to create object of AppDbContext when creating object of DepartmentRepository
        {                                                  // Add Services in program.cs , AddDbContext service or AddScoped service
            _context = context;            
        }

        public IEnumerable<Department> GetAll()
        {
            return _context.Departments.ToList();
        }

        public Department GetById( int id )
        {
            //return _context.Departments.FirstOrDefault(d => d.Id == id);
            // OR
            return _context.Departments.Find(id); //used to get the entity by its primary key

        }

        public int Add( Department entity )
        {
            _context.Departments.Add(entity);
            return _context.SaveChanges();
        }

        public int Update( Department entity )
        {
            _context.Departments.Update(entity);
            return _context.SaveChanges();
        }

        public int Delete( Department entity )
        {
            _context.Departments.Remove(entity);
            return _context.SaveChanges();
        }

    }
}
