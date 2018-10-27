using AutoMapper;
using MediaTonic.AnimalGame.API.Models;
using MediaTonic.AnimalGame.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaTonic.AnimalGame.API
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.Animals, source => source.MapFrom(src => src.UserAnimals));

            CreateMap<Animal, AnimalViewModel>()
                .ForMember(dest => dest.CurrentHappiness, source => source.MapFrom(src => src.CurrentHappiness()))
                .ForMember(dest => dest.CurrentHungriness, source => source.MapFrom(src => src.CurrentHungriness()));
        }
    }
}
