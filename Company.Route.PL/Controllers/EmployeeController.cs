using AutoMapper;
using Company.Route.BLL.Interfaces;
using Company.Route.BLL.Repositories;
using Company.Route.DAL.Models;
using Company.Route.PL.ViewModels.Employess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.Route.PL.Controllers
{
    public class EmployeeController : Controller
    {
        // Allow for interface not concrete class
        private readonly IEmployeeRepository _employeeRepository; // Null 
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController( IEmployeeRepository employeeController,
                                   IDepartmentRepository departmentRepository,
                                   IMapper mapper 
            )
        {
            _employeeRepository = employeeController;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }




        #region Index Actions

        ///=>> in case of search feature u need to make it post and u need to make it get to getall the employees both in same action then make it without any specified method it will be hybrid 
        ///[HttpGet] ,, if u dont specify the request method it will be flexible :: get when need , post when need
        public IActionResult Index( string InputSearch )
        {
            #region ViewData , ViewBag , TempData
            //// Extra info ? 
            //// Binding through views Using Dictionary [Key , Value] pair 
            //// inherited from controller class
            //// [ViewData , ViewBag] : transfer data from action to view or from view to view *ONE WAY* 
            //// [ViewTemp] : transfer data from action to action 

            //// 1. ViewData : Strongly Typed 
            //ViewData["Message"] = "Hello from ViewData";

            //// 2. ViewBAg : Dynamic Type "Weak data Type"
            //ViewBag.Message = "Hello from ViewBag"; // access with . 

            //// --------- NOTE THAT BOTH STORE IN SAME DICT THEN THEY OVERRIDE THE LAST VALUE STORES ---------- // 

            //// 3.TempData : transfer data from action to action , used as dict 
            //// Example on create method 
            #endregion


            var employees = Enumerable.Empty<Employee>();
            if ( string.IsNullOrEmpty(InputSearch) )
            {
                employees = _employeeRepository.GetAll();
            }
            else
            {
                employees = _employeeRepository.GetByName(InputSearch);
            }

            var mappedEmp = _mapper.Map <IEnumerable< EmployeeViewModel>> (employees);
            return View(mappedEmp);




        }
        #endregion

        #region Create Actions 

        [HttpGet]
        public IActionResult Create()
        {
            var departments = _departmentRepository.GetAll();
            // Use ViewDict to send extra info from request to view ViewData , ViewBag , TempData

            // 1 ViewData
            ViewData["Departments"] = departments;

            return View();
        }

        [HttpPost]
        public IActionResult Create( EmployeeViewModel model )
        {
            // 3. TempData => Action to action

            if ( ModelState.IsValid )
            {
                // We need Casting EmployeeViewModel -->> Employee  Which is mapping [Manual , Auto ]

                #region 1. Manual Mapping
                //// 1. Manual Mapping
                //Employee employee = new Employee()
                //{
                //    Id = model.Id,
                //    Address = model.Address,
                //    Name = model.Name,
                //    Salary = model.Salary,
                //    Age = model.Age,
                //    HiringDate = model.HiringDate,
                //    IsActive = model.IsActive,
                //    WorkFor = model.WorkFor,
                //    WorkForID = model.WorkForID,
                //    Email = model.Email,
                //    PhoneNumber = model.PhoneNumber,
                //}; 
                #endregion

                #region 2. Auto Mapper

                var employee = _mapper.Map<EmployeeViewModel, Employee>(model);


                #endregion


                var Count = _employeeRepository.Add(employee);
                if ( Count > 0 )
                {
                    TempData["Message"] = "Employee Created Succefully !";
                }
                else
                {
                    TempData["Message"] = "An Error Occurred  !";
                }
                return RedirectToAction(nameof(Index));
            }

            return View(model);

        }



        #endregion

        #region Details Actions
        [HttpGet]
        public IActionResult Details( int? id, string viewname = "Details" )
        {
            if ( id is null ) return BadRequest();
            var employee = _employeeRepository.GetById(id.Value);
            if ( employee is null ) return NotFound();


            return View(viewname, employee);


        }





        #endregion

        #region Edit Actions

        [HttpGet]
        public IActionResult Edit( int? id )
        {
            var departments = _departmentRepository.GetAll();
            // Use ViewDict to send extra info from request to view ViewData , ViewBag , TempData

            // 1 ViewData
            ViewData["Departments"] = departments;
            return Details(id, "Edit");
        }


        [HttpPost] // FromRoute is to bind the id frm segment only to don't make any conflict
        [ValidateAntiForgeryToken] // to allow only request from ur client side [used usually with post method in MVC APP]
        public IActionResult Edit( [FromRoute] int? id, EmployeeViewModel model )
        {
            try
            {

                var employee = _mapper.Map<Employee>(model);

                if ( id != model.Id ) return BadRequest(); // Then the id in segment not like the sent from the form 

                var Count = _employeeRepository.Update(employee);
                if ( ModelState.IsValid )
                {
                    if ( Count > 0 ) return RedirectToAction(nameof(Index));
                }

            }
            catch ( Exception ex )
            {
                ModelState.AddModelError(string.Empty, ex.Message); // is to 
            }


            return View(model);


        }

        #endregion

        #region Delete Actions
        [HttpGet]
        public IActionResult Delete( int? id )
        {


            return Details(id, "Delete");
        }


        [HttpPost]
        public IActionResult Delete( [FromRoute] int? id, EmployeeViewModel model )
        {
            try
            {
                var employee = _mapper.Map<EmployeeViewModel, Employee>(model);

                if ( id != model.Id ) return BadRequest();
                if ( ModelState.IsValid )
                {
                    var Count = _employeeRepository.Delete(employee);
                    if ( Count > 0 ) { return RedirectToAction(nameof(Index)); }

                }

            }
            catch ( Exception ex )
            {
                ModelState.AddModelError(string.Empty, ex.Message);

            }
            return View(model);

        }

        #endregion

    }
}
