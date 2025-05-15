using System.ComponentModel.DataAnnotations;

namespace Ajloun_Project.Models.ViewModels
{
    public class UserSignIn
    {

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
        public string Email { get; set; } = null!;
        public bool Admin { get; set; } = false;
    }
}
