using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shipping.DTO
{
    public class Leg
    {
        public string Voyage { get; set; }

        public string LoadLocation { get; set; }

        public string UnloadLocation { get; set; }

        public DateTime LoadTime { get; set; }

        public DateTime UnloadTime { get; set; }


    }
}
