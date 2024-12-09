using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Clinic.Models
{
    public class Appointment
    {
 
        public int Id { get; set; }

        public double Price { get; set; }
        public Treatment Treatment { get; set; }
        public double OriginalPrice { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; } 
        
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }
    }
}
