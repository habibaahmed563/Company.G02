using System.ComponentModel.DataAnnotations;

namespace Company.G02.PL.Dtos
{
    public class SignInDto
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "PassWord is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
