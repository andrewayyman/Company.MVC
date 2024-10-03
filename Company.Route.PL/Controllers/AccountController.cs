using Company.Route.DAL.Models;
using Company.Route.PL.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.Route.PL.Controllers
{
    // Password  : P@$$w0rd

    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }



        #region SignUp

        [HttpGet]
        // GET: /Account/SignUp
        public IActionResult SignUp()
        {
            return View();
        }


        // POST: /Account/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp( SignUpViewModel viewmodel )
        {
            if ( ModelState.IsValid ) // server side validation
            {
                // 1. Map from ViewModel to Model [Manual Mapping easier in this case]
                var user = new ApplicationUser()
                {
                    // take username from email
                    UserName = viewmodel.Email.Split("@")[0], // take string before @ 
                    Email = viewmodel.Email,
                    IsAgree = viewmodel.IsAgree,
                    FName = viewmodel.FName,
                    LName = viewmodel.LName,
                };


                // 2. Add the user using userManager 
                var Result = await _userManager.CreateAsync(user, viewmodel.Password);

                if ( Result.Succeeded )
                {
                    return RedirectToAction(nameof(SignIn));
                }
                foreach(var Error in Result.Errors)
                {
                    ModelState.AddModelError(string.Empty, Error.Description);
                }



            }
            return View(viewmodel);
        }

        #endregion




    }
}
