using Company.Route.BLL.Interfaces;
using Company.Route.BLL.Repositories;
using Company.Route.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.Route.PL.Controllers
{
    public class DepartmentController : Controller
    {
        // Allow for interface not concrete class
        private readonly IDepartmentRepository _departmentRepository; // Null 
        public DepartmentController( IDepartmentRepository departmentRepository )
        {
            _departmentRepository = departmentRepository;
        }


        [HttpGet] // Default
        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();
            return View(departments);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create( Department model )
        {
            if(ModelState.IsValid)
            {
            var Count = _departmentRepository.Add(model);
            if ( Count > 0 ) return RedirectToAction(nameof(Index));
            }    


            return View(model);
        }

        public IActionResult Details( int? id )
        {
            if ( id is null ) return BadRequest(); // 400

            var department = _departmentRepository.GetById(id.Value);

            if ( department == null ) return NotFound();

            return View(department);

        }





    }


}

