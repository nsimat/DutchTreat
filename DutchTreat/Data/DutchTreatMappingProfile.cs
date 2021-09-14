using AutoMapper;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchTreatMappingProfile : Profile
    {
        public DutchTreatMappingProfile()
        {
            CreateMap<Order, OrderViewModel>().ReverseMap();
            CreateMap<OrderItem, OrderItemViewModel>().ReverseMap();
        }
    }
}
