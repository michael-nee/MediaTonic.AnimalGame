using MediaTonic.AnimalGame.API.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaTonic.AnimalGame.API.ViewModels
{
    public class CreateAnimalViewModel
    {
        [Required]
        public AnimalType AnimalType { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
