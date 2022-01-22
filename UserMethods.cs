using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace VaxTrax_2._0_
{
    class LoginMethod
    {
        public static bool IsValid(string UserName, string Password)
        {
            bool Status;

            int userExists;

            string connectionString = GetConnectionString();

            string sqlString = "select count(*) from [User] where UserName = @UserName and Password = @Password";

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add("@UserName", UserName, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Password", Password, DbType.String, ParameterDirection.Input);

                    userExists = (Convert.ToInt32(db.ExecuteScalar(sqlString, parameters)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if(userExists == 1)
            {
                Status = true;
            }
            else
            {
                Status = false;
            }

            return Status;
        }

        public static int GetUserID(string username, string password)
        {
            int userID;

            string connectionString = GetConnectionString();

            string sqlString = "select User_ID from [User] where UserName = @UserName and Password = @Password";

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@UserName", username, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Password", password, DbType.String, ParameterDirection.Input);
                    userID = db.Execute(sqlString, parameters);
                    //myPatient = db.QuerySingleOrDefault<Patient>(sqlString, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return userID;
        }

        private static string GetConnectionString()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ToString();

            return connectionString;
        }
    }
}
