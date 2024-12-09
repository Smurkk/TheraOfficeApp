using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Clinic.Models
{
    public class Treatment
    {
        public override string ToString()
        {
            return Display;
        }

        public string Display
        {
            get
            {
                return $"[{Id}] {Name} {Price}";
            }
        }
        public int Id { get; set; }
        private string? name;
        public string Name
        {
            get
            {
                return name ?? string.Empty;
            }

            set
            {
                name = value;
            }
        }

        private double price;
        public double Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
            }
        }


        public Treatment()
        {
            Name = string.Empty;
            Price = 0.0;
        }
    }
}
