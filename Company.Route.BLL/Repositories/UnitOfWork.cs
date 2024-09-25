using Company.Route.BLL.Interfaces;
using Company.Route.DAL.Data.Contexts;

// Now Communication is  ::::: Now Controllers -->> UnitOfWork -->> Repositories -->> DbContext


#region Unit of Work Design Pattern 
/*
 
 -------------- What is Unit OF Work DP ------------------------------------------
 The Unit of Work design pattern in ASP.NET Core MVC is a way to maintain a list of operations 
 (such as insert, update, delete) to be executed within a transaction and ensures that all operations
 succeed or fail as a single unit. It helps in managing database access by grouping multiple related 
 operations together and controlling when the data is persisted.
 ----------------------------------------------------------------------------


 -------------- Why Use Unit of Work ------------------------------------------
 - Transaction Management: Unit of Work ensures that a transaction is created to handle multiple database operations as a single operation, committing them only when all succeed.
 - Reduces Redundant Calls: It minimizes database calls by accumulating operations in memory and then persisting them in one go.
 - Centralizes Operations: By coordinating multiple repositories through a single Unit of Work, the pattern centralizes and simplifies database operations.
 - State Tracking and Change Management : DbContext tracks the state of the entities it retrieves (added, modified, deleted), 
                                          and at the end of the transaction, it commits changes to the database. By injecting the DbContext, 
                                          you ensure that all changes made through repositories or directly are tracked within the same context and persisted together. 
-------------------------------------------------------------------------------


 -------------- Steps to implement ------------------------------------------
 - 1 - Create Repositories:
 Define an interface and class for each entity repository (e.g., IDepartmentRepository and DepartmentRepository) 
 to handle operations like Add, Update, Remove, and GetById.
 
 - 2 - Create the Unit of Work Interface: 
 Define an interface (IUnitOfWork) that contains methods to save changes and access the repositories.
 
 - 3 - Implement the Unit of Work:
 Create a UnitOfWork class that coordinates multiple repositories and handles transaction management. 
 Implement the Complete method to save changes to the database.
 
 - 4 - Register Repositories and Unit of Work in Dependency Injection: 
 Register the repositories and UnitOfWork in the dependency injection container in 
 Program.cs or Startup.cs using AddScoped.
 
 - 5 - Use Unit of Work in the Controller:
 Inject the IUnitOfWork into the controller and use the repositories through it. 
 Call the Complete method to commit database changes.
 ------------------------------------------------------------------------------------

---------------- Communucation betwwen layers -----------------
   Now Communication is  ::::: Now Controllers -->> UnitOfWork -->> Repositories -->> DbContext


 */



#endregion

namespace Company.Route.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDepartmentRepository _DepartmentRepository;
        private IEmployeeRepository _EmployeeRepository;

        public UnitOfWork( AppDbContext context )
        {
            _context = context;
            _DepartmentRepository = new DepartmentRepository(context);
            _EmployeeRepository = new EmployeeRepository(context);
        }

        public IDepartmentRepository DepartmentRepository => _DepartmentRepository;
        public IEmployeeRepository EmployeeRepository => _EmployeeRepository;

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }



        // it will be executed automattically when request is done and the life time of the object is ended
        // but we need the clr to know that we've dispose method then we implement IDisposable interface to make sure that the dispose method is called
        public void Dispose()
        {
            _context.Dispose();
        }

    }


}