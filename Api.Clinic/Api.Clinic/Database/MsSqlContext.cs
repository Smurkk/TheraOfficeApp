using Library.Clinic.DTO;
using Library.Clinic.Models;
using Microsoft.Data.SqlClient;

namespace Api.Clinic.Database
{
    public class MsSqlContext
    {
        private string connectionString = @"Server=CHRISTOPHER\CALSTON;Database=Clinic;Trusted_Connection=True;TrustServerCertificate=True;";

        public MsSqlContext() { }

        public List<Appointment> GetAppointments()
        {
            var returnVal = new List<Appointment>();
            using (var conn = new SqlConnection(connectionString))
            {
                var str = "select * from Appointments";
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = str;

                    try
                    {
                        conn.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            var newApp = new Appointment
                            {
                                Id = (int)reader["Id"],
                                PatientId = (int)reader["PatientId"],
                                StartTime = DateTime.Parse(reader["Date"].ToString() ?? "1901-01-01")
                            };
                            returnVal.Add(newApp);

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                }
            }
            return returnVal;
        }
    
        public void DeleteAppointment(int Id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var str = "Appointments.[Delete]";
                using (var cmd = new SqlCommand(str, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var param = new SqlParameter("@appointmentId", Id);
                    param.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(param);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                }
            }
        }

        public void UpdateAppointment(Appointment app)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var str = "Appointments.[Update]";
                using (var cmd = new SqlCommand(str, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@date", app.StartTime));
                    cmd.Parameters.Add(new SqlParameter("@physicianId", 3));
                    cmd.Parameters.Add(new SqlParameter("@patientId", app.PatientId));
                    cmd.Parameters.Add(new SqlParameter("@appointmentId", app.Id));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                }
            }
        }

        public Appointment CreateAppointment(Appointment app)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var str = "Appointments.[Create]";
                using (var cmd = new SqlCommand(str, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@date", app.StartTime));
                    cmd.Parameters.Add(new SqlParameter("@physicianId", 3));
                    cmd.Parameters.Add(new SqlParameter("@patientId", app.PatientId));
                    var param = new SqlParameter("@appointmentId", app.Id);
                    param.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(param);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    app.Id = (int)param.Value;
                    conn.Close();

                }
            }

            return app;
        }





        public List<PatientDTO> GetPatients()
        {
            var returnVal = new List<PatientDTO>();
            using (var conn = new SqlConnection(connectionString))
            {
                var str = "select * from Patients";
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = str;

                    try
                    {
                        conn.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            var newPatient = new PatientDTO
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"]
                            };
                            returnVal.Add(newPatient);

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                }
            }
            return returnVal;
        }

        public void DeletePatient(int Id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var str = "Patients.[Delete]";
                using (var cmd = new SqlCommand(str, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var param = new SqlParameter("@id", Id);
                    cmd.Parameters.Add(param);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                }
            }
        }

        public void UpdatePatient(PatientDTO patient)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var str = "Patients.[Update]";
                using (var cmd = new SqlCommand(str, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", patient.Id));
                    cmd.Parameters.Add(new SqlParameter("@Name", patient.Name));
                    cmd.Parameters.Add(new SqlParameter("@InsurancePolicy", patient.InsurancePolicy));
              

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                }
            }
        }

        public PatientDTO CreatePatient(PatientDTO patient)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var str = "Patients.[Create]";
                using (var cmd = new SqlCommand(str, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Name", patient.Name));
                    cmd.Parameters.Add(new SqlParameter("@InsurancePolicy", patient.InsurancePolicy));
                    var param = new SqlParameter("@ID", patient.Id);
                    param.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(param);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    patient.Id = (int)param.Value;
                    conn.Close();

                }
            }

            return patient;
        }
    }








}
