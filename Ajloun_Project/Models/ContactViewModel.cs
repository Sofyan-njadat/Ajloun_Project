using System.ComponentModel.DataAnnotations;

namespace Ajloun_Project.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "الرجاء إدخال الاسم")]
        [Display(Name = "الاسم")]
        public string Name { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال البريد الإلكتروني")]
        [EmailAddress(ErrorMessage = "الرجاء إدخال بريد إلكتروني صحيح")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال رقم الهاتف")]
        [Phone(ErrorMessage = "الرجاء إدخال رقم هاتف صحيح")]
        [Display(Name = "رقم الهاتف")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال الموضوع")]
        [Display(Name = "الموضوع")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال الرسالة")]
        [Display(Name = "الرسالة")]
        [StringLength(1000, ErrorMessage = "الرسالة يجب أن لا تتجاوز 1000 حرف")]
        public string Message { get; set; }
    }
} 