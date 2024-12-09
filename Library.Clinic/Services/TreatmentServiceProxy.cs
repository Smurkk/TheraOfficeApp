using Library.Clinic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Clinic.Services
{
    public class TreatmentServiceProxy
    {
        private static object _lock = new object();
        public static TreatmentServiceProxy Current
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new TreatmentServiceProxy();
                    }
                }
                return instance;
            }
        }

        private static TreatmentServiceProxy? instance;
        private TreatmentServiceProxy()
        {
            instance = null;


            Treatments = new List<Treatment>
            {
                new Treatment{Id = 1, Name = "General Care", Price = 500.0}
                , new Treatment{Id = 2, Name = "X-Ray", Price = 349.99}
                , new Treatment{Id = 3, Name = "Knee Surgery", Price = 2345.29}

            };
        }
        public int LastKey
        {
            get
            {
                if (Treatments.Any())
                {
                    return Treatments.Select(x => x.Id).Max();
                }
                return 0;
            }
        }

        private List<Treatment> treatments;
        public List<Treatment> Treatments
        {
            get
            {
                return treatments;
            }

            private set
            {
                if (treatments != value)
                {
                    treatments = value;
                }
            }
        }

        public void AddOrUpdateTreatment(Treatment treatment)
        {
            bool isAdd = false;
            if (treatment.Id <= 0)
            {
                treatment.Id = LastKey + 1;
                isAdd = true;
            }

            if (isAdd)
            {
                Treatments.Add(treatment);
            }

        }

        public void DeleteTreatment(int id)
        {
            var treatmentToRemove = Treatments.FirstOrDefault(p => p.Id == id);

            if (treatmentToRemove != null)
            {
                Treatments.Remove(treatmentToRemove);
            }
        }
    }
}
