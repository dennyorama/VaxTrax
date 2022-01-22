using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace VaxTrax_2._0_
{
    class RefusalMethods
    {
        public static bool AddRefusal(Refusal objRefusal, User objUser)
        {
            bool status;
            int rowsAffected = 0;
            string connectionString = GetConnectionString();
            string sqlString = "insert into Refusal (patient_ID, Refusal_Reason, Date, User_ID) select @patient_ID, @Refusal_Reason, @Date, User_ID from [User] where UserName = @UserName";

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@patient_ID", objRefusal.patient_ID, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("@Refusal_Reason", objRefusal.Refusal_Reason, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Date", objRefusal.Date, DbType.Date, ParameterDirection.Input);
                    parameters.Add("@UserName", objUser.UserName, DbType.String, ParameterDirection.Input);

                    rowsAffected = db.Execute(sqlString, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            status = rowsAffected > 0 ? true : false;

            return status;
        }

        public static List<Refusal> GetTodaysRefusals()
        {
            List<Refusal> refusalList = new List<Refusal>();
            string connectionString = GetConnectionString();
            string sqlString = "select * from Refusal where Date = @Date";

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Date", System.DateTime.Today, DbType.Date, ParameterDirection.Input);
                    refusalList = db.Query<Refusal>(sqlString, parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return refusalList;
        }

        public static bool DailyRefusalExists(DateTime Day)
        {
            bool status;
            int dailyRefusalExists;

            string connectionString = GetConnectionString();

            string sqlString = "select count(*) from Refusal where Date = @Date";

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add("@Date", Day, DbType.Date, ParameterDirection.Input);

                    dailyRefusalExists = (Convert.ToInt32(db.ExecuteScalar(sqlString, parameters)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (dailyRefusalExists > 0)
            {
                status = true;
            }
            else
            {
                status = false;
            }

            return status;
        }

        private static string GetConnectionString()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ToString();

            return connectionString;
        }
    }
}
