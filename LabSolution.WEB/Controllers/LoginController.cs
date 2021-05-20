using LabSolution.WEB.Common;
using LabSolution.WEB.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LabSolution.WEB.Controllers
{
    /// <summary>
    ///   <br />
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// tungnt5  5/11/2021   created
    /// </Modified>
    public class LoginController : Controller
    {
        // GET: Login
        /// <summary>Đăng nhập.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/11/2021   created
        /// </Modified>
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(UserModel user)
        {
            var tokenBased = string.Empty;

            Session.Remove("Token");
            Session.Remove("UserName");

            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await GlobalVariables.WebApiClient.PostAsJsonAsync("Users/ValidLogin", user);
                if(response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    tokenBased = JsonConvert.DeserializeObject<string>(result);

                    Session["Token"] = tokenBased;
                    Session["UserName"] = user.Username;

                    return RedirectToAction("Index", "Staffs");
                }
                else
                {
                    ModelState.AddModelError("", response.Content.ReadAsStringAsync().Result);
                    return View();
                }
                
            }
            else
                return View();
        }

        /// <summary>Đăng xuất.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/11/2021   created
        /// </Modified>
        public ActionResult LogOut()
        {
            Session.Remove("Token");
            Session.Remove("UserName");
            return RedirectToAction("Index", "Login");
        }
    }
}