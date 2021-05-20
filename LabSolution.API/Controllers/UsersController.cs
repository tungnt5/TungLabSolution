using LabSolution.Data.Models;
using LabSolution.WEB.Common;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Description;

namespace LabSolution.API.Controllers
{
    /// <summary>
    ///   <br />
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// tungnt5  5/11/2021   created
    /// </Modified>
    public class UsersController : ApiController
    {
        private DBModel db = new DBModel();

        /// <summary>Gets the users.</summary>
        /// <param name="username">The username.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/12/2021   created
        /// </Modified>
        [ResponseType(typeof(T_Users))]
        public T_Users GetUsers(string username)
        {
            return db.T_Users.SingleOrDefault(p => p.UserName == username);
        }

        /// <summary>Đăng nhập.</summary>
        /// <param name="user">The user.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/11/2021   created
        /// </Modified>
        [HttpPost]
        public HttpResponseMessage ValidLogin(T_Users user)
        {
            var login = db.T_Users.SingleOrDefault(p => p.UserName == user.UserName);

            string passWordMD5 = Encryptor.MD5Hash(user.PassWord);

            if (login == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadGateway, message: "Không tìm thấy user thích hợp");
            }
            else
            {
                if(login.PassWord == passWordMD5)
                {
                    //var obj = new
                    //{
                    //    id = login.PK_UserID,
                    //    username = login.UserName,
                    //    token = TokenManager.GenerateToken(user.UserName),
                    //};

                    return Request.CreateResponse(HttpStatusCode.OK, value: TokenManager.GenerateToken(user.UserName));
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadGateway, message: "LoginID hoặc Password bị sai");
                }    
            }
        }
    }
}
