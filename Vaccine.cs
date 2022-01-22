using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaxTrax_2._0_
{
    class Vaccine
    {
        public int patient_ID { get; set; }
        public int NumDose   { get; set; }
        public string Vaccinator_Name { get; set; }
        public string Administration_site { get; set; }
        public string Product { get; set; }
        public string Manufacture { get; set; }
        public string Typecvx { get; set; }
        public string LotNum { get; set; }
        public string ExpirationDate { get; set; }
        public string DateAdministered { get; set; }
        public int NumWasted { get; set; }
        public string MissedAppointment { get; set; }
        public string Comorbidity { get; set; }
        public string RevievedEUA { get; set; }
        public int User_ID { get; set; }
    }
}
