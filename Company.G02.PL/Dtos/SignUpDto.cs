
using System.ComponentModel.DataAnnotations;
namespace Company.G02.PL.Dtos
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "UserName is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "FirstName is Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is Required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "PassWord is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "ConfirmPassword is Required")]
        [Compare(nameof(Password),ErrorMessage = "Confirmed password does not match the password")]
        public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }

    }
}
