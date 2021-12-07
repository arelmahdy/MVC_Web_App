using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Web_App.Data
{
    public class Users
    {
        DataAccessLayer Dal = new DataAccessLayer();
        DataTable dt = new DataTable();
        public bool state = false;

        public DataTable chekUserNameExist(string username)
        {
            state = false;
            dt = new DataTable();
            SqlParameter[] parameter = new SqlParameter[1];
            parameter[0] = new SqlParameter("@username", SqlDbType.NVarChar, 250);
            parameter[0].Value = username;
            dt = Dal.SelectData("chekUserNameExist", parameter);
            this.state = Dal.state;
            return dt;
        }
        public DataTable GetSingleUserRow(string id)
        {
            state = false;
            dt = new DataTable();
            SqlParameter[] parameter = new SqlParameter[1];
            parameter[0] = new SqlParameter("@id", SqlDbType.NVarChar, 450);
            parameter[0].Value = id;
            dt = Dal.SelectData("[GetSingleUserRow]", parameter);
            this.state = Dal.state;
            return dt;
        }
        public void UpdateEmailConfirm(string id, bool emailconfrim)
        {
            state = false;
            Dal.Open();

            SqlParameter[] parameter = new SqlParameter[2];

            parameter[0] = new SqlParameter("@id", SqlDbType.NVarChar, 450);
            parameter[0].Value = id;

            parameter[1] = new SqlParameter("@EmailConfirm", SqlDbType.Bit);
            parameter[1].Value = emailconfrim;

            Dal.ExecuteCommand("[UpdateEmailConfirm]", parameter);

            Dal.Close();
            this.state = Dal.state;
        }
        public void DeleteEmailConfirm(string id)
        {
            state = false;
            Dal.Open();

            SqlParameter[] parameter = new SqlParameter[1];

            parameter[0] = new SqlParameter("@id", SqlDbType.NVarChar, 450);
            parameter[0].Value = id;

            Dal.ExecuteCommand("[DeleteEmailConfirm]", parameter);

            Dal.Close();
            this.state = Dal.state;
        }
        public DataTable CheckEmailConfirmExist(string UID)
        {

            state = false;
            dt = new DataTable();
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@UserId", SqlDbType.NVarChar, 450);
            parameters[0].Value = UID;
            dt = Dal.SelectData("[CheckEmailConfirmExist]", parameters);
            this.state = true;
            return dt;
        }
        public DataTable GetUserRole()
        {
            state = false;
            dt = new DataTable();
            dt = Dal.SelectData("[GetUserRole]", null);
            this.state = Dal.state;
            return dt;

        }

        public DataTable CheckLogin(string username,string pass)
        {

            state = false;
            dt = new DataTable();
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@UserName", SqlDbType.NVarChar, 250);
            parameters[0].Value = username;
            parameters[1] = new SqlParameter("@Password", SqlDbType.NVarChar, 50);
            parameters[1].Value = pass;
            dt = Dal.SelectData("[userLogin]", parameters);
            this.state = true;
            return dt;
        }
    }
}
