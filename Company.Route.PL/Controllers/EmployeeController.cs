using AutoMapper;
using Company.Route.BLL.Interfaces;
using Company.Route.BLL.Repositories;
using Company.Route.DAL.Models;
using Company.Route.PL.Helpers;
using Company.Route.PL.ViewModels.Employess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Company.Route.PL.Controllers
{


    [Authorize]

    public class EmployeeController : Controller
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(

            /// NOTE :: After using unitofwork no need to inject repositories we inject unitofwork and use it to access the repositories
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }




        #region Index Actions

       
        public async Task<IActionResult> Index( string InputSearch )
        {


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

        [Authorize(Roles = "Admin")]
        #region Create Actions 

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
          
            ViewData["Departments"] = departments;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create( EmployeeViewModel viewModel )
        {

            if ( ModelState.IsValid )
            {
                // Upload Image
                if ( viewModel.Image is not null )
                {
                    viewModel.ImageName = DocumentSettings.UploadFile(viewModel.Image, "images");

                }

                // AutoMapper
                var employee = _mapper.Map<EmployeeViewModel, Employee>(viewModel);






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

            return View(viewModel);

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

        [Authorize(Roles = "Admin")]
        #region Edit Actions

        [HttpGet]
        public async Task<IActionResult> Edit( int? id )
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
          
            ViewData["Departments"] = departments;
            return await Details(id, "Edit");
        }


        [HttpPost] 
        [ValidateAntiForgeryToken] // to allow only request from ur client side [used usually with post method in MVC APP]
        public async Task<IActionResult> Edit( [FromRoute] int? id, EmployeeViewModel viewModel )
        {
            try
            {
                if ( viewModel.Image is not null )
                {
                    DocumentSettings.DeleteFile(viewModel.ImageName, "images");
                }

                if ( viewModel.Image is not null )
                {
                    viewModel.ImageName = DocumentSettings.UploadFile(viewModel.Image, "images");
                }



                var employee = _mapper.Map<Employee>(viewModel);

                if ( id != viewModel.Id ) return BadRequest(); 

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


            return View(viewModel);


        }

        #endregion

        [Authorize(Roles = "Admin")]
        #region Delete Actions
        [HttpGet]
        public async Task<IActionResult> Delete( int? id )
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        public async Task<IActionResult> Delete( [FromRoute] int? id, EmployeeViewModel viewModel )
        {
            try
            {
                var employee = _mapper.Map<EmployeeViewModel, Employee>(viewModel);

                if ( id != viewModel.Id ) return BadRequest();
                if ( ModelState.IsValid )
                {
                    _unitOfWork.EmployeeRepository.Delete(employee);
                    var Count = await _unitOfWork.CompleteAsync();
                    if ( Count > 0 )
                    {
                        if ( viewModel.Image is not null )
                        {
                            DocumentSettings.DeleteFile(viewModel.ImageName, "images");
                        }

                        return RedirectToAction(nameof(Index));
                    }

                }

            }
            catch ( Exception ex )
            {
                ModelState.AddModelError(string.Empty, ex.Message);

            }
            return View(viewModel);

        }


        #endregion

    }
}
