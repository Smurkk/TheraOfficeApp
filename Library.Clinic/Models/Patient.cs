using Library.Clinic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Clinic.Models
{
    public class Patient
    {
        public override string ToString()
        {
            return $"[{Id}] {Name}";
        }

        //TODO: Remove this and put it on a ViewModel instead
        public string Display
        {
            get
            {
                return $"[{Id}] {Name}";
            }
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime Birthday { get; set; }
        public string? Address { get; set; }
        public string? SSN { get; set; }

        public double InsurancePolicy {  get; set; }
        public string abcd1 { get; set; }
        public string abcd2 { get; set; }
        public string abcd3 { get; set; }
        public string abcd4 { get; set; }
        public string abcd5 { get; set; }
        public string abcd6 { get; set; }
        public string abcd7 { get; set; }
        public string abcd8 { get; set; }


        public Patient()
        {
            Name = string.Empty;
            Address = string.Empty;
            Birthday = DateTime.MinValue;
            SSN = string.Empty;
            InsurancePolicy = 0;
        }

        public Patient(PatientDTO p)
        {
            Id = p.Id;
            Name = p.Name;
            Birthday = p.Birthday;
            Address = p.Address;
            SSN = p.SSN;
            InsurancePolicy = p.InsurancePolicy;
        }

        public static explicit operator Patient(PatientDTO dto)
        {
            return new Patient
            {
                Id = dto.Id,
                Name = dto.Name,
                Birthday = dto.Birthday,
                Address = dto.Address,
                SSN = dto.SSN,
                InsurancePolicy = dto.InsurancePolicy
            };
        }

    }
}