using App.Clinic.ViewModels;
using Library.Clinic.Models;
using Library.Clinic.Services;

namespace App.Clinic.Views;

[QueryProperty(nameof(AppointmentId), "appointmentId")]
public partial class AppointmentDetailsView : ContentPage
{
    public int AppointmentId { get; set; }

    public AppointmentDetailsView()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs e)
    {
        base.OnNavigatedTo(e);
        if (AppointmentId > 0)
        {
            var model = AppointmentServiceProxy.Current.Appointments.FirstOrDefault(a => a.Id == AppointmentId);
            if (model != null)
            {
                BindingContext = new AppointmentDetailsViewModel(model);
            }
        }
    }

    private async void BackClicked(object sender, EventArgs e)
    {
    
        await Shell.Current.GoToAsync("//Appointments");
    }
}
