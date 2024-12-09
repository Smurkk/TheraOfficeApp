using Library.Clinic.Models;
using System.ComponentModel;

namespace App.Clinic.ViewModels;

public class AppointmentDetailsViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private Appointment _appointment;

    public string PatientName => _appointment.Patient?.Name ?? "Unknown";
    public double InsurancePlanPrice => _appointment.Patient?.InsurancePolicy ?? 0;
    public string TreatmentName => _appointment.Treatment?.Name ?? "Unknown";
    public double TreatmentPrice => _appointment.Treatment?.Price ?? 0;
    public double TotalPriceWithoutInsurance => _appointment.Treatment?.Price ?? 0;
    public double TotalPriceWithInsurance => Math.Max(0, (TotalPriceWithoutInsurance - InsurancePlanPrice));

    public AppointmentDetailsViewModel(Appointment appointment)
    {
        _appointment = appointment;
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
