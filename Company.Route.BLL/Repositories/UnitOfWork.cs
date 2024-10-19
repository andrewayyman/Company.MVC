using Company.Route.BLL.Interfaces;
using Company.Route.DAL.Data.Contexts;


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



       
        public void Dispose()
        {
            _context.Dispose();
        }

    }


}