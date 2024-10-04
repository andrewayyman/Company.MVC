using System.ComponentModel.DataAnnotations;

namespace Company.Route.PL.ViewModels.Account
{
	public class ForgetPasswordViewModel
	{
		[Required(ErrorMessage ="Email is Requierd")]
		[EmailAddress (ErrorMessage ="Invalid Email Address")]
        public string Email { get; set; }
    }
}
