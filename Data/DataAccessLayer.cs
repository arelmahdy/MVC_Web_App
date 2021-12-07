using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Web_App.Data
{
    public class DataAccessLayer
    {
        SqlConnection con;
        public string a = string.Empty;
        public bool state = false;
        public DataAccessLayer()
        {
            con = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=CoreDb;Trusted_Connection=True;MultipleActiveResultSets=true;");

        }
        public void Open()
        {
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
        }
        public void Close()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        public DataTable SelectData(string storproc, SqlParameter[] param)
        {
            DataTable dt = new DataTable();
            state = false;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storproc;
            cmd.Connection = con;
            if (param != null)
            {
                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                da.Fill(dt);
                state = true;
            }
            catch 
            {

                state = false;
            }
            return dt;
        }
        public void ExecuteCommand(string storedproc, SqlParameter[] param)
        {

            try
            {
                state = false;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storedproc;
                cmd.Connection = con;
                if (param != null)
                {
                    for (int i = 0; i < param.Length; i++)
                    {
                        cmd.Parameters.Add(param[i]);
                    }
                }
                cmd.ExecuteNonQuery();
                state = true;
            }
            catch
            {
                state = false;
                
            }
        }
        public string GetString(string storedproc,SqlParameter[] param)
        {
            a = string.Empty;
            state = false;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storedproc;
            cmd.Connection = con;
            if (param != null)
            {
                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }
            }
            cmd.ExecuteNonQuery();
            state = true;
            try
            {
                Open();
                a = cmd.ExecuteScalar().ToString();
                Close();
                state = true;
            }
            catch (Exception)
            {

                state = false;
            }
            return a;
        }

    }
}
