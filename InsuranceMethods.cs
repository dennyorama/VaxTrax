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
    class InsuranceMethods
    {
        public static void AddInsurance(Insurance objInsurance)
        {
            //bool status;
            //int rowsAffected = 0;
            string connectionString = GetConnectionString();

            string sqlString = "insert into Insurance values (@patient_ID, @Insurer, @Primary_Holder, @Group_ID)";

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@patient_ID", objInsurance.patient_ID, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("@Insurer", objInsurance.Insurer, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Primary_Holder", objInsurance.Primary_Holder, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Group_ID", objInsurance.Group_ID, DbType.String, ParameterDirection.Input);

                    //rowsAffected = db.Execute(sqlString, parameters);
                    db.Execute(sqlString, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //status = rowsAffected > 0 ? true : false;

            //return status;
        }

        public static bool InsuranceExists(Insurance objInsurance)
        {
            bool status;

            int insuranceExists;

            string connectionString = GetConnectionString();

            string sqlString = "select count(*) from Insurance where patient_ID = @patient_ID";

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add("@patient_ID", objInsurance.patient_ID, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@Insurer", objInsurance.Insurer, DbType.String, ParameterDirection.Input);
                    //parameters.Add("@Primary_Holder", objInsurance.Primary_Holder, DbType.String, ParameterDirection.Input);
                    //parameters.Add("@Group_ID", objInsurance.Group_ID, DbType.String, ParameterDirection.Input);

                    insuranceExists = (Convert.ToInt32(db.ExecuteScalar(sqlString, parameters)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (insuranceExists == 1)
            {
                status = true;
            }
            else
            {
                status = false;
            }

            return status;
        }

        public static Insurance GetInsurance(int patient_ID)
        {
            Insurance myInsurance;

            string connectionString = GetConnectionString();

            string sqlString = "select patient_ID, Insurer, Primary_Holder, Group_ID from Insurance where patient_ID = @patient_ID";

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@patient_ID", patient_ID, DbType.Int32, ParameterDirection.Input);
                    myInsurance = db.QuerySingleOrDefault<Insurance>(sqlString, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return myInsurance;
        }

        public static void UpdateInsurance(Insurance objInsurance)
        {
            string connectionString = GetConnectionString();
            string sqlString = "update Insurance set Insurer = @Insurer, Primary_Holder = @Primary_Holder, Group_ID = @Group_ID " +
                "where patient_ID = @patient_ID";

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Insurer", objInsurance.Insurer, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Primary_Holder", objInsurance.Primary_Holder, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Group_ID", objInsurance.Group_ID, DbType.String, ParameterDirection.Input);
                    parameters.Add("@patient_ID", objInsurance.patient_ID, DbType.Int32, ParameterDirection.Input);
                    db.Execute(sqlString, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetConnectionString()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ToString();

            return connectionString;
        }
    }
}
