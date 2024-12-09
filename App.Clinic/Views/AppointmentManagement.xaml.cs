using App.Clinic.ViewModels;
using Library.Clinic.Models;
using Library.Clinic.Services;

namespace App.Clinic.Views;

public partial class AppointmentManagement : ContentPage
{
    private AppointmentManagementViewModel ViewModel;

    public AppointmentManagement()
    {
        InitializeComponent();
        ViewModel = new AppointmentManagementViewModel();
        BindingContext = ViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Refresh the appointments list when the page appears
        ViewModel.RefreshAppointments();
    }

    private async void AddClicked(object sender, EventArgs e)
    {
        // Navigate to the appointment view to add a new appointment with appointmentId set to 0
        await Shell.Current.GoToAsync("//AppointmentDetails?appointmentId=0");
    }

    private async void CancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    public void RefreshAppointments()
    {
        // Refresh appointments when explicitly requested
        ViewModel.RefreshAppointments();
    }

    private async void DetailsClicked(object sender, EventArgs e)
    {
        // Get the selected appointment Id to navigate to the details view
        var button = sender as Button;
        var appointmentViewModel = button?.BindingContext as AppointmentViewModel;

        if (appointmentViewModel != null)
        {
            int appointmentId = appointmentViewModel.Model.Id;
            await Shell.Current.GoToAsync($"//AppointmentDetailsView?appointmentId={appointmentId}");
        }
    }
}
