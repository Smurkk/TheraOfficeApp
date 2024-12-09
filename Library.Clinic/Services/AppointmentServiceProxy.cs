using Library.Clinic.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Clinic.Services
{
    public class AppointmentServiceProxy
    {
        private static object _lock = new object();
        private int LastKey
        {
            get
            {
                if (Appointments.Any())
                {
                    return Appointments.Select(x => x.Id).Max();
                }
                return 0;
            }
        }

        public List<Appointment> Appointments { get; private set; }
        private static AppointmentServiceProxy? instance;

        public static AppointmentServiceProxy Current
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new AppointmentServiceProxy();
                    }
                }
                return instance;
            }
        }

        private AppointmentServiceProxy()
        {
            // Initialize appointments list using treatments from TreatmentServiceProxy
            var defaultTreatment = TreatmentServiceProxy.Current.Treatments.FirstOrDefault(t => t.Id == 1);

            Appointments = new List<Appointment>
            {
                new Appointment
                {
                    Id = 1,
                    StartTime = new DateTime(2024, 10, 11, 14, 30, 0),
                    EndTime = new DateTime(2024, 10, 11, 15, 30, 0),
                    PatientId = 1,
                    Treatment = defaultTreatment,
                    Price = defaultTreatment?.Price ?? 0 // Set price to treatment's price
                }
            };

            // Ensure OriginalPrice is initialized for all appointments
            foreach (var appointment in Appointments)
            {
                if (appointment.OriginalPrice == 0)
                {
                    appointment.OriginalPrice = appointment.Price;
                }
            }
        }

        public void AddOrUpdate(Appointment a)
        {
            lock (_lock) // Use lock to make sure concurrent operations don't interfere
            {
                if (a.Id <= 0)
                {
                    // Adding a new appointment
                    a.Id = LastKey + 1;
                    Appointments.Add(a);
                }
                else
                {
                    // Update existing appointment if it already exists
                    var existingAppointment = Appointments.FirstOrDefault(app => app.Id == a.Id);
                    if (existingAppointment != null)
                    {
                        // Update the properties of the existing appointment
                        existingAppointment.StartTime = a.StartTime;
                        existingAppointment.EndTime = a.EndTime;
                        existingAppointment.PatientId = a.PatientId;
                        existingAppointment.Patient = a.Patient;
                        existingAppointment.Treatment = a.Treatment;
                        existingAppointment.Price = a.Price;

                        // Ensure OriginalPrice is set properly
                        if (existingAppointment.OriginalPrice == 0)
                        {
                            existingAppointment.OriginalPrice = existingAppointment.Price;
                        }
                    }
                    else
                    {
                        // If no matching appointment is found, add it as a new appointment
                        a.Id = LastKey + 1;
                        Appointments.Add(a);
                    }
                }
            }
        }




    }
}
