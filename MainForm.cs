using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;
using Dapper;

namespace VaxTrax_2._0_
{
    // Author: Dennis Steven Dyer II
    //   Date: 10/13/2021
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        //Insurance newInsurance = new Insurance();

        private void MainForm_Load(object sender, EventArgs e)
        {
            // This is a form load event showing which user is logged into the system.
            // It also sets reasonable default values for DateTimePicker controls. 

            //userIDLabel.Text = GlobalUser.UserID + " Logged In.";
            userIDLabel.Text = "User Logged In: " + LoginForm.UserName; 

            dateAdministeredTextBox.Value = DateTime.Today;

            expirationDateTextBox.Value = dateAdministeredTextBox.Value.AddMonths(1);
            expirationDateTextBox.MinDate = DateTime.Today;

            // typeTextBox.BackColor = Color.White;
            

        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit the application?", "Exit Application", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                this.Close();
            }
            //this.Close();
        }

        private void vaersButton_Click(object sender, EventArgs e)
        {
            // This is a link to the VAERS website for reporting adverse reactions.
            System.Diagnostics.Process.Start("https://vaers.hhs.gov/esub/index.jsp");
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoginForm newLoginForm = new LoginForm();

            newLoginForm.Show();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            // This block of code occurs when the user clicks the add button
            // It instantiates new objects for Vaccine and Patient and assigns values
            // to their respective properties from the textboxes. After the properties 
            // have been assigned values, the object information is passed into the database
            Vaccine newVaccine = new Vaccine();

            Patient newPatient = new Patient();

            Insurance newInsurance = new Insurance();

            User newUser = new User();
            newUser.UserName = LoginForm.UserName;
            newUser.Password = LoginForm.Password;
            newUser.User_ID = LoginMethod.GetUserID(LoginForm.UserName, LoginForm.Password);

            List<string> PatientRace = new List<string>();

            List<string> PatientEthnicity = new List<string>();

            newPatient.First_Name = fNameTextBox.Text.Trim();
            newPatient.Middle_Name = mNameTextBox.Text.Trim();
            newPatient.Last_Name = lNameTextBox.Text.Trim();
            newPatient.Date_of_Birth = DOBTextBox.Text.Trim();
            newPatient.Street1 = street1TextBox.Text.Trim();
            newPatient.Street2 = street2TextBox.Text.Trim();
            newPatient.City = cityTextBox.Text.Trim();
            newPatient.County = countyTextBox.Text.Trim();
            newPatient.State = stateTextBox.Text.Trim();
            newPatient.Zipcode = zipcodeTextBox.Text.Trim();

            if (alaskanCheck.Checked)
            {
                PatientRace.Add("American Indian/Alaskan Native");
            }
            if (asianCheck.Checked)
            {
                PatientRace.Add("Asian");
            }
            if (hawaiianCheck.Checked)
            {
                PatientRace.Add("Native Hawaiian or Pacific Islander");
            }
            if (blackCheck.Checked)
            {
                PatientRace.Add("Black/African American");
            }
            if (whiteCheck.Checked)
            {
                PatientRace.Add("White");
            }
            if (otherCheck.Checked)
            {
                PatientRace.Add("Other Race");
            }
            if (unknownRaceCheck.Checked)
            {
                PatientRace.Add("Unknown");
            }
            if (unreportableRaceCheck.Checked)
            {
                PatientRace.Add("Unable to report");
            }

            for(int i = 0; i < PatientRace.Count; i++)
            {
                newPatient.Race += PatientRace[i] + ", ";
            }

            if (hispanicCheck.Checked)
            {
                PatientEthnicity.Add("Hispanic or Latino");
            }
            if (notHispanicCheck.Checked)
            {
                PatientEthnicity.Add("Not Hispanic or Latino");
            }
            if (unknownCheck.Checked)
            {
                PatientEthnicity.Add("Unknown");
            }
            if (unreportableCheck.Checked)
            {
                PatientEthnicity.Add("Unable to report");
            }

            for(int j = 0; j < PatientEthnicity.Count; j++)
            {
                newPatient.Ethnicity += PatientEthnicity[j] + ", ";
            }

            if (maleRadio.Checked)
            {
                newPatient.Sex = "M";
            }
            else if (femaleRadio.Checked)
            {
                newPatient.Sex = "F";
            }
            else if (unknownRadio.Checked)
            {
                newPatient.Sex = "U";
            }
            /*
            if (insurerTextBox.Text.Trim() != "")
            { newInsurance.Insurer = insurerTextBox.Text.Trim(); }
            if (primaryHolderTextBox.Text.Trim() != "")
            { newInsurance.Primary_Holder = primaryHolderTextBox.Text.Trim(); }
            if (insuranceIDTextBox.Text.Trim() != "")
            { newInsurance.Group_ID = insuranceIDTextBox.Text.Trim(); }
            */

            newInsurance.Insurer = insurerTextBox.Text.Trim();
            newInsurance.Primary_Holder = primaryHolderTextBox.Text.Trim();
            newInsurance.Group_ID = insuranceIDTextBox.Text.Trim();

            newVaccine.Typecvx = typeTextBox.Text.Trim();
            newVaccine.Product = productTextBox.Text.Trim();
            newVaccine.LotNum = lotNumberTextBox.Text.Trim();
            newVaccine.DateAdministered = dateAdministeredTextBox.Text.Trim();
            newVaccine.ExpirationDate = expirationDateTextBox.Text.Trim();
            newVaccine.Manufacture = manufacturerTextBox.Text.Trim();

            if(wastedAmountTextBox.SelectedIndex != -1)
            {
                newVaccine.NumWasted = Convert.ToInt32(wastedAmountTextBox.Text.Trim());
            }
            else
            {
                //MessageBox.Show("An integer value must be entered for # Wasted.");
            }

            if(doseNumberTextBox.SelectedIndex != -1)
            {
                newVaccine.NumDose = Convert.ToInt32(doseNumberTextBox.Text.Trim());
            }
            else
            {
                //MessageBox.Show("An integer value must entered for dose number.");
            }
           
            //newVaccine.RefusalReason = refusalReasonTextBox.Text.Trim();
            newVaccine.Vaccinator_Name = vaccinatorTextBox.Text.Trim();

            if (leftarmRadio.Checked)
            {
                newVaccine.Administration_site = "Left Arm";
            }
            else if (rightarmRadio.Checked)
            {
                newVaccine.Administration_site = "Right Arm";
            }
            else if (extremityleftRadio.Checked)
            {
                newVaccine.Administration_site = "Lower Extremity Left";
            }
            else if (extremityRightRadio.Checked)
            {
                newVaccine.Administration_site = "Lower Extremity Right";
            }

            if (maYesRadio.Checked)
            {
                newVaccine.MissedAppointment = "Y";
            }
            else if (maNoRadio.Checked)
            {
                newVaccine.MissedAppointment = "N";
            }

            if (coYesRadio.Checked)
            {
                newVaccine.Comorbidity = "Yes";
            }
            else if (coNoRadio.Checked)
            {
                newVaccine.Comorbidity = "No";
            }

            if (EUAYesRadio.Checked)
            {
                newVaccine.RevievedEUA = "Yes";
            }
            else if (EUANoRadio.Checked)
            {
                newVaccine.RevievedEUA = "No";
            }

            /*
            if (refuseYesRadio.Checked)
            {
                newVaccine.RefusedVaccination = "Yes";
            }
            else if (refusedNoRadio.Checked)
            {
                newVaccine.RefusedVaccination = "No";
            }
            */

            try
            {
                CheckInput.CheckPatientFields(newPatient);

                if (PatientMethods.PatientExists(fNameTextBox.Text.Trim(), lNameTextBox.Text.Trim(), DOBTextBox.Text))
                {
                    if (refuseYesRadio.Checked)
                    {
                        newPatient = PatientMethods.GetPatient(fNameTextBox.Text.Trim(), lNameTextBox.Text.Trim(), DOBTextBox.Text);

                        Refusal newRefusal = new Refusal();
                        newRefusal.patient_ID = newPatient.patient_ID;
                        newRefusal.User_ID = newUser.User_ID;
                        newRefusal.Date = dateAdministeredTextBox.Value;

                        if (refusalReasonTextBox.SelectedIndex == -1)
                        {
                            MessageBox.Show("A reason must be given for a vaccination refusal.");
                        }
                        else
                        {
                            newRefusal.Refusal_Reason = refusalReasonTextBox.Text.Trim();

                            if (MessageBox.Show("Would you like to save vaccine refusal information for this patient?",
                                "Patient " + fNameTextBox.Text.Trim() + " " + lNameTextBox.Text.Trim() + ".", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                try
                                {
                                    newInsurance.patient_ID = newPatient.patient_ID;

                                    if (CheckInput.CheckForInsurance(newInsurance) && !(InsuranceMethods.InsuranceExists(newInsurance)))
                                    {
                                        try
                                        {
                                            InsuranceMethods.AddInsurance(newInsurance);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw ex;
                                        }
                                    }
                                    else if (CheckInput.CheckForInsurance(newInsurance) && InsuranceMethods.InsuranceExists(newInsurance))
                                    {
                                        try
                                        {
                                            InsuranceMethods.UpdateInsurance(newInsurance);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw ex;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                                try
                                {
                                    if (RefusalMethods.AddRefusal(newRefusal, newUser))
                                    {
                                        //update patient address here
                                        if (CheckForAddressInput())
                                        {
                                            newPatient.Street1 = street1TextBox.Text.Trim();
                                            newPatient.Street2 = street2TextBox.Text.Trim();
                                            newPatient.City = cityTextBox.Text.Trim();
                                            newPatient.County = countyTextBox.Text.Trim();
                                            newPatient.State = stateTextBox.Text.Trim();
                                            newPatient.Zipcode = zipcodeTextBox.Text.Trim();
                                            PatientMethods.UpdatePatientAddress(newPatient);
                                        }
                                        MessageBox.Show("Vaccine refusal information for this patient has been recorded.");

                                        ClearAllFields();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }

                            }
                        }
                   
                    }
                    else if(MessageBox.Show("Would you like to save vaccine information for this patient?",
                         "Patient " + fNameTextBox.Text.Trim() + " " + lNameTextBox.Text.Trim() + ".", MessageBoxButtons.YesNo) == DialogResult.Yes)
                         {
                             newPatient = PatientMethods.GetPatient(fNameTextBox.Text.Trim(), lNameTextBox.Text.Trim(), DOBTextBox.Text);

                             newVaccine.patient_ID = newPatient.patient_ID;

                             CheckInput.CheckRequiredFields(newPatient, newVaccine);

                             try
                             {
                                 if (VaccineMethods.VaccineExists(newVaccine.patient_ID, newVaccine.NumDose))
                                 {
                                     MessageBox.Show("A record of dose number " + newVaccine.NumDose + " already exists for " + newPatient.First_Name + " " + newPatient.Last_Name
                                         + ". A duplicate record can not be created.");
                                 }
                                 else
                                 {
                                     try
                                     {
                                         newInsurance.patient_ID = newPatient.patient_ID;

                                         if (CheckInput.CheckForInsurance(newInsurance) && !(InsuranceMethods.InsuranceExists(newInsurance)))
                                         {
                                             try
                                             {
                                                 InsuranceMethods.AddInsurance(newInsurance);
                                             }
                                             catch (Exception ex)
                                             {
                                                 throw ex;
                                             }
                                         }
                                         else if (CheckInput.CheckForInsurance(newInsurance) && InsuranceMethods.InsuranceExists(newInsurance))
                                         {
                                             try
                                             {
                                                 InsuranceMethods.UpdateInsurance(newInsurance);
                                             }
                                             catch (Exception ex)
                                             {
                                                 throw ex;
                                             }
                                         }
                                     }
                                     catch (Exception ex)
                                     {
                                         throw ex;
                                     }
                                     try
                                     {
                                         //bool status = VaccineMethods.AddVaccine(newVaccine);

                                         if (VaccineMethods.AddVaccine(newVaccine, newUser))
                                         {
                                             //update address info
                                             if (CheckForAddressInput())
                                             {
                                                 newPatient.Street1 = street1TextBox.Text.Trim();
                                                 newPatient.Street2 = street2TextBox.Text.Trim();
                                                 newPatient.City = cityTextBox.Text.Trim();
                                                 newPatient.County = countyTextBox.Text.Trim();
                                                 newPatient.State = stateTextBox.Text.Trim();
                                                 newPatient.Zipcode = zipcodeTextBox.Text.Trim();
                                                 PatientMethods.UpdatePatientAddress(newPatient);
                                             }
                                             MessageBox.Show("Vaccine information for " + fNameTextBox.Text.Trim() + " " + lNameTextBox.Text.Trim() + " has been updated.");

                                             ClearAllFields();
                                         }
                                         else
                                         {
                                             MessageBox.Show("Vaccine information COULD NOT be updated.");
                                         }
                                     }
                                     catch (Exception ex)
                                     {
                                         MessageBox.Show(ex.Message);
                                     }
                                 }
                             }
                             catch (Exception ex)
                             {
                                 MessageBox.Show(ex.Message);
                             }
                         }
                }
                else
                {
                    string connectionString = GetConnectionString();
                    string sqlString = "select max(patient_ID) from Patient";

                    int patientMaxID;

                    using (IDbConnection db = new SqlConnection(connectionString))
                    {
                        patientMaxID = (Convert.ToInt32(db.ExecuteScalar(sqlString)));
                    }

                    newPatient.patient_ID = (patientMaxID + 1);


                    try
                    {
                        CheckInput.CheckPatientFields(newPatient);

                        if (refuseYesRadio.Checked)
                        {
                            Refusal newRefusal = new Refusal();
                            newRefusal.patient_ID = newPatient.patient_ID;
                            newRefusal.User_ID = newUser.User_ID;
                            newRefusal.Date = dateAdministeredTextBox.Value;

                            if (refusalReasonTextBox.SelectedIndex == -1)
                            {
                                MessageBox.Show("A reason must be given for a vaccination refusal.");
                            }
                            else if (PatientMethods.AddPatient(newPatient))
                            {
                                newRefusal.Refusal_Reason = refusalReasonTextBox.Text.Trim();

                                if (MessageBox.Show("Would you like to save vaccine refusal information for this patient?",
                                        "Patient " + fNameTextBox.Text.Trim() + " " + lNameTextBox.Text.Trim() + ".", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    try
                                    {
                                        newInsurance.patient_ID = newPatient.patient_ID;

                                        if (CheckInput.CheckForInsurance(newInsurance) && !(InsuranceMethods.InsuranceExists(newInsurance)))
                                        {
                                            try
                                            {
                                                InsuranceMethods.AddInsurance(newInsurance);
                                            }
                                            catch (Exception ex)
                                            {
                                                throw ex;
                                            }
                                        }
                                        else if (CheckInput.CheckForInsurance(newInsurance) && InsuranceMethods.InsuranceExists(newInsurance))
                                        {
                                            try
                                            {
                                                InsuranceMethods.UpdateInsurance(newInsurance);
                                            }
                                            catch (Exception ex)
                                            {
                                                throw ex;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }

                                    try
                                    {
                                        if (RefusalMethods.AddRefusal(newRefusal, newUser))
                                        {
                                            MessageBox.Show("Vaccine refusal information for this patient has been recorded.");

                                            ClearAllFields();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }
                                }
                            }
                        }
                        else
                        {

                            newVaccine.patient_ID = newPatient.patient_ID;

                            CheckInput.CheckRequiredFields(newPatient, newVaccine);

                            try
                            {
                                if (PatientMethods.AddPatient(newPatient) && MessageBox.Show("Would you like to save vaccine information for this patient?",
                                    "Patient " + fNameTextBox.Text.Trim() + " " + lNameTextBox.Text.Trim() + ".", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    try
                                    {
                                        newInsurance.patient_ID = newPatient.patient_ID;

                                        if (CheckInput.CheckForInsurance(newInsurance) && !(InsuranceMethods.InsuranceExists(newInsurance)))
                                        {
                                            try
                                            {
                                                InsuranceMethods.AddInsurance(newInsurance);
                                            }
                                            catch (Exception ex)
                                            {
                                                throw ex;
                                            }
                                        }
                                        else if (CheckInput.CheckForInsurance(newInsurance) && InsuranceMethods.InsuranceExists(newInsurance))
                                        {
                                            try
                                            {
                                                InsuranceMethods.UpdateInsurance(newInsurance);
                                            }
                                            catch (Exception ex)
                                            {
                                                throw ex;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }
                                    try
                                    {
                                        if (VaccineMethods.AddVaccine(newVaccine, newUser))
                                        {
                                            MessageBox.Show("Vaccine information for " + fNameTextBox.Text.Trim() + " " + lNameTextBox.Text.Trim() + " has been updated.");

                                            ClearAllFields();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Vaccine information COULD NOT be updated.");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //ClearAllFields();

            
        }

        private void refuseYesRadio_CheckedChanged(object sender, EventArgs e)
        {
            // This block of code simply enables or disables controls within the form
            // based on whether or not the patient has been refused a vaccine
            if (refuseYesRadio.Checked)
            {
                typeTextBox.Text = "";
                productTextBox.Text = "";
                lotNumberTextBox.Text = "";
                manufacturerTextBox.Text = "";
                dateAdministeredTextBox.Text = "";
                expirationDateTextBox.Text = "";
                wastedAmountTextBox.Text = "";
                doseNumberTextBox.Text = "";
                vaccinatorTextBox.Text = "";

                leftarmRadio.Checked = false;
                rightarmRadio.Checked = false;
                extremityleftRadio.Checked = false;
                extremityRightRadio.Checked = false;

                maYesRadio.Checked = false;
                maNoRadio.Checked = false;

                coYesRadio.Checked = false;
                coNoRadio.Checked = false;

                EUAYesRadio.Checked = false;
                EUANoRadio.Checked = false;

                vaccineGroupBox.Enabled = false;

                refusalReasonTextBox.Enabled = true;
            }

            if (refusedNoRadio.Checked)
            {
                vaccineGroupBox.Enabled = true;

                refusalReasonTextBox.SelectedIndex = -1;
                refusalReasonTextBox.Enabled = false;
            }
        }

        private static string GetConnectionString()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ToString();

            return connectionString;
        }


        private void clearButton_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to clear all fields?", "Clear All Fields", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                ClearAllFields();
            }
        }

        public void ClearAllFields()
        {
            // This is a simple method for clearing all the fields if the user needs to reset the form
            searchFName.SelectedIndex = -1;
            searchLName.SelectedIndex = -1;
            fNameTextBox.Text = "";
            mNameTextBox.Text = "";
            lNameTextBox.Text = "";
            maleRadio.Checked = false;
            femaleRadio.Checked = false;
            unknownRadio.Checked = false;
            hispanicCheck.Checked = false;
            notHispanicCheck.Checked = false;
            unknownCheck.Checked = false;
            unreportableCheck.Checked = false;
            alaskanCheck.Checked = false;
            asianCheck.Checked = false;
            hawaiianCheck.Checked = false;
            blackCheck.Checked = false;
            whiteCheck.Checked = false;
            otherCheck.Checked = false;
            unknownRaceCheck.Checked = false;
            unreportableRaceCheck.Checked = false;
            street1TextBox.Text = "";
            street2TextBox.Text = "";
            cityTextBox.Text = "";
            countyTextBox.Text = "";
            stateTextBox.Text = "MO";
            zipcodeTextBox.Text = "";
            insurerTextBox.Text = "";
            primaryHolderTextBox.Text = "";
            insuranceIDTextBox.Text = "";
            leftarmRadio.Checked = false;
            rightarmRadio.Checked = false;
            extremityleftRadio.Checked = false;
            extremityRightRadio.Checked = false;
            typeTextBox.SelectedIndex = -1;
            productTextBox.SelectedIndex = -1;
            lotNumberTextBox.Text = "";
            manufacturerTextBox.SelectedIndex = -1;
            wastedAmountTextBox.SelectedIndex = -1;
            doseNumberTextBox.SelectedIndex = -1;
            vaccinatorTextBox.Text = "";
            maYesRadio.Checked = false;
            maNoRadio.Checked = false;
            coYesRadio.Checked = false;
            coNoRadio.Checked = false;
            
            refuseYesRadio.Checked = false;
            refusedNoRadio.Checked = false;
            refusalReasonTextBox.SelectedIndex = -1;
            refusalReasonTextBox.Enabled = false;
            
            EUAYesRadio.Checked = false;
            EUANoRadio.Checked = false;

            vaccineDataGrid.DataSource = null;

            patientInfoGroupBox.Enabled = true;
            vaccineGroupBox.Enabled = true;

            EnableGroupControls(patientInfoGroupBox.Controls);
            EnableGroupControls(sexGroupBox.Controls);
            EnableGroupControls(raceGroupBox.Controls);
            EnableGroupControls(ethnicityGroupBox.Controls);

            

        }

        private void searchFName_DropDown(object sender, EventArgs e)
        {

        }

        private void searchFName_TextChanged(object sender, EventArgs e)
        {
            // This section determines date of birth based on first and last name
            if (searchFName.Text != "" && searchLName.Text != "")
            {
                string firstName = searchFName.Text.Trim();
                string lastName = searchLName.Text.Trim();
                DateTime lookupDOB;
                string connectionString = GetConnectionString();
                string sqlString = "select Date_of_Birth from Patient where First_Name = @First_Name and Last_Name = @Last_Name";
                try
                {
                    using (IDbConnection db = new SqlConnection(connectionString))
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@First_Name", firstName, DbType.String, ParameterDirection.Input);
                        parameters.Add("@Last_Name", lastName, DbType.String, ParameterDirection.Input);
                        lookupDOB = Convert.ToDateTime(db.ExecuteScalar(sqlString, parameters));
                        searchDOB.Value = lookupDOB;
                    }
                }
                catch /*(Exception ex)*/
                {
                    searchDOB.Value = searchDOB.MinDate;
                }
                //searchDOB.Value = lookupDOB;
            }
 
        }

        private void searchFName_Enter(object sender, EventArgs e)
        {
            // This section determines a list of first names based on the value of the last name
            if (searchLName.Text == "")
            {
                List<string> patientList = new List<string>();
                string connectionString = GetConnectionString();
                string sqlString = "select distinct(First_Name) from Patient order by First_Name";
                try
                {
                    using (IDbConnection db = new SqlConnection(connectionString))
                    {
                        patientList = db.Query<string>(sqlString).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //patientList.Insert(0, "");
                searchFName.DataSource = patientList;
            }
            else
            {
                string lastName = searchLName.Text.Trim();

                List<string> patientList = new List<string>();
                string connectionString = GetConnectionString();
                string sqlString = "select First_Name from Patient where Last_Name = @Last_Name order by First_Name";
                try
                {
                    using (IDbConnection db = new SqlConnection(connectionString))
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@Last_Name", lastName, DbType.String, ParameterDirection.Input);
                        patientList = db.Query<string>(sqlString, parameters).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //patientList.Insert(0, "");
                searchFName.DataSource = patientList;
            }
        }

        private void searchLName_Enter(object sender, EventArgs e)
        {
            // This section determines a list of last names based on the value of the first name
            if (searchFName.Text == "")
            {
                List<string> patientList = new List<string>();
                string connectionString = GetConnectionString();
                string sqlString = "select distinct(Last_Name) from Patient order by Last_Name";
                try
                {
                    using (IDbConnection db = new SqlConnection(connectionString))
                    {
                        patientList = db.Query<string>(sqlString).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //patientList.Insert(0, "");
                searchLName.DataSource = patientList;
            }
            else
            {
                string firstName = searchFName.Text.Trim();

                List<string> patientList = new List<string>();
                string connectionString = GetConnectionString();
                string sqlString = "select Last_Name from Patient where First_Name = @First_Name order by Last_Name";
                try
                {
                    using (IDbConnection db = new SqlConnection(connectionString))
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@First_Name", firstName, DbType.String, ParameterDirection.Input);
                        patientList = db.Query<string>(sqlString, parameters).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //patientList.Insert(0, "");
                searchLName.DataSource = patientList;
            }
        }

        private void searchButton_Click_1(object sender, EventArgs e)
        {
            // This section searches the database for a matching patient record based on first name, last name, and date of birth
            
            Patient newPatient = new Patient();

            Insurance newInsurance = new Insurance();

            if (searchFName.SelectedIndex != -1 && searchLName.SelectedIndex != -1)
            {
                try
                {
                    newPatient = PatientMethods.GetPatient(searchFName.Text.Trim(), searchLName.Text.Trim(), searchDOB.Text.Trim());

                    newInsurance = InsuranceMethods.GetInsurance(newPatient.patient_ID);

                    if (newPatient != null)
                    {
                        ClearAllFields();

                        try
                        {
                            //newPatient = PatientMethods.GetPatient(searchFName.Text.Trim(), searchLName.Text.Trim(), searchDOB.Text.Trim());

                            fNameTextBox.Text = newPatient.First_Name;
                            mNameTextBox.Text = newPatient.Middle_Name;
                            lNameTextBox.Text = newPatient.Last_Name;
                            DOBTextBox.Text = newPatient.Date_of_Birth;
                            street1TextBox.Text = newPatient.Street1;
                            street2TextBox.Text = newPatient.Street2;
                            cityTextBox.Text = newPatient.City;
                            countyTextBox.Text = newPatient.County;
                            stateTextBox.Text = newPatient.State;
                            zipcodeTextBox.Text = newPatient.Zipcode;

                            if (newPatient.Sex == "M")
                            {
                                maleRadio.Checked = true;
                            }
                            else if (newPatient.Sex == "F")
                            {
                                femaleRadio.Checked = true;
                            }
                            else if (newPatient.Sex == "U")
                            {
                                unknownRadio.Checked = true;
                            }

                            if (newPatient.Ethnicity.Contains("Hispanic") && !newPatient.Ethnicity.Contains("Not"))
                            {
                                hispanicCheck.Checked = true;
                            }
                            else
                            {
                                hispanicCheck.Checked = false;
                            }

                            if (newPatient.Ethnicity.Contains("Not"))
                            {
                                notHispanicCheck.Checked = true;
                            }
                            else
                            {
                                notHispanicCheck.Checked = false;
                            }

                            if (newPatient.Ethnicity.Contains("Unknown"))
                            {
                                unknownCheck.Checked = true;
                            }
                            else
                            {
                                unknownCheck.Checked = false;
                            }

                            if (newPatient.Ethnicity.Contains("Unable"))
                            {
                                unreportableCheck.Checked = true;
                            }
                            else
                            {
                                unreportableCheck.Checked = false;
                            }

                            if (newPatient.Race.Contains("Alaskan"))
                            {
                                alaskanCheck.Checked = true;
                            }
                            else
                            {
                                alaskanCheck.Checked = false;
                            }

                            if (newPatient.Race.Contains("Hawaiian"))
                            {
                                hawaiianCheck.Checked = true;
                            }
                            else
                            {
                                hawaiianCheck.Checked = false;
                            }

                            if (newPatient.Race.Contains("Asian"))
                            {
                                asianCheck.Checked = true;
                            }
                            else
                            {
                                asianCheck.Checked = false;
                            }

                            if (newPatient.Race.Contains("Black"))
                            {
                                blackCheck.Checked = true;
                            }
                            else
                            {
                                blackCheck.Checked = false;
                            }

                            if (newPatient.Race.Contains("White"))
                            {
                                whiteCheck.Checked = true;
                            }
                            else
                            {
                                whiteCheck.Checked = false;
                            }

                            if (newPatient.Race.Contains("Other"))
                            {
                                otherCheck.Checked = true;
                            }
                            else
                            {
                                otherCheck.Checked = false;
                            }

                            if (newPatient.Race.Contains("Unknown"))
                            {
                                unknownRaceCheck.Checked = true;
                            }
                            else
                            {
                                unknownRaceCheck.Checked = false;
                            }

                            if (newPatient.Race.Contains("Unable"))
                            {
                                unreportableRaceCheck.Checked = true;
                            }
                            else
                            {
                                unreportableRaceCheck.Checked = false;
                            }

                            //patientInfoGroupBox.Enabled = false;

                            SetReadonlyControls(patientInfoGroupBox.Controls);
                            SetReadonlyControls(sexGroupBox.Controls);
                            SetReadonlyControls(ethnicityGroupBox.Controls);
                            SetReadonlyControls(raceGroupBox.Controls);

                            if (newInsurance != null)
                            {
                                insurerTextBox.Text = newInsurance.Insurer;
                                primaryHolderTextBox.Text = newInsurance.Primary_Holder;
                                insuranceIDTextBox.Text = newInsurance.Group_ID;
                            }

                            try
                            {
                                List<DataGridVaccine> vaccineList = new List<DataGridVaccine>();
                                vaccineList = VaccineMethods.GetVaccines(newPatient.patient_ID);
                                vaccineDataGrid.DataSource = vaccineList;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Patient {searchFName.Text.Trim() + " " + searchLName.Text.Trim()} not found in database.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void searchLName_TextChanged(object sender, EventArgs e)
        {
            // This section determines date of birth based on first and last name
            if (searchFName.Text != "" && searchLName.Text != "")
            {
                string firstName = searchFName.Text.Trim();
                string lastName = searchLName.Text.Trim();
                DateTime lookupDOB;
                string connectionString = GetConnectionString();
                string sqlString = "select Date_of_Birth from Patient where First_Name = @First_Name and Last_Name = @Last_Name";
                try
                {
                    using (IDbConnection db = new SqlConnection(connectionString))
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@First_Name", firstName, DbType.String, ParameterDirection.Input);
                        parameters.Add("@Last_Name", lastName, DbType.String, ParameterDirection.Input);
                        lookupDOB = Convert.ToDateTime(db.ExecuteScalar(sqlString, parameters));
                        searchDOB.Value = lookupDOB;
                    }
                }
                catch /*(Exception ex)*/
                {
                    searchDOB.Value = searchDOB.MinDate;
                }
                //searchDOB.Value = lookupDOB;
            }
        }

        private void vaccineDataGrid_DoubleClick(object sender, EventArgs e)
        {
            // This section populates fields with vaccine information from the datagrid by double clicking on a record
            
            /*
            if (vaccineDataGrid.SelectedRows.Count > 0)
            {
                doseNumberTextBox.Text = vaccineDataGrid.SelectedRows[0].Cells[1].Value.ToString();
                vaccinatorTextBox.Text = vaccineDataGrid.SelectedRows[0].Cells[2].Value.ToString();

                if (vaccineDataGrid.SelectedRows[0].Cells[3].Value.ToString() == "Left Arm")
                {
                    leftarmRadio.Checked = true;
                    rightarmRadio.Checked = false;
                    extremityleftRadio.Checked = false;
                    extremityRightRadio.Checked = false;
                }
                else if (vaccineDataGrid.SelectedRows[0].Cells[3].Value.ToString() == "Right Arm")
                {
                    leftarmRadio.Checked = false;
                    rightarmRadio.Checked = true;
                    extremityleftRadio.Checked = false;
                    extremityRightRadio.Checked = false;
                }
                else if (vaccineDataGrid.SelectedRows[0].Cells[3].Value.ToString() == "Lower Extremity Left")
                {
                    leftarmRadio.Checked = false;
                    rightarmRadio.Checked = false;
                    extremityleftRadio.Checked = true;
                    extremityRightRadio.Checked = false;
                }
                else if (vaccineDataGrid.SelectedRows[0].Cells[3].Value.ToString() == "Lower Extremity Right")
                {
                    leftarmRadio.Checked = false;
                    rightarmRadio.Checked = false;
                    extremityleftRadio.Checked = false;
                    extremityRightRadio.Checked = true;
                }

                productTextBox.Text = vaccineDataGrid.SelectedRows[0].Cells[4].Value.ToString();
                manufacturerTextBox.Text = vaccineDataGrid.SelectedRows[0].Cells[5].Value.ToString();
                typeTextBox.Text = vaccineDataGrid.SelectedRows[0].Cells[6].Value.ToString();
                lotNumberTextBox.Text = vaccineDataGrid.SelectedRows[0].Cells[7].Value.ToString();
                expirationDateTextBox.Text = vaccineDataGrid.SelectedRows[0].Cells[8].Value.ToString();
                dateAdministeredTextBox.Text = vaccineDataGrid.SelectedRows[0].Cells[9].Value.ToString();
                wastedAmountTextBox.Text = vaccineDataGrid.SelectedRows[0].Cells[10].Value.ToString();

                if (vaccineDataGrid.SelectedRows[0].Cells[11].Value.ToString() == "Y")
                {
                    maYesRadio.Checked = true;
                    maNoRadio.Checked = false;
                }
                else if (vaccineDataGrid.SelectedRows[0].Cells[11].Value.ToString() == "N")
                {
                    maYesRadio.Checked = false;
                    maNoRadio.Checked = true;
                }

                if (vaccineDataGrid.SelectedRows[0].Cells[12].Value.ToString() == "Yes")
                {
                    coYesRadio.Checked = true;
                    coNoRadio.Checked = false;
                }
                else if (vaccineDataGrid.SelectedRows[0].Cells[12].Value.ToString() == "No")
                {
                    coYesRadio.Checked = false;
                    coNoRadio.Checked = true;
                }

                if (vaccineDataGrid.SelectedRows[0].Cells[13].Value.ToString() == "Yes")
                {
                    EUAYesRadio.Checked = true;
                    EUANoRadio.Checked = false;
                }
                else if (vaccineDataGrid.SelectedRows[0].Cells[13].Value.ToString() == "No")
                {
                    EUAYesRadio.Checked = false;
                    EUANoRadio.Checked = true;
                }
            
                
            }
        */
        }

        private void zipcodeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // This little snippet restricts the user from entering any character into the zip code
            // textbox other than a numeric digit.
            e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void fNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // This restricts the user from entering numeric digits in the first name text box.
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space || e.KeyChar == '.');
        }

        private void mNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // This restricts the user from entering numeric digits in the middle name text box.
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space || e.KeyChar == '.');
        }

        private void lNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // This restricts the user from entering numeric digits in the last name text box.
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space || e.KeyChar == '.');
        }

        private void cityTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // This restricts the user from entering numeric digits in the city text box.
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space || e.KeyChar == '.');
        }

        private void countyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // This restricts the user from entering numeric digits in the county text box.
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space || e.KeyChar == '.');
        }

        private void vaccinatorTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // This restricts the user from entering numeric digits in the vaccinator text box.
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space || e.KeyChar == '.');
        }

        private void primaryHolderTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // This restricts the user from entering numeric digits in the insurance holder text box.
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space || e.KeyChar == '.');
        }

        public bool CheckForAddressInput()
        {
            bool status;

            if (street1TextBox.Text.Trim() != "" &&
                cityTextBox.Text.Trim() != "" &&
                countyTextBox.Text.Trim() != "" &&
                zipcodeTextBox.Text.Trim() != "")
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return status;
        }

        private void vaccineReportButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to create a daily vaccination report?", "Daily Report Confirmation.", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (VaccineMethods.DailyVaccineExists(System.DateTime.Today))
                {
                    try
                    {
                        string today = (System.DateTime.Today).ToString("yyyy'-'MM'-'dd");
                        string filename = $"DailyVaccineReport({today})";
                        string directory = System.Environment.CurrentDirectory;

                        List<Vaccine> vaccineList = new List<Vaccine>();
                        vaccineList = VaccineMethods.GetTodaysVaccines();

                        string headerLine = string.Join(",", vaccineList[0].GetType().GetProperties().Select(p => p.Name));

                        var dataLines = from vax in vaccineList
                                        let dataLine = string.Join(",", vax.GetType()
                                        .GetProperties().Select(p => p.GetValue(vax)))
                                        select dataLine;

                        var csvData = new List<string>();
                        csvData.Add(headerLine);
                        csvData.AddRange(dataLines);

                        string csvFilePath = $@"{directory}\{filename}.csv";

                        System.IO.File.WriteAllLines(csvFilePath, csvData);

                        MessageBox.Show("CSV file for daily vaccination records successfully created.");
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    MessageBox.Show("There are no daily vaccine records to report. CSV file can not be created.");
                }
            }
        }

        private void refusalReportButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to create a daily refusal report?", "Daily Report Confirmation.", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (RefusalMethods.DailyRefusalExists(System.DateTime.Today))
                {
                    try
                    {
                        string today = (System.DateTime.Today).ToString("yyyy'-'MM'-'dd");
                        string filename = $"DailyRefusalReport({today})";
                        string directory = System.Environment.CurrentDirectory;

                        List<Refusal> refusalList = new List<Refusal>();
                        refusalList = RefusalMethods.GetTodaysRefusals();

                        string headerLine = string.Join(",", refusalList[0].GetType().GetProperties().Select(p => p.Name));

                        var dataLines = from vax in refusalList
                                        let dataLine = string.Join(",", vax.GetType()
                                        .GetProperties().Select(p => p.GetValue(vax)))
                                        select dataLine;

                        var csvData = new List<string>();
                        csvData.Add(headerLine);
                        csvData.AddRange(dataLines);

                        string csvFilePath = $@"{directory}\{filename}.csv";

                        System.IO.File.WriteAllLines(csvFilePath, csvData);

                        MessageBox.Show("CSV file for daily refusal records successfully created.");
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    MessageBox.Show("There are no daily refusal records to report. CSV file can not be created.");
                }
            }
        }

        private void SetReadonlyControls(Control.ControlCollection controlCollection)
        {
            if (controlCollection == null)
            {
                return;
            }
            foreach (RadioButton r in controlCollection.OfType<RadioButton>())
            {
                r.Enabled = false; //RadioButtons do not have readonly property
            }
            foreach (ComboBox c in controlCollection.OfType<ComboBox>())
            {
                c.Enabled = false;//ComboBoxes do not have readonly property
            }
            foreach (TextBoxBase c in controlCollection.OfType<TextBoxBase>())
            {
                c.ReadOnly = true;
            }
            foreach (DateTimePicker d in controlCollection.OfType<DateTimePicker>())
            {
                d.Enabled = false;
            }
            foreach (CheckBox ch in controlCollection.OfType<CheckBox>())
            {
                ch.Enabled = false;
            }
        }

        private void EnableGroupControls(Control.ControlCollection controlCollection)
        {
            foreach (RadioButton r in controlCollection.OfType<RadioButton>())
            {
                r.Enabled = true;
            }
            foreach (ComboBox c in controlCollection.OfType<ComboBox>())
            {
                c.Enabled = true;
            }
            foreach (TextBoxBase t in controlCollection.OfType<TextBoxBase>())
            {
                t.ReadOnly = false;
            }
            foreach (DateTimePicker d in controlCollection.OfType<DateTimePicker>())
            {
                d.Enabled = true;
            }
            foreach (CheckBox ch in controlCollection.OfType<CheckBox>())
            {
                ch.Enabled = true;
            }
        }
    }
}
