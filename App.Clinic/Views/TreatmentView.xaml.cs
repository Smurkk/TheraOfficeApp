using App.Clinic.ViewModels;
using Library.Clinic.Models;
using Library.Clinic.Services;


namespace App.Clinic.Views;

[QueryProperty(nameof(TreatmentID), "treatmentId")]
public partial class TreatmentView : ContentPage
{
    public TreatmentView()
    {
        InitializeComponent();

    }
    public int TreatmentID { get; set; }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Treatments");
    }

    private void AddClicked(object sender, EventArgs e)
    {
        (BindingContext as TreatmentViewModel)?.ExecuteAdd();
    }

    private void TreatmentView_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        //TODO: this really needs to be in a view model
        if (TreatmentID > 0)
        {
            var model = TreatmentServiceProxy.Current
                .Treatments.FirstOrDefault(p => p.Id == TreatmentID);
            if (model != null)
            {
                BindingContext = new TreatmentViewModel(model);
            }
            else
            {
                BindingContext = new TreatmentViewModel();
            }

        }
        else
        {
            BindingContext = new TreatmentViewModel();
        }

    }
}