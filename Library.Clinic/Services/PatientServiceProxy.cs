using Library.Clinic.DTO;
using Library.Clinic.Models;
using Newtonsoft.Json;
using PP.Library.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Library.Clinic.Services
{
    public class PatientServiceProxy
    {
        private static object _lock = new object();
        public static PatientServiceProxy Current
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new PatientServiceProxy();
                    }
                }
                return instance;
            }
        }

        private static PatientServiceProxy? instance;
        private PatientServiceProxy()
        {
            instance = null;

            var patientsData = new WebRequestHandler().Get("/Patient").Result;

            Patients = JsonConvert.DeserializeObject<List<PatientDTO>>(patientsData) ?? new List<PatientDTO>();
        }

        public int LastKey
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

        private List<PatientDTO> patients;
        public List<PatientDTO> Patients
        {
            get
            {
                return patients;
            }

            private set
            {
                if (patients != value)
                {
                    patients = value;
                }
            }
        }

        public async Task<List<PatientDTO>> Search(string query)
        {
            var patientsPayload = await new WebRequestHandler().Post("/Patient/Search", new Query(query));

            Patients = JsonConvert.DeserializeObject<List<PatientDTO>>(patientsPayload) ?? new List<PatientDTO>();

            return Patients;
        }

        public async Task<PatientDTO?> AddOrUpdatePatient(PatientDTO patient)
        {
            string payload = null;

            if (patient.Id == 0)
            {
                // New patient, make a POST request to add the patient
                payload = await new WebRequestHandler().Post("/Patient", patient);
            }
            else
            {
                // Existing patient, make a POST request to update the patient
                payload = await new WebRequestHandler().Post("/Patient/UpdatePatient", patient);
            }

            var updatedPatient = JsonConvert.DeserializeObject<PatientDTO>(payload);

            if (updatedPatient != null && updatedPatient.Id > 0)
            {
                if (patient.Id == 0)
                {
                    // If it was a new patient, add to the Patients collection
                    Patients.Add(updatedPatient);
                }
                else
                {
                    // If updating, find the current patient in the collection
                    var currentPatient = Patients.FirstOrDefault(p => p.Id == updatedPatient.Id);
                    if (currentPatient != null)
                    {
                        var index = Patients.IndexOf(currentPatient);
                        Patients[index] = updatedPatient;

                        // Update appointments for the existing patient
                        UpdateAppointmentsForPatient(updatedPatient);
                    }
                    else
                    {
                        // If patient not found in collection, add it
                        Patients.Add(updatedPatient);
                    }
                }
            }

            return updatedPatient;
        }


        public async void DeletePatient(int id)
        {
            var patientToRemove = Patients.FirstOrDefault(p => p.Id == id);

            if (patientToRemove != null)
            {
                Patients.Remove(patientToRemove);

                await new WebRequestHandler().Delete($"/Patient/{id}");
            }
        }

        public void UpdatePatientInsurancePolicy(int patientId, double newInsurancePolicy)
        {
            var patient = Patients.FirstOrDefault(p => p.Id == patientId);
            if (patient != null)
            {
                
                patient.InsurancePolicy = newInsurancePolicy;

            
                UpdateAppointmentsForPatient(patient);
            }
        }

        private void UpdateAppointmentsForPatient(PatientDTO updatedPatient)
        {
           
            var appointments = AppointmentServiceProxy.Current.Appointments
                .Where(a => a.PatientId == updatedPatient.Id);

            foreach (var appointment in appointments)
            {
               
                appointment.Patient = (Patient?)updatedPatient;

                
                appointment.Price = Math.Max(0, appointment.Treatment?.Price ?? 0 - updatedPatient.InsurancePolicy);
            }
        }
    }
}
