using LabSolution.WEB.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
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
    public class StaffsController : Controller
    {
        /// <summary>Danh sách nhân viên.</summary>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="search">The search.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/12/2021   created
        /// </Modified>
        [HttpGet]
        [ValidateInput(false)]
        public async Task<ActionResult> Index(int? page, int? pageSize, string search)
        {
            List<StaffModel> staffs;

            if (Session["Token"] == null || Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            page = page ?? 1;
            pageSize = pageSize ?? 3;
            search = search ?? "";

            ViewBag.PageNumber = page;
            ViewBag.PageSize = pageSize;
            ViewBag.search = search;

            try
            {
                GlobalVariables.WebApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Token"].ToString() + ":" + Session["UserName"].ToString());
                HttpResponseMessage response = await GlobalVariables.WebApiClient.GetAsync($"Staffs/GetStaffs/{page}/{pageSize}/{search}");

                if(response.IsSuccessStatusCode)
                {
                    staffs = response.Content.ReadAsAsync<List<StaffModel>>().Result;

                    return View("Index", new StaffInfoModel() { staffProfile = staffs });
                }    
            }
            catch
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        [HttpGet]
        [ValidateInput(false)]
        public async Task<ActionResult> IndexInfo(int? page, int? pageSize, string search)
        {
            List<StaffModel> staffs = new List<StaffModel>(); ;

            page = page ?? 1;
            pageSize = pageSize ?? 3;
            search = search ?? "";
             
            ViewBag.PageNumber = page;
            ViewBag.PageSize = pageSize;
            ViewBag.search = search;
            
            try
            {
                GlobalVariables.WebApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Token"].ToString() + ":" + Session["UserName"].ToString());
                HttpResponseMessage response = await GlobalVariables.WebApiClient.GetAsync($"Staffs/GetStaffs/{page}/{pageSize}/{search}");

                if (response.IsSuccessStatusCode)
                {
                    staffs = response.Content.ReadAsAsync<List<StaffModel>>().Result;
                }

                return PartialView("IndexInfo", new StaticPagedList<StaffModel>(staffs,
                    Convert.ToInt32(page), Convert.ToInt32(pageSize), staffs.Count > 0 ? staffs.FirstOrDefault().TotalCount : 0));
            }
            catch
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>Thêm mới.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/11/2021   created
        /// </Modified>
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Insert()
        {
            if (Session["Token"] == null || Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View(new StaffModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Insert(StaffModel staff)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GlobalVariables.WebApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Token"].ToString() + ":" + Session["UserName"].ToString());
                    HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync($"Staffs/EmailExists/0?email={staff.Email ?? ""}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.Content.ReadAsStringAsync().Result == "true")
                        {
                            ModelState.AddModelError("", "Email đã tồn tại!");
                            return View(staff);
                        }

                        return RedirectToAction("ConfirmInsert", staff);
                    }
                    return View(staff);
                }

                return View(staff);
            }
            catch
            {
                return View(staff);
            }
        }

        /// <summary>Xác nhận thêm mới.</summary>
        /// <param name="staff">StaffModel.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/11/2021   created
        /// </Modified>
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult ConfirmInsert(StaffModel staff)
        {
            if (Session["Token"] == null || Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View(staff);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult confirmInsert(StaffModel staff)
        {
            try
            {
                //staff.Name = WebUtility.HtmlEncode(staff.Name);

                staff.Name = Regex.Replace(staff.Name, "<.*?>", String.Empty);

                GlobalVariables.WebApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Token"].ToString() + ":" + Session["UserName"].ToString());
                HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Staffs/InsertStaff", staff).Result;

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
                else
                {
                    ModelState.AddModelError("", response.Content.ReadAsStringAsync().Result);
                    return View(staff);
                }
            }
            catch
            {
                ModelState.AddModelError("", "Thêm mới không thành công!");

                return View(staff);
            }


        }

        /// <summary>Cập nhật.</summary>
        /// <param name="id">ID staff.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/11/2021   created
        /// </Modified>
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Edit(int id)
        {
            if (Session["Token"] == null || Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            try
            {
                GlobalVariables.WebApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Token"].ToString() + ":" + Session["UserName"].ToString());
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Staffs/GetStaffById/" + id.ToString()).Result;

                if (response.IsSuccessStatusCode)
                    return View(response.Content.ReadAsAsync<StaffModel>().Result);

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, StaffModel staff)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GlobalVariables.WebApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Token"].ToString() + ":" + Session["UserName"].ToString());
                    HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync($"Staffs/EmailExists/{id}?email={staff.Email ?? ""}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.Content.ReadAsStringAsync().Result == "true")
                        {
                            ModelState.AddModelError("", "Email đã tồn tại!");
                            return View(staff);
                        }

                        return RedirectToAction("ConfirmEdit", staff);
                    }
                    return View(staff);
                }

                return View(staff);
            }
            catch
            {
                return View(staff);
            }

        }

        /// <summary>Xác nhận cập nhật.</summary>
        /// <param name="staff">StaffModel.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/11/2021   created
        /// </Modified>
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult ConfirmEdit(StaffModel staff)
        {
            if (Session["Token"] == null || Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View(staff);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult confirmEdit(StaffModel staff)
        {
            try
            {
                //staff.Name = WebUtility.HtmlEncode(staff.Name);

                string name = Regex.Replace(staff.Name, "<.*?>", String.Empty);

                staff.Name = name;

                GlobalVariables.WebApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Token"].ToString() + ":" + Session["UserName"].ToString());
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("Staffs/UpdateStaff/" + staff.PK_StaffID, staff).Result;

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
                else
                {
                    ModelState.AddModelError("", response.Content.ReadAsStringAsync().Result);
                    return View(staff);
                }
            }
            catch
            {
                ModelState.AddModelError("", "Cập nhật không thành công!");
                return View(staff);
            }
        }

        /// <summary>Xác nhận xóa.</summary>
        /// <param name="id">ID staff.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/11/2021   created
        /// </Modified>
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Delete(int id)
        {
            if (Session["Token"] == null || Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            try
            {
                GlobalVariables.WebApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Token"].ToString() + ":" + Session["UserName"].ToString());
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Staffs/GetStaffById/" + id.ToString()).Result;

                if (response.IsSuccessStatusCode)
                {
                    return View(response.Content.ReadAsAsync<StaffModel>().Result);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Delete(int id, StaffModel staff)
        {
            try
            {
                GlobalVariables.WebApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Token"].ToString() + ":" + Session["UserName"].ToString());
                HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Staffs/DeleteStaff/" + id.ToString()).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", response.Content.ReadAsStringAsync().Result);
                    return View(staff);
                }
            }
            catch
            {
                ModelState.AddModelError("", "Xóa không thành công");
                return View(staff);
            }

        }

    }
}