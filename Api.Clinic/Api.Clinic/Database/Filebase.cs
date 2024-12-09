using Library.Clinic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ToDoApplication.Persistence
{
    public class Filebase
    {
        private string _root;
        private string _patientRoot;
        private string _physicianRoot;
        private Filebase _instance;


        public Filebase Current
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Filebase();
                }

                return _instance;
            }
        }

        private Filebase()
        {
            _root = @"C:\temp";
            _patientRoot = $"{_root}\\Patients";
            _physicianRoot = $"{_root}\\Physicians";
        }
        public int LastKey
        {
            get
            {
                if (Patients.Any())
                {
                    return Patients.Select(x => x.Id).Max();
                }
                return 0;
            }
        }
        public Patient AddOrUpdate(Patient patient)
        {
            
            if(patient.Id <= 0)
            {
                patient.Id = LastKey + 1;
            }

            //go to the right place]
            string path = $"{_patientRoot}\\{patient.Id}.json";


            //if the item has been previously persisted
            if(File.Exists(path))
            {
                //blow it up
                File.Delete(path);
            }

            //write the file
            File.WriteAllText(path, JsonConvert.SerializeObject(patient));

            //return the item, which now has an id
            return patient;
        }
        

       
        public List<Patient> Patients
        {
            get
            {
                var root = new DirectoryInfo(_patientRoot);
                var _patients = new List<Patient>();
                foreach(var patientFile in root.GetFiles())
                {
                    var patient = JsonConvert.DeserializeObject<Patient>(File.ReadAllText(patientFile.FullName));
                    if(patient != null)
                    {
                        _patients.Add(patient);
                    }
                    
                }
                return _patients;
            }
        }

        public List<Physician> Physicians
        {
            get
            {
                var root = new DirectoryInfo(_physicianRoot);
                var _physicians = new List<Physician>();
                foreach (var physicianFile in root.GetFiles())
                {
                    var physician = JsonConvert.DeserializeObject<Physician>(File.ReadAllText(physicianFile.FullName));
                    if (physician != null)
                    {
                        _physicians.Add(physician);
                    }

                }
                return _physicians;
            }
        }
        public bool Delete(string type, string id)
        {
            
            return true;
        }
    }



}
