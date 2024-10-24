﻿using Company.Route.BLL.Interfaces;
using Company.Route.BLL.Repositories;
using Company.Route.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Route.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
  
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController ( 

            IUnitOfWork UnitOfWork
            
        )
        {
            _unitOfWork = UnitOfWork;
        }


        #region Index Actions 
        [HttpGet] // Default
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(departments);
        }

        #endregion

        [Authorize(Roles = "Admin")]
        #region Create Actions


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create( Department model )
        {
            if ( ModelState.IsValid )
            {
                await _unitOfWork.DepartmentRepository.AddAsync(model);
                var Count = await _unitOfWork.CompleteAsync();
                if ( Count > 0 ) return RedirectToAction(nameof(Index));
            }


            return View(model);
        }

        #endregion

        #region Details Actions
        [HttpGet]
        public async Task<IActionResult> Details( int? id, string viewName = "Details" ) 
        {
            if ( id is null ) return BadRequest(); 
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if ( department == null ) return NotFound();
            return View(viewName, department); 

        }
        #endregion

        [Authorize(Roles = "Admin")]
        #region Edit Actions

        [HttpGet]
        public async Task<IActionResult> Edit( int? id )
        {
          

            return await Details(id, "Edit");
        }


        [HttpPost] 
        [ValidateAntiForgeryToken] // to allow only request from ur client side [used usually with post method in MVC APP]
        public async Task<IActionResult> Edit( [FromRoute] int? id, Department model )
        {
            try
            {
                if ( id != model.Id ) return BadRequest(); // Then the id in segment not like the sent from the form 

                _unitOfWork.DepartmentRepository.Update(model);
                var Count = await _unitOfWork.CompleteAsync();
                if ( ModelState.IsValid )
                {
                    if ( Count > 0 ) return RedirectToAction(nameof(Index));
                }

            }
            catch ( Exception ex )
            {
                ModelState.AddModelError(string.Empty, ex.Message); 
            }


            return View(model);


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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete( [FromRoute] int? id, Department model )
        {
            try
            {
                if ( id != model.Id ) return BadRequest();

                if ( ModelState.IsValid )
                {
                    _unitOfWork.DepartmentRepository.Delete(model);
                    var Count = await _unitOfWork.CompleteAsync();
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

