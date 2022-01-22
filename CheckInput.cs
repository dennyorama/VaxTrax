using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaxTrax_2._0_
{
    class CheckInput
    {
        public static void CheckRequiredFields(Patient newPatient, Vaccine newVaccine)
        {
            if (newPatient.First_Name == "")
            {
                throw new Exception("Patient first name is a required field.");
            }
            if (newPatient.Last_Name == "")
            {
                throw new Exception("Patient last name is a required field.");
            }
            if (newPatient.Sex == null)
            {
                throw new Exception("Patient sex is a required field.");
            }
            if (newPatient.Date_of_Birth == "")
            {
                throw new Exception("Patient date of birth is a required field.");
            }
            if (newPatient.Street1 == "")
            {
                throw new Exception("Patient address is required.");
            }
            if (newPatient.City == "")
            {
                throw new Exception("Patient address is required.");
            }
            if (newPatient.County == "")
            {
                throw new Exception("Patient address is required.");
            }
            if (newPatient.State == "")
            {
                throw new Exception("Patient address is required.");
            }
            if (newPatient.Zipcode == "")
            {
                throw new Exception("Patient address is required.");
            }
            if (newPatient.Race == null)
            {
                throw new Exception("Patient must select at least one option for race.");
            }
            if (newPatient.Ethnicity == null)
            {
                throw new Exception("Patient must select at least one option for ethnicity.");
            }

            
            if (newVaccine.Typecvx == "")
            {
                throw new Exception("Vaccine type is a required field.");
            }
            if (newVaccine.Product == "")
            {
                throw new Exception("Vaccine product is a required field.");
            }
            if (newVaccine.DateAdministered == "")
            {
                throw new Exception("The administered date of the vaccine is required.");
            }
            if (newVaccine.Manufacture == "")
            {
                throw new Exception("Vaccine manufacturer is a required field.");
            }
            if (newVaccine.LotNum == "")
            {
                throw new Exception("Vaccine lot number is a required field.");
            }
            if (newVaccine.ExpirationDate == "")
            {
                throw new Exception("The expiration date of the vaccine is required.");
            }
            if (newVaccine.NumWasted < 0 || newVaccine.NumWasted > 6)
            {
                throw new Exception("The wasted amount of vaccine doses is required and must be a reasonable number, even if the wasted amount is 0.");
            }
            if (newVaccine.Administration_site == null)
            {
                throw new Exception("If a vaccine was administered, the administration site is required.");
            }
            if (newVaccine.NumDose < 1 || newVaccine.NumDose >= 10)
            {
                throw new Exception("Vaccine dose number is a required field.");
            }
            if (newVaccine.MissedAppointment == null)
            {
                throw new Exception("Missed appointment is a required field.");
            }
            if (newVaccine.Comorbidity == null)
            {
                throw new Exception("Comorbidity is a required field.");
            }

            /*
            if (newVaccine.RefusedVaccination == "")
            {
                throw new Exception("Whether or not the patient was refused a vaccination is a required field.");
            }
            */

            if (newVaccine.Vaccinator_Name == "")
            {
                throw new Exception("Vaccinator is a required field.");
            }
            if (newVaccine.RevievedEUA == null)
            {
                throw new Exception("If a patient receives a vaccination, then the patient must receive an EUA fact sheet.");
            }

            /*
            if (newVaccine.RefusedVaccination == "Yes")
            {
                if (newVaccine.RefusalReason == "")
                {
                    throw new Exception("If a patient was refused vaccination, then a reason must be given.");
                }
            }
            */
        }

        public static void CheckPatientFields(Patient newPatient)
        {
            if (newPatient.First_Name == "")
            {
                throw new Exception("Patient first name is a required field.");
            }
            if (newPatient.Last_Name == "")
            {
                throw new Exception("Patient last name is a required field.");
            }
            if (newPatient.Sex == null)
            {
                throw new Exception("Patient sex is a required field.");
            }
            if (newPatient.Date_of_Birth == "")
            {
                throw new Exception("Patient date of birth is a required field.");
            }
            if (newPatient.Street1 == "")
            {
                throw new Exception("Patient address is required.");
            }
            if (newPatient.City == "")
            {
                throw new Exception("Patient address is required.");
            }
            if (newPatient.County == "")
            {
                throw new Exception("Patient address is required.");
            }
            if (newPatient.State == "")
            {
                throw new Exception("Patient address is required.");
            }
            if (newPatient.Zipcode == "" || newPatient.Zipcode.Length < 5)
            {
                throw new Exception("A valid zip code is required.");
            }
            if (newPatient.Race == null)
            {
                throw new Exception("Patient must select at least one option for race.");
            }
            if (newPatient.Ethnicity == null)
            {
                throw new Exception("Patient must select at least one option for ethnicity.");
            }
        }

        public static void CheckVaccineFields(Vaccine newVaccine)
        {
            if (newVaccine.Typecvx == "")
            {
                throw new Exception("Vaccine type is a required field.");
            }
            if (newVaccine.Product == "")
            {
                throw new Exception("Vaccine product is a required field.");
            }
            if (newVaccine.DateAdministered == "")
            {
                throw new Exception("The administered date of the vaccine is required.");
            }
            if (newVaccine.Manufacture == "")
            {
                throw new Exception("Vaccine manufacturer is a required field.");
            }
            if (newVaccine.LotNum == "")
            {
                throw new Exception("Vaccine lot number is a required field.");
            }
            if (newVaccine.ExpirationDate == "")
            {
                throw new Exception("The expiration date of the vaccine is required.");
            }
            if (newVaccine.NumWasted < 0 || newVaccine.NumWasted > 6)
            {
                throw new Exception("The wasted amount of vaccine doses is required and must be a reasonable number, even if the wasted amount is 0.");
            }
            if (newVaccine.Administration_site == null)
            {
                throw new Exception("If a vaccine was administered, the administration site is required.");
            }
            if (newVaccine.NumDose < 1 || newVaccine.NumDose >= 10)
            {
                throw new Exception("Vaccine dose number is a required field.");
            }
            if (newVaccine.MissedAppointment == null)
            {
                throw new Exception("Missed appointment is a required field.");
            }
            if (newVaccine.Comorbidity == null)
            {
                throw new Exception("Comorbidity is a required field.");
            }

            /*
            if (newVaccine.RefusedVaccination == "")
            {
                throw new Exception("Whether or not the patient was refused a vaccination is a required field.");
            }
            */

            if (newVaccine.Vaccinator_Name == "")
            {
                throw new Exception("Vaccinator is a required field.");
            }
            if (newVaccine.RevievedEUA == null)
            {
                throw new Exception("If a patient receives a vaccination, then the patient must receive an EUA fact sheet.");
            }

            /*
            if (newVaccine.RefusedVaccination == "Yes")
            {
                if (newVaccine.RefusalReason == "")
                {
                    throw new Exception("If a patient was refused vaccination, then a reason must be given.");
                }
            }
            */
        }

        public static bool CheckForInsurance(Insurance objInsurance)
        {
            bool hasInsurance;
            
            if (objInsurance.patient_ID != 0 && objInsurance.Insurer != "" && objInsurance.Primary_Holder != "" && objInsurance.Group_ID != "")
            {
                hasInsurance = true;
            }
            else
            {
                hasInsurance = false;
            }

            return hasInsurance;
        }
    }
}
