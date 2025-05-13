using System.ComponentModel.DataAnnotations;

namespace Ajloun_Project.Models.ViewModels
{
    public class User
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [StringLength(70, MinimumLength = 6, ErrorMessage = "الاسم الكامل يجب أن يكون بين 6 و70 حرف")]
        public string FullName { get; set; } = null!;


        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون على الأقل 6 أحرف")]
        [RegularExpression(@"^(?!.*(?:012345|123456|234567|345678|456789|567890|098765|987654|876543|765432|654321|543210)).*$",
    ErrorMessage = "كلمة المرور لا يجب أن تحتوي على أرقام متتابعة")]
        public string Password { get; set; } = null!;


        [Phone(ErrorMessage = "رقم الهاتف غير صالح")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار الجنس")]
        [RegularExpression("^(ذكر|انثى)$", ErrorMessage = "الرجاء اختيار إما 'ذكر' أو 'انثى' فقط")]
        public string? Gender { get; set; }


        [DataType(DataType.Date)]
        public DateOnly? BirthDate { get; set; }

        [StringLength(200, ErrorMessage = "العنوان يجب ألا يتجاوز 200 حرف")]
        public string? Address { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "الرقم الوطني يجب أن يتكون من 10 أرقام")]
        public string? NationalId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
