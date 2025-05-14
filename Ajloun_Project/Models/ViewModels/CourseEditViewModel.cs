using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Ajloun_Project.Models.ViewModels
{
    public class CourseEditViewModel
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "اسم الدورة مطلوب")]
        public string Name { get; set; }

        [Required(ErrorMessage = "وصف الدورة مطلوب")]
        public string Description { get; set; }

        [Required(ErrorMessage = "الفئة العمرية مطلوبة")]
        public string AgeRange { get; set; }

        // الصورة الحالية
        public string CurrentImagePath { get; set; }

        // ملف الصورة الجديدة (اختياري)
        public IFormFile CourseImage { get; set; }
    }
} 