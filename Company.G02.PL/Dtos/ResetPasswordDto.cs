using System.ComponentModel.DataAnnotations;

namespace Company.G02.PL.Dtos
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "PassWord is Required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "ConfirmPassword is Required")]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirmed password does not match the password")]
        public string ConfirmPassword { get; set; }
    }
}
