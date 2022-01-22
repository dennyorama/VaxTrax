using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace VaxTrax_2._0_
{
    class VaccineMethods
    {
        public static bool AddVaccine(Vaccine objVaccine, User objUser)
        {
            bool Status;
            int rowsAffected = 0;
            string connectionString = GetConnectionString();

            string sqlString = "insert into Vaccine (patient_ID, NumDose, Vaccinator_Name, Administration_site, Product, Manufacture, Typecvx, LotNum, ExpirationDate, " +
                "DateAdministered, NumWasted, MissedAppointment, Comorbidity, RevievedEUA, User_ID) select @patient_ID, @NumDose, @Vaccinator_Name, @Administration_site, " +
                "@Product, @Manufacture, @Typecvx, @LotNum, @ExpirationDate, @DateAdministered, @NumWasted," +
                " @MissedAppointment, @Comorbidity, @RevievedEUA, User_ID from [User] where UserName = @UserName";

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@patient_ID", objVaccine.patient_ID, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("@NumDose", objVaccine.NumDose, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("@Vaccinator_Name", objVaccine.Vaccinator_Name, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Administration_site", objVaccine.Administration_site, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Product", objVaccine.Product, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Manufacture", objVaccine.Manufacture, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Typecvx", objVaccine.Typecvx, DbType.String, ParameterDirection.Input);
                    parameters.Add("@LotNum", objVaccine.LotNum, DbType.String, ParameterDirection.Input);
                    parameters.Add("@ExpirationDate", objVaccine.ExpirationDate, DbType.Date, ParameterDirection.Input);
                    parameters.Add("@DateAdministered", objVaccine.DateAdministered, DbType.Date, ParameterDirection.Input);
                    parameters.Add("@NumWasted", objVaccine.NumWasted, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("@MissedAppointment", objVaccine.MissedAppointment, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Comorbidity", objVaccine.Comorbidity, DbType.String, ParameterDirection.Input);
                    parameters.Add("@RevievedEUA", objVaccine.RevievedEUA, DbType.String, ParameterDirection.Input);
                    parameters.Add("@UserName", objUser.UserName, DbType.String, ParameterDirection.Input);

                    rowsAffected = db.Execute(sqlString, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Status = rowsAffected > 0 ? true : false;

            return Status;
        }

        public static bool VaccineExists(int Patient_ID, int DoseNumber)
        {
            bool status;
            int vaccineExists;
            string connectionString = GetConnectionString();
            string sqlString = "select count(*) from Vaccine where patient_ID = @patient_ID and NumDose = @NumDose";

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@patient_ID", Patient_ID, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("@NumDose", DoseNumber, DbType.Int32, ParameterDirection.Input);

                    vaccineExists = (Convert.ToInt32(db.ExecuteScalar(sqlString, parameters)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (vaccineExists == 1)
            {
                status = true;
            }
            else
            {
                status = false;
            }

            return status;
        }

        public static List<DataGridVaccine> GetVaccines(int patient_id)
        {
            List<DataGridVaccine> vaccineList = new List<DataGridVaccine>();
            string connectionString = GetConnectionString();
            string sqlString = "select NumDose, Product, DateAdministered " +
                "from Vaccine where patient_ID = @patient_ID";

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@patient_ID", patient_id, DbType.Int32, ParameterDirection.Input);
                    vaccineList = db.Query<DataGridVaccine>(sqlString, parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return vaccineList;
        }

        public static List<Vaccine> GetTodaysVaccines()
        {
            List<Vaccine> vaccineList = new List<Vaccine>();
            string connectionString = GetConnectionString();
            string sqlString = "select * from Vaccine where DateAdministered = @DateAdministered";

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@DateAdministered", System.DateTime.Today, DbType.Date, ParameterDirection.Input);
                    vaccineList = db.Query<Vaccine>(sqlString, parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return vaccineList;
        }

        public static bool DailyVaccineExists(DateTime Day)
        {
            bool status;
            int dailyVaxExists;

            string connectionString = GetConnectionString();

            string sqlString = "select count(*) from Vaccine where DateAdministered = @DateAdministered";

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add("@DateAdministered", Day, DbType.Date, ParameterDirection.Input);

                    dailyVaxExists = (Convert.ToInt32(db.ExecuteScalar(sqlString, parameters)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (dailyVaxExists > 0)
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
