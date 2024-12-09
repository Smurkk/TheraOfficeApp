using Api.Clinic.Database;
using Library.Clinic.DTO;
using Library.Clinic.Models;

namespace Api.Clinic.Enterprise
{
    public class PatientEC
    {
        public PatientEC() { }

        public IEnumerable<PatientDTO> Patients
        {
            get
            {
                var appointments = new AppointmentEC().Appointments;
                var patients = new MsSqlContext().GetPatients();
                return patients;
            }
        }

        

        public IEnumerable<PatientDTO>? Search(string query)
        {
            return new MsSqlContext().GetPatients()
                .Where(p => p.Name.ToUpper()
                .Contains(query?.ToUpper() ?? string.Empty))
                .Select(p=> new PatientDTO((Patient)p));
        }

        public PatientDTO? GetById(int id)
        {
            var patient = FakeDatabase.Patients.FirstOrDefault(p => p.Id == id);
            if (patient != null)
            {
                return new PatientDTO(patient);
            }

            return null;
        }

        public void Delete(int Id)
        {
            new MsSqlContext().DeletePatient(Id);
            return;
        }

        public PatientDTO Update(PatientDTO patient)
        {
            new MsSqlContext().UpdatePatient(patient);
            return patient;
        }

        public PatientDTO Create(PatientDTO patient)
        {
            new MsSqlContext().CreatePatient(patient);
            return patient;
        }
    }
}
