using Library.Clinic.Models;
using Library.Clinic.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace App.Clinic.ViewModels;

public class AppointmentManagementViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private AppointmentServiceProxy _proxy = AppointmentServiceProxy.Current;

    private ObservableCollection<AppointmentViewModel> _appointments;
    public ObservableCollection<AppointmentViewModel> Appointments
    {
        get => _appointments;
        set
        {
            _appointments = value;
            OnPropertyChanged(nameof(Appointments));
        }
    }

    public AppointmentViewModel? SelectedAppointment { get; set; }

    public AppointmentManagementViewModel()
    {
        // Initialize the Appointments collection
        _appointments = new ObservableCollection<AppointmentViewModel>();
        RefreshAppointments();
    }

    public void RefreshAppointments()
    {
        // Clear existing appointments and add updated appointments from proxy
        _appointments.Clear();
        foreach (var appointment in _proxy.Appointments)
        {
            _appointments.Add(new AppointmentViewModel(appointment));
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}