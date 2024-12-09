using Api.Clinic.Database;
using Library.Clinic.Models;

namespace Api.Clinic.Enterprise
{
    public class AppointmentEC
    {
        public IEnumerable<Appointment> Appointments
        {
            get
            {
                var apps = new MsSqlContext().GetAppointments();
                return apps;
            }
        }

        public void Delete(int Id)
        {
            new MsSqlContext().DeleteAppointment(Id);
            return;
        }

        public Appointment Update(Appointment appointment)
        {
            new MsSqlContext().UpdateAppointment(appointment);
            return appointment;
        }

        public Appointment Create(Appointment appointment)
        {
            new MsSqlContext().CreateAppointment(appointment);
            return appointment;
        }
    }
}
