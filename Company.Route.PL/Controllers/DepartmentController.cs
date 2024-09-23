using Company.Route.BLL.Interfaces;
using Company.Route.BLL.Repositories;
using Company.Route.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.Route.PL.Controllers
{
    public class DepartmentController : Controller
    {
        // Allow for interface not concrete class
        //private readonly IDepartmentRepository _departmentRepository; // Null 
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController ( 

            //IDepartmentRepository departmentRepository, 
            IUnitOfWork UnitOfWork
            
        )
        {
            _unitOfWork = UnitOfWork;
            //_departmentRepository = departmentRepository;
        }


        #region Index Actions 
        [HttpGet] // Default
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            return View(departments);
        }

        #endregion

        #region Create Actions


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create( Department model )
        {
            if ( ModelState.IsValid )
            {
                _unitOfWork.DepartmentRepository.Add(model);
                var Count = _unitOfWork.Complete();
                if ( Count > 0 ) return RedirectToAction(nameof(Index));
            }


            return View(model);
        }

        #endregion

        #region Details Actions
        [HttpGet]
        public IActionResult Details( int? id, string viewName = "Details" ) // passing view for refactoring the code , use it in any action with same bhaviour but change the returned view
        {
            if ( id is null ) return BadRequest(); // 400
            var department = _unitOfWork.DepartmentRepository.GetById(id.Value);
            if ( department == null ) return NotFound();
            return View(viewName, department); // another overload

        } 
        #endregion

        #region Edit Actions

        [HttpGet]
        public IActionResult Edit( int? id )
        {
            #region Before Refactoring

            //    if ( id is null ) return BadRequest();
            //    var department = _departmentRepository.GetById(id.Value);
            //    if ( department == null ) return NotFound();
            //    return View(department);
            #endregion

            return Details(id, "Edit");
        }


        [HttpPost] // FromRoute is to bind the id frm segment only to don't make any conflict
        [ValidateAntiForgeryToken] // to allow only request from ur client side [used usually with post method in MVC APP]
        public IActionResult Edit( [FromRoute] int? id, Department model )
        {
            try
            {
                if ( id != model.Id ) return BadRequest(); // Then the id in segment not like the sent from the form 

                _unitOfWork.DepartmentRepository.Update(model);
                var Count = _unitOfWork.Complete();
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

            #region Before Refactoring
            //if ( id is null ) return BadRequest();
            //var department = _departmentRepository.GetById(id.Value);
            //if ( department is null ) return NotFound();
            //return View(department); 
            #endregion
            return Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete( [FromRoute] int? id, Department model )
        {
            try
            {
                if ( id != model.Id ) return BadRequest();

                if ( ModelState.IsValid )
                {
                    _unitOfWork.DepartmentRepository.Delete(model);
                    var Count = _unitOfWork.Complete();
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


    }


}

