using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace MechanicTracker
{
    /// <summary>
    /// Summary description for WebServiceSearcRegistration
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class WebServiceSearcRegistration : System.Web.Services.WebService
    {

        [WebMethod]
        public List<string> GetData(string carReg)
        {
            List<string> result = new List<string>();
            using (SqlConnection con = new SqlConnection("MTEntities"))
            {
                using (SqlCommand cmd = new SqlCommand("select Registration from car where carReg like '%'+@SearchText+'%'", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@SearchText", carReg);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        result.Add(dr["carReg"].ToString());
                    }
                    return result;
                }
            }
        }
    }
}
