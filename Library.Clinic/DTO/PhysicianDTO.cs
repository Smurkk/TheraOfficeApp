using Library.Clinic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Clinic.DTO
{
    public class PhysicianDTO
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
        public DateTime? Birthday { get; set; }
        public string? Address { get; set; }

        public string? SSN { get; set; }

        public PhysicianDTO() { }

        public PhysicianDTO(Physician p )
        {
            Id = p.Id ;
            Name = p.Name ;
            Birthday = p.Birthday ;
            Address = p.Address ;
            SSN = p.SSN ;

        }

        public static explicit operator PhysicianDTO(Physician physician)
        {
            return new PhysicianDTO
            {
                Id = physician.Id,
                Name = physician.Name,
                Birthday = physician.Birthday,
                Address = physician.Address,
                SSN = physician.SSN
            };
        }


    }
}
