using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaxTrax_2._0_
{
    class Patient
    {
        public int patient_ID { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Date_of_Birth { get; set; }
        public string Sex { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Ethnicity { get; set; }
        public string Race { get; set; }

        /*
        public Patient()
        {
            Ethnicity = new List<string>();
            Race = new List<string>();
        }
        */
    }
}
