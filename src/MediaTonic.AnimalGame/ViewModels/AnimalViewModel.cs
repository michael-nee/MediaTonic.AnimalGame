using MediaTonic.AnimalGame.API.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaTonic.AnimalGame.API.ViewModels
{
    public class AnimalViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset LastPetted { get; set; }

        public DateTimeOffset LastFed { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public decimal CurrentHungriness { get; set; }

        public decimal CurrentHappiness { get; set; }

        public bool Alive { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AnimalType AnimalType { get; set; }
    }
}
