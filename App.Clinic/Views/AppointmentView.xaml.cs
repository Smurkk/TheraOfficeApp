using App.Clinic.ViewModels;
using Library.Clinic.Models;
using Library.Clinic.Services;

namespace App.Clinic.Views;

[QueryProperty(nameof(AppointmentId), "appointmentId")]
public partial class AppointmentView : ContentPage
{
    public int AppointmentId { get; set; }

    public AppointmentView()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (AppointmentId == 0)
        {
            // Create a new appointment with Id = 0
            BindingContext = new AppointmentViewModel(new Appointment { Id = 0 });
        }
        else
        {
            // Load the existing appointment for editing
            var existingAppointment = AppointmentServiceProxy.Current.Appointments.FirstOrDefault(a => a.Id == AppointmentId);
            if (existingAppointment != null)
            {
                BindingContext = new AppointmentViewModel(existingAppointment);
            }
            else
            {
                // If the appointment was not found, initialize a new appointment
                BindingContext = new AppointmentViewModel(new Appointment { Id = 0 });
            }
        }
    }

    private async void OkClicked(object sender, EventArgs e)
    {
        var viewModel = BindingContext as AppointmentViewModel;
        if (viewModel != null)
        {
            viewModel.AddOrUpdate();
        }

        // Navigate back to the appointments list after adding/updating
        await Shell.Current.GoToAsync("//Appointments");
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Appointments");
    }

    private void TimePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        (BindingContext as AppointmentViewModel)?.RefreshTime();
    }
}