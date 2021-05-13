using LabSolution.WEB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace LabSolution.WEB
{
    /// <summary>
    ///   <br />
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// tungnt5  5/12/2021   created
    /// </Modified>
    public static class GlobalVariables
    {
        public static HttpClient WebApiClient = new HttpClient();

        static GlobalVariables()
        {
            WebApiClient.BaseAddress = new Uri("https://localhost:44395/");
            //WebApiClient.BaseAddress = new Uri("http://localhost:8069/");
            WebApiClient.DefaultRequestHeaders.Clear();
            WebApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}