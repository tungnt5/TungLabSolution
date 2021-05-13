using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LabSolution.WEB.Models
{
    public class UserModel
    {
        [Display(Name = "LoginID")]
        [Required(ErrorMessage = "Vui lòng nhập LoginID.")]
        public string Username { set; get; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Vui lòng nhập Password.")]
        public string Password { set; get; }
    }
}