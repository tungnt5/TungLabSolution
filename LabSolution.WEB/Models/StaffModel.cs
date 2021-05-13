using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LabSolution.WEB.Models
{
    public class StaffModel
    {
        [Display(Name = "ID")]
        public int PK_StaffID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Tên.")]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email.")]
        [RegularExpression(@"^\S+@\S+\.\S+$", ErrorMessage = "Email không hợp lệ")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Tel.")]
        [RegularExpression(@"\d{1,9}-\d{1,9}-\d{1,9}$", ErrorMessage = "Tel không hợp lệ")]
        [Display(Name = "Tel")]
        public string Tel { get; set; }

        public int TotalCount { get; set; }
    }
}