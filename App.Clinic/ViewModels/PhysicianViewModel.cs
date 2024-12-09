using Library.Clinic.DTO;
using Library.Clinic.Models;
using Library.Clinic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace App.Clinic.ViewModels
{
    public class PhysicianViewModel
    {
        public PhysicianDTO? Model { get; set; }
        public ICommand? DeleteCommand { get; set; }
        public ICommand? EditCommand { get; set; }

        public ICommand? InsuranceCommand { get; set; }
        public int Id
        {
            get
            {
                if(Model == null)
                {
                    return -1;
                }

                return Model.Id;
            }

            set
            {
                if(Model != null && Model.Id != value) {
                    Model.Id = value;
                }
            }
        }

        public string Name
        {
            get => Model?.Name ?? string.Empty;
            set
            {
                if(Model != null)
                {
                    Model.Name = value;
                }
            }
        }

        public void SetupCommands()
        {
            DeleteCommand = new Command(DoDelete);
            EditCommand = new Command((p) => DoEdit(p as PhysicianViewModel));
        }

        private void DoDelete()
        {
            if (Id > 0)
            {
                PhysicianServiceProxy.Current.DeletePhysician(Id);
                Shell.Current.GoToAsync("//Physicians");
            }
        }

        private void DoEdit(PhysicianViewModel? pvm)
        {
            if (pvm == null)
            {
                return;
            }
            var selectedPhysicianId = pvm?.Id ?? 0;
            Shell.Current.GoToAsync($"//PhysicianDetails?physicianId={selectedPhysicianId}");
        }


        public PhysicianViewModel()
        {
            Model = new PhysicianDTO();
            SetupCommands();
        }

        public PhysicianViewModel(PhysicianDTO? _model)
        {
            Model = _model;
            SetupCommands();
        }

        public async void ExecuteAdd()
        {
            if (Model != null)
            {
                await PhysicianServiceProxy
                .Current
                .AddOrUpdatePhysician(Model);
            }

            await Shell.Current.GoToAsync("//Physicians");
        }


    }
}
