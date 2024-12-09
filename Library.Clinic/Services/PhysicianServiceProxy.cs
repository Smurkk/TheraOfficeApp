using Library.Clinic.DTO;
using Library.Clinic.Models;
using Newtonsoft.Json;
using PP.Library.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Library.Clinic.Services
{
    public class PhysicianServiceProxy
    {
        private static object _lock = new object();
        public static PhysicianServiceProxy Current
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new PhysicianServiceProxy();
                    }
                }
                return instance;
            }
        }

        private static PhysicianServiceProxy? instance;
        private PhysicianServiceProxy()
        {
            instance = null;

            var physiciansData = new WebRequestHandler().Get("/Physician").Result;

            Physicians = JsonConvert.DeserializeObject<List<PhysicianDTO>>(physiciansData) ?? new List<PhysicianDTO>();

        }
        public int LastKeyPhysician
        {
            get
            {
                if (Physicians.Any())
                {
                    return Physicians.Select(x => x.Id).Max();
                }
                return 0;
            }
        }

        private List<PhysicianDTO> physicians;
        public List<PhysicianDTO> Physicians
        {
            get
            {
                return physicians;
            }

            private set
            {
                if (physicians != value)
                {
                    physicians = value;
                }
            }
        }

        public async Task<List<PhysicianDTO>> Search(string query)
        {

            var physiciansPayload = await new WebRequestHandler().Post("/Physician/Search", new Query(query));

            Physicians = JsonConvert.DeserializeObject<List<PhysicianDTO>>(physiciansPayload) ?? new List<PhysicianDTO>();

            return Physicians;

        }

        public async Task<PhysicianDTO?> AddOrUpdatePhysician(PhysicianDTO physician)
        {

            var payload = await new WebRequestHandler().Post("/Physician", physician);
            var newPhysician = JsonConvert.DeserializeObject<PhysicianDTO>(payload);
            if (newPhysician != null && newPhysician.Id > 0 && physician.Id == 0)
            {
            
                Physicians.Add(newPhysician);
            }
            else if (newPhysician != null && physician != null && physician.Id > 0 && physician.Id == newPhysician.Id)
            {
              
                var currentPhysician = Physicians.FirstOrDefault(p => p.Id == newPhysician.Id);
                var index = Physicians.Count;
                if (currentPhysician != null)
                {
                    index = Physicians.IndexOf(currentPhysician);
                    Physicians.RemoveAt(index);
                }

                Physicians.Insert(index, newPhysician);

            }

            return newPhysician;
        }



        public async void DeletePhysician(int id)
        {
            var physicianToRemove = Physicians.FirstOrDefault(p => p.Id == id);

            if (physicianToRemove != null)
            {
                Physicians.Remove(physicianToRemove);

                await new WebRequestHandler().Delete($"/Physician/{id}");
            }
        }


    }
}
