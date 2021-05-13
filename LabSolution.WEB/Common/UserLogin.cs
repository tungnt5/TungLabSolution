using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabSolution.WEB.Common
{
    public class UserLogin
    {
        public int PK_UserID { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public bool Status { get; set; }
    }
}