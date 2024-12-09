using App.Clinic.ViewModels;
using Library.Clinic.Models;
using Library.Clinic.Services;


namespace App.Clinic.Views;

[QueryProperty(nameof(PatientId), "patientId")]
[QueryProperty(nameof(InsurancePolicy), "InsurancePolicy")]
public partial class InsuranceView : ContentPage
{
    public InsuranceView()
    {
        InitializeComponent();
        this.NavigatedTo += InsuranceView_NavigatedTo;
    }

    public int PatientId { get; set; }
    public int InsurancePolicy { get; set; }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Patients");
    }

    private void AddClicked(object sender, EventArgs e)
    {
        var patientViewModel = BindingContext as PatientViewModel;
        if (patientViewModel != null)
        {
            // Save the updated insurance policy
            patientViewModel.ExecuteAdd();

            // Update prices for all appointments of this patient
            var appointments = AppointmentServiceProxy.Current
                .Appointments
                .Where(a => a.PatientId == patientViewModel.Id)
                .ToList();

            foreach (var appointment in appointments)
            {
                var insurancePolicy = patientViewModel.InsurancePolicy;

                // Adjust the price based on the original price
                appointment.Price = Math.Max(0, appointment.OriginalPrice - insurancePolicy);

                // Save the updated appointment back to the service
                AppointmentServiceProxy.Current.AddOrUpdate(appointment);
            }

            // Navigate back to Patients tab
            Shell.Current.GoToAsync("//Patients");
        }
    }



    private void InsuranceView_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        //TODO: this really needs to be in a view model
        if (PatientId > 0)
        {
            var model = PatientServiceProxy.Current
                .Patients.FirstOrDefault(p => p.Id == PatientId);
            if (model != null)
            {
                BindingContext = new PatientViewModel(model);
            }
            else
            {
                BindingContext = new PatientViewModel();
            }

        }
        else
        {
            BindingContext = new PatientViewModel();
        }

    }
}