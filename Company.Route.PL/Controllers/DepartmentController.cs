using Company.Route.BLL.Interfaces;
using Company.Route.BLL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Company.Route.PL.Controllers
{
    public class DepartmentController : Controller
    {
        // Allow for interface not concrete class
        private readonly IDepartmentRepository _departmentRepository; // Null 
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }


        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();
            return View(departments);
        }
    }
}
