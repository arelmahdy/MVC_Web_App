using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
namespace MVC_Web_App.Data
{
    public static class AppAuthentication
    {
        public static string GetRoleId(string RoleName)
        {
            string str = string.Empty;
            DataTable dt = new DataTable();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@RoleName", SqlDbType.NVarChar, 150);
            param[0].Value = RoleName;
            DataAccessLayer Dal = new DataAccessLayer();
            dt = Dal.SelectData("GetMemberId",param);
            if (dt.Rows.Count > 0)
            {
                return str = dt.Rows[0][0].ToString();
            }
            return str;
        }
        public static string GetRoleName(string userName)
        {
            string str = string.Empty;
            DataTable dt = new DataTable();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@UserName", SqlDbType.NVarChar, 250);
            param[0].Value = userName;

            DataAccessLayer DAL = new DataAccessLayer();
            dt = DAL.SelectData("GetUserRoles_ByUserName", param);
            if (dt.Rows.Count > 0)
            {
                str = dt.Rows[0][3].ToString();
            }

            return str;
        }
        public static string GetIdByUserName(string username)
        {
            string name = string.Empty;
            DataTable dt = new DataTable();
            Users users = new Users();
            dt = users.chekUserNameExist(username);
            if(dt.Rows.Count > 0)
            {
                name = dt.Rows[0][0].ToString();
            }
            return name;
        }
    }
}
