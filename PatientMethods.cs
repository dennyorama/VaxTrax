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
    class PatientMethods
    {
        public static bool AddPatient(Patient objPatient)
        {
            bool Status;
            int rowsAffected = 0;
            string connectionString = GetConnectionString();
            
            string sqlString = "insert into Patient values (@patient_ID, @First_Name, @Middle_Name, @Last_Name, @Date_of_Birth, @Street1, @Street2, @City, @State, @County, @Zipcode, @Sex, @Race, @Ethnicity)";

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@patient_ID", objPatient.patient_ID, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("@First_Name", objPatient.First_Name, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Middle_Name", objPatient.Middle_Name, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Last_Name", objPatient.Last_Name, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Date_of_Birth", objPatient.Date_of_Birth, DbType.Date, ParameterDirection.Input);
                    parameters.Add("@Street1", objPatient.Street1, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Street2", objPatient.Street2, DbType.String, ParameterDirection.Input);
                    parameters.Add("@City", objPatient.City, DbType.String, ParameterDirection.Input);
                    parameters.Add("@State", objPatient.State, DbType.String, ParameterDirection.Input);
                    parameters.Add("@County", objPatient.County, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Zipcode", objPatient.Zipcode, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Sex", objPatient.Sex, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Race", objPatient.Race, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Ethnicity", objPatient.Ethnicity, DbType.String, ParameterDirection.Input);

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

        public static Patient GetPatient(string FName, string LName, string DOB)
        {
            Patient myPatient;

            string connectionString = GetConnectionString();

            string sqlString = "select patient_ID, First_Name, Middle_Name, Last_Name, Date_of_Birth, Street1, Street2, City, State, " +
                "County, Zipcode, Sex, Race, Ethnicity from Patient where First_Name = @First_Name and Last_Name = @Last_Name and Date_of_Birth = @DOB";

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@First_Name", FName, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Last_Name", LName, DbType.String, ParameterDirection.Input);
                    parameters.Add("@DOB", DOB, DbType.Date, ParameterDirection.Input);
                    myPatient = db.QuerySingleOrDefault<Patient>(sqlString, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return myPatient;
        }

        public static bool PatientExists(string FName, string LName, string DOB)
        {
            bool status;

            int patientExists;

            string connectionString = GetConnectionString();

            string sqlString = "select count(*) from Patient where First_Name = @First_Name and Last_Name = @Last_Name and Date_of_Birth = @DOB";

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add("@First_Name", FName, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Last_Name", LName, DbType.String, ParameterDirection.Input);
                    parameters.Add("@DOB", DOB, DbType.Date, ParameterDirection.Input);

                    patientExists = (Convert.ToInt32(db.ExecuteScalar(sqlString, parameters)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (patientExists == 1)
            {
                status = true;
            }
            else
            {
                status = false;
            }

            return status;
        }
        
        public static void UpdatePatientAddress(Patient objPatient)
        {
            string connectionString = GetConnectionString();
            string sqlString = "update Patient set Street1 = @Street1, Street2 = @Street2, City = @City, County = @County, State = @State, Zipcode = @Zipcode " +
                "where patient_ID = @patient_ID";

            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Street1", objPatient.Street1, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Street2", objPatient.Street2, DbType.String, ParameterDirection.Input);
                    parameters.Add("@City", objPatient.City, DbType.String, ParameterDirection.Input);
                    parameters.Add("@County", objPatient.County, DbType.String, ParameterDirection.Input);
                    parameters.Add("@State", objPatient.State, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Zipcode", objPatient.Zipcode, DbType.String, ParameterDirection.Input);
                    parameters.Add("@patient_ID", objPatient.patient_ID, DbType.Int32, ParameterDirection.Input);
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
