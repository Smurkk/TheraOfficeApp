using Library.Clinic.DTO;
using Library.Clinic.Services;
using System.ComponentModel;
using System.Windows.Input; // Required for INotifyPropertyChanged

namespace App.Clinic.ViewModels
{
    public class PatientViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PatientDTO? Model { get; set; }
        public ICommand? DeleteCommand { get; set; }
        public ICommand? EditCommand { get; set; }
        public ICommand? InsuranceCommand { get; set; }

        public int Id
        {
            get => Model?.Id ?? -1;
            set
            {
                if (Model != null && Model.Id != value)
                {
                    Model.Id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string Name
        {
            get => Model?.Name ?? string.Empty;
            set
            {
                if (Model != null && Model.Name != value)
                {
                    Model.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public double InsurancePolicy
        {
            get => Model?.InsurancePolicy ?? 0.0;
            set
            {
                if (Model != null && Model.InsurancePolicy != value)
                {
                    Model.InsurancePolicy = value;
                    OnPropertyChanged(nameof(InsurancePolicy));
                }
            }
        }

        public void SetupCommands()
        {
            DeleteCommand = new Command(DoDelete);
            EditCommand = new Command((p) => DoEdit(p as PatientViewModel));
            InsuranceCommand = new Command((p) => DoInsurance(p as PatientViewModel));
        }

        private void DoDelete()
        {
            if (Id > 0)
            {
                PatientServiceProxy.Current.DeletePatient(Id);
                Shell.Current.GoToAsync("//Patients");
            }
        }

        private void DoEdit(PatientViewModel? pvm)
        {
            if (pvm == null) return;

            var selectedPatientId = pvm?.Id ?? 0;
            Shell.Current.GoToAsync($"//PatientDetails?patientId={selectedPatientId}");
        }

        private void DoInsurance(PatientViewModel? pvm)
        {
            if (pvm == null) return;

            var selectedPatientId = pvm?.Id ?? 0;
            Shell.Current.GoToAsync($"//InsuranceDetails?patientId={selectedPatientId}");
        }

        public PatientViewModel()
        {
            Model = new PatientDTO();
            SetupCommands();
        }

        public PatientViewModel(PatientDTO? _model)
        {
            Model = _model;
            SetupCommands();
        }

        public async void ExecuteAdd()
        {
            if (Model != null)
            {
                await PatientServiceProxy.Current.AddOrUpdatePatient(Model);

            }

            await Shell.Current.GoToAsync("//Patients");
        }

    }
}
