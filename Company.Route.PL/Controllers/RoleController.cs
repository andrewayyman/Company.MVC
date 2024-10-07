using AutoMapper;
using Company.Route.DAL.Models;
using Company.Route.PL.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace Company.Route.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController( RoleManager<IdentityRole> roleManager )
        {
            _roleManager = roleManager;
        }



        #region Index Actions 
        public async Task<IActionResult> Index( string InputSearch )
        {
            var roles = Enumerable.Empty<RoleViewModel>();

            try
            {
                if ( string.IsNullOrEmpty(InputSearch) )
                {
                    roles = await _roleManager.Roles.Select(R => new RoleViewModel
                    {
                        Id = R.Id,
                        RoleName = R.Name


                    }).ToListAsync();

                }
                else // get search value
                {
                    roles = await _roleManager.Roles.Where(R => R.Name
                                                    .ToLower()
                                                    .Contains(InputSearch.ToLower()))
                                                    .Select(R => new RoleViewModel
                                                    {
                                                        Id = R.Id,
                                                        RoleName = R.Name
                                                    }).ToListAsync();


                }

            }
            catch ( Exception ex )
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(roles);
        }



        #endregion

        #region Create Actions 

        [HttpGet]
        public IActionResult Create ()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create (RoleViewModel viewModel)
        {
            if (ModelState.IsValid)

            {
                var role = new IdentityRole()
                {
                    Name = viewModel.RoleName
                };


                var IsCreated = await _roleManager.CreateAsync(role);
                if( IsCreated.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            if ( !ModelState.IsValid )
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach ( var error in errors )
                {
                    // You can log or inspect the error messages
                    Console.WriteLine(error.ErrorMessage);
                }
            }


            return View(viewModel);
        }


        #endregion

        #region Details Action

        [HttpGet]
        public async Task<IActionResult> Details( string? id, string viewname = "Details" )
        {
            if ( id is null ) return BadRequest();

            var role = await _roleManager.FindByIdAsync(id);

            if ( role is null ) return NotFound();

            var mappedRole = new RoleViewModel()
            {
                Id= role.Id,
                RoleName= role.Name

            };

            return View(viewname, mappedRole);


        }


        #endregion

        #region Edit Actions

        [HttpGet]
        public async Task<IActionResult> Edit( string? id )
        {
            return await Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( [FromRoute] string? id, RoleViewModel viewModel )
        {
            try
            {
                if ( id != viewModel.Id ) return BadRequest();

                if ( ModelState.IsValid )
                {
                    var roleFromDb = await _roleManager.FindByIdAsync(id);
                    if ( roleFromDb is null ) return NotFound();

                    roleFromDb.Name = viewModel.RoleName;

                    await _roleManager.UpdateAsync(roleFromDb);

                    return RedirectToAction(nameof(Index));
                }

            }
            catch ( Exception ex )
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(viewModel);
        }

        #endregion

        #region Delete Actions

        [HttpGet]
        public async Task<IActionResult> Delete( string? id )
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        public async Task<IActionResult> Delete( [FromRoute] string? id, RoleViewModel viewModel )
        {
            try
            {
                if ( id != viewModel.Id ) return BadRequest();

                if ( ModelState.IsValid )
                {
                    var roleFromDb = await _roleManager.FindByIdAsync(id);
                    if ( roleFromDb is null ) return NotFound();


                    await _roleManager.DeleteAsync(roleFromDb);

                    return RedirectToAction(nameof(Index));
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

