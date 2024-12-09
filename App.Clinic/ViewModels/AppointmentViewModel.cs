using Library.Clinic.DTO;
using Library.Clinic.Models;
using Library.Clinic.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace App.Clinic.ViewModels
{
    public class AppointmentViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Appointment? Model { get; set; }

        // Patients collection
        private ObservableCollection<PatientDTO> _patients;
        public ObservableCollection<PatientDTO> Patients
        {
            get => _patients;
            set
            {
                _patients = value;
                OnPropertyChanged(nameof(Patients));
            }
        }

        // Treatments collection
        private ObservableCollection<Treatment> _treatments;
        public ObservableCollection<Treatment> Treatments
        {
            get => _treatments;
            set
            {
                _treatments = value;
                OnPropertyChanged(nameof(Treatments));
            }
        }

        // Selected Patient
        private PatientDTO? _selectedPatient;
        public PatientDTO? SelectedPatient
        {
            get => _selectedPatient;
            set
            {
                if (_selectedPatient != value)
                {
                    _selectedPatient = value;
                    if (Model != null && value != null)
                    {
                        Model.Patient = new Patient
                        {
                            Id = value.Id,
                            Name = value.Name,
                            InsurancePolicy = value.InsurancePolicy
                        };
                        Model.PatientId = value.Id;
                    }
                    OnPropertyChanged(nameof(SelectedPatient));
                    OnPropertyChanged(nameof(PatientName));
                    OnPropertyChanged(nameof(Price)); // Update price if insurance changes
                }
            }
        }

        // Selected Treatment
        private Treatment? _selectedTreatment;
        public Treatment? SelectedTreatment
        {
            get => _selectedTreatment;
            set
            {
                if (_selectedTreatment != value)
                {
                    _selectedTreatment = value;
                    if (Model != null)
                    {
                        Model.Treatment = value;
                        OnPropertyChanged(nameof(SelectedTreatment));
                        OnPropertyChanged(nameof(TreatmentName));
                        OnPropertyChanged(nameof(Price)); // Update price if treatment changes
                    }
                }
            }
        }

        // Property for Treatment Name
        public string TreatmentName
        {
            get => Model?.Treatment?.Name ?? string.Empty;
            set
            {
                if (Model?.Treatment != null)
                {
                    Model.Treatment.Name = value;
                    OnPropertyChanged(nameof(TreatmentName));
                }
            }
        }

        // Property for displaying Patient Name
        public string PatientName
        {
            get
            {
                if (Model != null && Model.PatientId > 0)
                {
                    if (Model.Patient == null)
                    {
                        var patientDTO = PatientServiceProxy
                            .Current
                            .Patients
                            .FirstOrDefault(p => p.Id == Model.PatientId);
                        if (patientDTO != null)
                        {
                            Model.Patient = new Patient
                            {
                                Id = patientDTO.Id,
                                Name = patientDTO.Name,
                                InsurancePolicy = patientDTO.InsurancePolicy
                            };
                        }
                    }
                }

                return Model?.Patient?.Name ?? string.Empty;
            }
        }

        // Price Calculation Property
        public double Price
        {
            get
            {
                double treatmentPrice = Model?.Treatment?.Price ?? 0.0;
                double insurancePolicy = Model?.Patient?.InsurancePolicy ?? 0.0;
                return Math.Max(0, treatmentPrice - insurancePolicy);
            }
            set
            {
                if (Model != null)
                {
                    double insurancePolicy = Model.Patient?.InsurancePolicy ?? 0.0;
                    Model.Price = value + insurancePolicy;

                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        // Start Date for Appointment
        public DateTime StartDate
        {
            get => Model?.StartTime ?? DateTime.Today;
            set
            {
                if (Model != null)
                {
                    Model.StartTime = value.Date.Add(_startTime);
                    RefreshTime();
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        // End Date for Appointment
        public DateTime EndDate
        {
            get => Model?.EndTime ?? DateTime.Today;
            set
            {
                if (Model != null)
                {
                    Model.EndTime = value.Date.Add(_endTime);
                    RefreshTime();
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        // Start Time for Appointment
        private TimeSpan _startTime = TimeSpan.Zero;
        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                if (Model != null)
                {
                    Model.StartTime = StartDate.Date.Add(_startTime);
                }
                OnPropertyChanged(nameof(StartTime));
                RefreshTime();
            }
        }

        // End Time for Appointment
        private TimeSpan _endTime = TimeSpan.Zero;
        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                _endTime = value;
                if (Model != null)
                {
                    Model.EndTime = EndDate.Date.Add(_endTime);
                }
                OnPropertyChanged(nameof(EndTime));
                RefreshTime();
            }
        }

        // Minimum Start Date for Appointment
        public DateTime MinStartDate => DateTime.Today;

        public AppointmentViewModel()
        {
            Model = new Appointment();
            Patients = new ObservableCollection<PatientDTO>(PatientServiceProxy.Current.Patients);
            Treatments = new ObservableCollection<Treatment>(TreatmentServiceProxy.Current.Treatments);
        }

        public AppointmentViewModel(Appointment a)
        {
            Model = a;
            Patients = new ObservableCollection<PatientDTO>(PatientServiceProxy.Current.Patients);
            Treatments = new ObservableCollection<Treatment>(TreatmentServiceProxy.Current.Treatments);
        }

        public void RefreshTime()
        {
            if (Model != null)
            {
                if (Model.StartTime != null)
                {
                    DateTime startDateTime = StartDate.Date;
                    Model.StartTime = startDateTime.AddHours(StartTime.Hours).AddMinutes(StartTime.Minutes);
                }

                if (Model.EndTime != null)
                {
                    DateTime endDateTime = EndDate.Date;
                    Model.EndTime = endDateTime.AddHours(EndTime.Hours).AddMinutes(EndTime.Minutes);
                }

                OnPropertyChanged(nameof(StartTime));
                OnPropertyChanged(nameof(EndTime));
                OnPropertyChanged(nameof(StartDate));
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public void AddOrUpdate()
        {
            if (Model != null)
            {
                if (!IsValidAppointment())
                {
                    Console.WriteLine("Invalid appointment. Patient and treatment must be selected.");
                    return;
                }

                // Recalculate the price before saving
                Model.Price = Price;

                AppointmentServiceProxy.Current.AddOrUpdate(Model);

                // Refresh key properties after update
                OnPropertyChanged(nameof(StartDate));
                OnPropertyChanged(nameof(EndDate));
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(PatientName));
                OnPropertyChanged(nameof(TreatmentName));
            }
        }

        public bool IsValidAppointment()
        {
            if (Model == null)
            {
                return false;
            }

            if (Model.Patient == null || Model.PatientId <= 0)
            {
                return false;
            }

            if (Model.Treatment == null)
            {
                return false;
            }

            return true;
        }
    }
}