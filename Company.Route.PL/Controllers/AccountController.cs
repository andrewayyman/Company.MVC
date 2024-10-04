using Company.Route.DAL.Models;
using Company.Route.PL.Helpers;
using Company.Route.PL.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace Company.Route.PL.Controllers
{
    // Password01 : P@$$w0rd
    // Password02 : P@$$w0rD

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
        public async Task<IActionResult> SignUp( SignUpViewModel viewModel )
        {
            if ( ModelState.IsValid ) // server side validation
            {
                // 1. Map from ViewModel to Model [Manual Mapping easier in this case]
                var user = new ApplicationUser()
                {
                    // take username from email
                    UserName = viewModel.Email.Split("@")[0], // take string before @ 
                    Email = viewModel.Email,
                    IsAgree = viewModel.IsAgree,
                    FName = viewModel.FName,
                    LName = viewModel.LName,
                };


                // 2. Add the user using userManager 
                var Result = await _userManager.CreateAsync(user, viewModel.Password);

                if ( Result.Succeeded )
                {
                    return RedirectToAction(nameof(SignIn));
                }
                foreach ( var Error in Result.Errors )
                {
                    ModelState.AddModelError(string.Empty, Error.Description);
                }



            }
            return View(viewModel);
        }

        #endregion

        #region SignIn

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn( SignInViewModel viewModel )
        {
            if ( ModelState.IsValid )
            {
                var user = await _userManager.FindByEmailAsync(viewModel.Email);
                if ( user is not null )
                {
                    bool IsPasswordMatches = await _userManager.CheckPasswordAsync(user, viewModel.Password);
                    if ( IsPasswordMatches )
                    {
                        // ispersistent is like remember me it save the token of the user in the cookie so he can send another request with the same login request
                        var IsSignedIn = await _signInManager.PasswordSignInAsync(user, viewModel.Password, true, false);
                        if ( IsSignedIn.Succeeded )
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }

                }
                ModelState.AddModelError(string.Empty, "Invalid Login");
            }
            return View(viewModel);
        }

        #endregion

        #region SignOut

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        #endregion

        #region ForgetPassword & Send Email 
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword( ForgetPasswordViewModel viewModel )
        {
            if ( ModelState.IsValid )
            {
                var user = await _userManager.FindByEmailAsync(viewModel.Email);
                if ( user is not null )
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var ResetPasswordUrl = Url.Action(
                       "ResetPassword",
                    "Account",
                       new { email = viewModel.Email, token = token },
                     Request.Scheme,   // Scheme (http or https)
                        Request.Host.ToString() // Host (localhost:port or domain)
                        );
                    var email = new Email()
                    {
                        Subject = "Reset Your Password",
                        Reciepints = viewModel.Email,
                        Body = $"{ResetPasswordUrl}"
                    };
                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourInbox));
                }
                ModelState.AddModelError(string.Empty, "Invalid Email");

            }


            return View(viewModel);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }


        public IActionResult ResetPassword( string email, string token )
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword( ResetPasswordViewModel viewModel )
        {
            if ( ModelState.IsValid )
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;
                var user = await _userManager.FindByEmailAsync(email);
                var result = await _userManager.ResetPasswordAsync(user, token, viewModel.Password);

                if ( result.Succeeded )
                {
                    return RedirectToAction(nameof(SignIn));
                }

                foreach ( var Error in result.Errors )
                {
                    ModelState.AddModelError(string.Empty, Error.Description);
                }


            }
            return View(viewModel);
        } 
        #endregion



    }
}
