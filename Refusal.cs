using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaxTrax_2._0_
{
    class Refusal
    {
        public int patient_ID { get; set; }
        public string Refusal_Reason { get; set; }
        public DateTime Date { get; set; }
        public int User_ID { get; set; }
    }
}
