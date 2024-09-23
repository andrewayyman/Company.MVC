


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
 */



#endregion



namespace Company.Route.BLL.Interfaces
{
    // Contain all Repositories
    // To communicate withh database 
    public interface IUnitOfWork : IDisposable
    {
        public IDepartmentRepository DepartmentRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }
        int Complete(); // to replace it by savechanges COMMIT

        void Dispose();

    }
}
