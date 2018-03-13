using System;

namespace App.DTO
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
