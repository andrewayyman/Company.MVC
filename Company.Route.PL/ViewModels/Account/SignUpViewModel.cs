using System.ComponentModel.DataAnnotations;

namespace Company.Route.PL.ViewModels.Account
{
	public class SignUpViewModel
	{
		[Required(ErrorMessage ="Fname Is Required")]
        public string FName { get; set; }

		[Required(ErrorMessage ="Lname Is Required")]
        public string LName { get; set; }


        [Required(ErrorMessage = "Email Name is required")]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "Required to Agree")]
		public bool IsAgree { get; set; }	


    }
}
