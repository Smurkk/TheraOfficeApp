using Api.Clinic.Database;
using Library.Clinic.DTO;
using Library.Clinic.Models;
using System.Net;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;

namespace Api.Clinic.Enterprise
{
    public class PhysicianEC
    {
        public PhysicianEC() { }

        public IEnumerable<PhysicianDTO> Physicians
        {
            get
            {
                return FakeDatabase.Physicians.Take(100).Select(p => new PhysicianDTO(p));
            }
        }

        public IEnumerable<PhysicianDTO>? Search(string query)
        {
            return FakeDatabase.Physicians
                .Where(p => p.Name.ToUpper()
                .Contains(query?.ToUpper() ?? string.Empty))
                .Select(p=> new PhysicianDTO(p));
        }

        public PhysicianDTO? GetById(int id)
        {
            var physician = FakeDatabase.Physicians.FirstOrDefault(p => p.Id == id);
            if (physician != null)
            {
                return new PhysicianDTO(physician);
            }

            return null;
        }

        public PhysicianDTO? Delete(int id)
        {
            var physicianToDelete = FakeDatabase.Physicians.FirstOrDefault(p => p.Id == id);
            if (physicianToDelete != null)
            {
             
                FakeDatabase.Physicians.Remove(physicianToDelete);
                return new PhysicianDTO(physicianToDelete);
            }

            return null;

        }

        public Physician? AddOrUpdate(PhysicianDTO? physician)
        {
            if(physician == null)
            {
                return null;
            }
            return FakeDatabase.AddOrUpdatePhysician(new Physician(physician));
        }


    }
}
