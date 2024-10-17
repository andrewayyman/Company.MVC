using AutoMapper;
using Company.Route.DAL.Data.Migrations;
using Company.Route.DAL.Models;
using Company.Route.PL.Helpers;
using Company.Route.PL.ViewModels.Account;
using Company.Route.PL.ViewModels.Employess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Company.Route.PL.Controllers
{
    [Authorize]

    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserController( UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }


        #region Index Actions 
        public async Task<IActionResult> Index( string InputSearch )
        {
            var users = Enumerable.Empty<UserViewModel>();

            try
            {
                if ( string.IsNullOrEmpty(InputSearch) ) // get all
                {
                    users = await _userManager.Users.Select(U => new UserViewModel()
                    {
                        Id = U.Id,
                        FName = U.FName,
                        LName = U.LName,
                        Email = U.Email,
                        Roles = _userManager.GetRolesAsync(U).Result

                    }).ToListAsync();

                }
                else // get search value
                {
                    users = await _userManager.Users.Where(U => U.Email
                                                    .ToLower()
                                                    .Contains(InputSearch.ToLower()))
                                                    .Select(U => new UserViewModel()
                                                    {
                                                        Id = U.Id,
                                                        FName = U.FName,
                                                        LName = U.LName,
                                                        Email = U.Email,
                                                        Roles = _userManager.GetRolesAsync(U).Result
                                                    }).ToListAsync();
                                        
                }

            }
            catch ( Exception ex )
            {
                ModelState.AddModelError(string.Empty, ex.Message);

            }


            return View(users);

        }

        #endregion

        #region Details Action
        [HttpGet]

        public async Task<IActionResult> Details( string? id, string viewname = "Details" )
        {
            if ( id is null ) return BadRequest();

            var user = await _userManager.FindByIdAsync(id);

            if ( user is null ) return NotFound();

            var mappedUser = _mapper.Map<ApplicationUser, UserViewModel>(user);

            return View(viewname, mappedUser);


        }


        #endregion

        [Authorize(Roles = "Admin")]
        #region Edit Actions

        [HttpGet]
        public async Task<IActionResult> Edit( string? id )
        {
            return await Details(id, "Edit");
        }


        [HttpPost] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( [FromRoute] string? id, UserViewModel viewModel )
        {
            try
            {
                if ( id != viewModel.Id ) return BadRequest();

                if ( ModelState.IsValid )
                {
                    var userFromDb = await _userManager.FindByIdAsync(id);
                    if ( userFromDb is null ) return NotFound();

                    userFromDb.FName = viewModel.FName;
                    userFromDb.LName = viewModel.LName;
                    userFromDb.Email = viewModel.Email;
                    
                    await _userManager.UpdateAsync(userFromDb);

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

        [Authorize(Roles = "Admin")]
        #region Delete Actions
        [HttpGet]
        public async Task<IActionResult> Delete( string? id )
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        public async Task<IActionResult> Delete( [FromRoute] string? id, UserViewModel viewModel )
        {
            try
            {
                if ( id != viewModel.Id ) 
                    return BadRequest();

                if ( ModelState.IsValid )
                {
                    var userFromDb = await _userManager.FindByIdAsync(id);
                    if ( userFromDb is null ) return NotFound();

                    await _userManager.DeleteAsync(userFromDb);

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
