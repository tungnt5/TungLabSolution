using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace LabSolution.API
{
    /// <summary>
    ///   <br />
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// tungnt5  5/12/2021   created
    /// </Modified>
    public class TokenManager
    {
        private static string Secret = "012345dsadJKNAUIDFNmfnDASNDSAJDNAHNFUINQWINDOWIQDMKLNASINDFASNFUILEWejbnadAJKFNJFNAEUIbDJKNEN6789ABCDEFDSADN";

        /// <summary>Generates the token.</summary>
        /// <param name="userName">UserName.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/12/2021   created
        /// </Modified>
        public static string GenerateToken(string userName)
        {
            byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims: new[] { new Claim(type: ClaimTypes.Name, value: userName) }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(securityKey,
                    algorithm: SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

        /// <summary>Gets the principal.</summary>
        /// <param name="token">The token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/12/2021   created
        /// </Modified>
        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                byte[] key = Convert.FromBase64String(Secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                       parameters, out securityToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Validates the token.</summary>
        /// <param name="token">The token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tungnt5  5/12/2021   created
        /// </Modified>
        public static string ValidateToken(string token)
        {
            string username = null;
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
                return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim usernameClaim = identity.FindFirst(type:ClaimTypes.Name);
            username = usernameClaim.Value;
            return username;
        }
    }
}