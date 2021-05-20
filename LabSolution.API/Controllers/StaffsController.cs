using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Description;
using LabSolution.Data.Models;

namespace LabSolution.API.Controllers
{
    /// <summary>
    ///   <br />
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// tungnt5  5/11/2021   created
    /// </Modified>
    public class StaffsController : ApiController
    {
        private DBModel db = new DBModel();

        /// <summary>Lấy tất cả danh sách staff.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/11/2021   created
        /// </Modified>
        [HttpGet]
        [CustomAuthenticationFilter]
        [Route("Staffs/GetAllStaffs")]
        public IQueryable<T_Staffs> GetAllStaffs()
        {
            return db.T_Staffs.Where(p => p.Status == true);
        }

        /// <summary>Lấy danh sách staff theo page và search.</summary>
        /// <param name="page">Page.</param>
        /// <param name="search">Tìm kiếm.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/11/2021   created
        /// </Modified>
        [HttpGet]
        [CustomAuthenticationFilter]
        [Route("Staffs/GetStaffs/{page}/{pageSize}/{search?}")]
        public IEnumerable<spGetStaffs_Result> GetStaffs(int page, int pageSize, string search = "")
        {
            return db.spGetStaffs(page, pageSize, search);
        }

        /// <summary>Lấy staff theo id.</summary>
        /// <param name="id">ID staff.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/11/2021   created
        /// </Modified>
        [HttpGet]
        [CustomAuthenticationFilter]
        [Route("Staffs/GetStaffById/{id}")]
        public IHttpActionResult GetStaffById(int id)
        {
            T_Staffs t_Staffs = db.T_Staffs.Find(id);
            if (t_Staffs == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            t_Staffs.Name = WebUtility.HtmlDecode(t_Staffs.Name);

            return Ok(t_Staffs);
        }

        /// <summary>Cập nhật staff.</summary>
        /// <param name="id">ID staff.</param>
        /// <param name="t_Staffs">T_Staffs.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/11/2021   created
        /// </Modified>
        [HttpPut]
        [CustomAuthenticationFilter]
        [Route("Staffs/UpdateStaff/{id}")]
        public IHttpActionResult UpdateStaff(int id, T_Staffs t_Staffs)
        {
            if (id != t_Staffs.PK_StaffID)
            {
                return BadRequest();
            }

            var staff = db.T_Staffs.FirstOrDefault(x => x.PK_StaffID == id);

            if (staff == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }

            staff.Name = Regex.Replace(t_Staffs.Name.Trim(), "<.*?>", String.Empty);
            staff.Email = t_Staffs.Email.Trim();
            staff.Tel = t_Staffs.Tel.Trim();
            staff.UpdateBy = 1;
            staff.UpdateDate = DateTime.Now;

            //db.T_Staffs.Attach(t_Staffs);
            //t_Staffs.UpdateBy = 1;
            //t_Staffs.UpdateDate = DateTime.Now;

            //db.Entry(t_Staffs).Property(x => x.Name).IsModified = true;
            //db.Entry(t_Staffs).Property(x => x.Email).IsModified = true;
            //db.Entry(t_Staffs).Property(x => x.Tel).IsModified = true;
            //db.Entry(t_Staffs).Property(x => x.UpdateBy).IsModified = true;
            //db.Entry(t_Staffs).Property(x => x.UpdateDate).IsModified = true;

            try
            {
                db.SaveChanges();
            }
            catch
            {
                return BadRequest("Cập nhật không thành công!");
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>Thêm mới staff.</summary>
        /// <param name="t_Staffs">T_Staffs.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/11/2021   created
        /// </Modified>
        [HttpPost]
        [CustomAuthenticationFilter]
        [Route("Staffs/InsertStaff")]
        public IHttpActionResult InsertStaff(T_Staffs t_Staffs)
        {
            t_Staffs.Name = Regex.Replace(t_Staffs.Name.Trim(), "<.*?>", String.Empty);
            t_Staffs.Email = t_Staffs.Email.Trim();
            t_Staffs.Tel = t_Staffs.Tel.Trim();

            t_Staffs.Status = true;
            t_Staffs.CreateBy = 1;
            t_Staffs.CreateDate = DateTime.Now;

            db.T_Staffs.Add(t_Staffs);

            try
            {
                db.SaveChanges();
            }
            catch
            {
                return BadRequest("Thêm mới không thành công!");
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>Xóa staff.</summary>
        /// <param name="id">ID staff.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/11/2021   created
        /// </Modified>
        [HttpDelete]
        [CustomAuthenticationFilter]
        [Route("Staffs/DeleteStaff/{id}")]
        public IHttpActionResult DeleteStaff(int id)
        {
            var staff = db.T_Staffs.FirstOrDefault(x => x.PK_StaffID == id);
            if (staff == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }

            staff.UpdateBy = 1;
            staff.UpdateDate = DateTime.Now;
            staff.Status = false;

            //db.T_Staffs.Attach(t_Staffs);
            //t_Staffs.Status = false;
            //t_Staffs.UpdateBy = 1;
            //t_Staffs.UpdateDate = DateTime.Now;

            //db.Entry(t_Staffs).Property(x => x.Status).IsModified = true;
            //db.Entry(t_Staffs).Property(x => x.UpdateBy).IsModified = true;
            //db.Entry(t_Staffs).Property(x => x.UpdateDate).IsModified = true;

            try
            {
                db.SaveChanges();
            }
            catch
            {
                return BadRequest("Xóa không thành công!");
            }

            //db.T_Staffs.Remove(t_Staffs);
            //db.SaveChanges();
            //return Ok(staff);

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>Check trùng email.</summary>
        /// <param name="id">ID staff.</param>
        /// <param name="email">Email.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/11/2021   created
        /// </Modified>
        [HttpGet]
        [CustomAuthenticationFilter]
        [Route("Staffs/EmailExists/{id}/{email?}")]
        public bool EmailExists(int id, string email = "")
        {
            if(id == 0)
            {
                return db.T_Staffs.Count(x => x.Email == email.Trim() && x.Status == true) > 0;
            }
            else
            {
                return db.T_Staffs.Count(x => x.Email == email.Trim() && x.PK_StaffID != id && x.Status == true) > 0;
            }    
        }
    }
}