using AutoMapper;
using Company.Route.BLL.Interfaces;
using Company.Route.BLL.Repositories;
using Company.Route.DAL.Models;
using Company.Route.PL.Helpers;
using Company.Route.PL.ViewModels.Employess;
using Microsoft.AspNetCore.Mvc;


namespace Company.Route.PL.Controllers
{

    #region Request Flow After UnitofWork
    // Now Communication is  ::::: Now Controllers -->> UnitOfWork -->> Repositories -->> DbContext

    // The Full request flow from request to response
    // 1. Request from client to controller using route of the action
    // 2. Controller get the request and call the unitofwork to get the data from database
    // 3. Unitofwork call the repository to get the data from database
    // 4. Repository get the data from database using the DbContext
    // 5. Repository return the data to the unitofwork
    // 6. Unitofwork return the data to the controller
    // 7. Controller return the data to the client as response [view in mvc]

    #endregion

    public class EmployeeController : Controller
    {
        /// Allow for interface not concrete class
        ///private readonly IEmployeeRepository _employeeRepository; // Null 
        ///private readonly IDepartmentRepository _departmentRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(

            /// NOTE :: After using unitofwork no need to inject repositories we inject unitofwork and use it to access the repositories
            ///IEmployeeRepository employeeController,                                    
            ///IDepartmentRepository departmentRepository,

            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            ///_employeeRepository = employeeController;
            ///_departmentRepository = departmentRepository;

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }




        #region Index Actions

        ///=>> in case of search feature u need to make it post and u need to make it get to getall the employees both in same action then make it without any specified method it will be hybrid 
        ///[HttpGet] ,, if u dont specify the request method it will be flexible :: get when need , post when need
        public async Task<IActionResult> Index( string InputSearch )
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
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(InputSearch);
            }

            var mappedEmp = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmp);




        }
        #endregion

        #region Create Actions 

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            // Use ViewDict to send extra info from request to view ViewData , ViewBag , TempData

            // 1 ViewData
            ViewData["Departments"] = departments;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create( EmployeeViewModel model )
        {
            // 3. TempData => Action to action

            if ( ModelState.IsValid )
            {
                // Upload Image
                if ( model.Image is not null )
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");

                }


                // Mapping EmployeeViewModel -->> Employee
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

                // 2. AutoMapper
                var employee = _mapper.Map<EmployeeViewModel, Employee>(model);






                await _unitOfWork.EmployeeRepository.AddAsync(employee); // state changed to added
                var Count = await _unitOfWork.CompleteAsync(); // Save Changes
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
        public async Task<IActionResult> Details( int? id, string viewname = "Details" )
        {
            if ( id is null ) return BadRequest();

            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);

            if ( employee is null ) return NotFound();

            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

            return View(viewname, employeeViewModel);


        }

        #endregion

        #region Edit Actions

        [HttpGet]
        public async Task<IActionResult> Edit( int? id )
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            // Use ViewDict to send extra info from request to view ViewData , ViewBag , TempData

            // 1 ViewData
            ViewData["Departments"] = departments;
            return await Details(id, "Edit");
        }


        [HttpPost] // FromRoute is to bind the id frm segment only to don't make any conflict
        [ValidateAntiForgeryToken] // to allow only request from ur client side [used usually with post method in MVC APP]
        public async Task<IActionResult> Edit( [FromRoute] int? id, EmployeeViewModel model )
        {
            try
            {
                if ( model.Image is not null )
                {
                    DocumentSettings.DeleteFile(model.ImageName, "images");
                }

                if ( model.Image is not null )
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }



                var employee = _mapper.Map<Employee>(model);

                if ( id != model.Id ) return BadRequest(); // Then the id in segment not like the sent from the form 

                _unitOfWork.EmployeeRepository.Update(employee);
                var Count = await _unitOfWork.CompleteAsync();
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
        public async Task<IActionResult> Delete( int? id )
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        public async Task<IActionResult> Delete( [FromRoute] int? id, EmployeeViewModel model )
        {
            try
            {
                var employee = _mapper.Map<EmployeeViewModel, Employee>(model);

                if ( id != model.Id ) return BadRequest();
                if ( ModelState.IsValid )
                {
                    _unitOfWork.EmployeeRepository.Delete(employee);
                    var Count = await _unitOfWork.CompleteAsync();
                    if ( Count > 0 )
                    {
                        if ( model.Image is not null )
                        {
                            DocumentSettings.DeleteFile(model.ImageName, "images");
                        }

                        return RedirectToAction(nameof(Index));
                    }

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
