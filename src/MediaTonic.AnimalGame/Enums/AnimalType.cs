using System.ComponentModel.DataAnnotations;

namespace MediaTonic.AnimalGame.API.Enums
{
    public enum AnimalType
    {
        [Display(Name = "Dog")]
        Dog,
        [Display(Name = "Cat")]
        Cat,
        [Display(Name = "Rabbit")]
        Rabbit,
        [Display(Name = "Guinea Pig")]
        GuineaPig,
        [Display(Name = "Tortoise")]
        Tortoise,
        [Display(Name = "Horse")]
        Horse
    }
}
