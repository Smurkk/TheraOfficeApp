using Library.Clinic.Models;

namespace Api.Clinic.Database
{

    static public class FakeDatabase
    {

        public static int LastKey
        {
            get
            {
                if (Patients.Any())
                {
                    return Patients.Select(x => x.Id).Max();
                }
                return 0;
            }
        }

        public static int LastKeyPhysician
        {
            get
            {
                if (Physicians.Any())
                {
                    return Physicians.Select(x => x.Id).Max();
                }
                return 0;
            }
        }
        private static List<Patient> patients = new List<Patient>
                {
                    new Patient{Id = 1, Name = "John Doe", InsurancePolicy = 0.0}
                    , new Patient{Id = 2, Name = "Jane Doe", InsurancePolicy = 0.0}
                };
        public static List<Patient> Patients { get
            {
                return patients;
            } 
        }

        private static List<Physician> physicians = new List<Physician>
                {
                  new Physician{Id = 1, Name = "Alberto Himothy"}
                , new Physician{Id = 2, Name = "Jane John Jones"}
                };
        public static List<Physician> Physicians
        {
            get
            {
                return physicians;
            }
        }

        public static Patient? AddOrUpdatePatient(Patient? patient)
        {
            if (patient == null)
            {
                return null;
            }
            bool isAdd = false;
            if (patient.Id <= 0)
            {
                patient.Id = LastKey + 1;
                isAdd = true;
            }

            if (isAdd)
            {
                Patients.Add(patient);
            }

            return patient;
        }

        public static Physician? AddOrUpdatePhysician(Physician? physician)
        {
            if (physician == null)
            {
                return null;
            }
            bool isAdd = false;
            if (physician.Id <= 0)
            {
                physician.Id = LastKeyPhysician + 1;
                isAdd = true;
            }

            if (isAdd)
            {
                Physicians.Add(physician);
            }

            return physician;
        }
    }
}
