using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerDirectory.Data;
using CustomerDirectory.DTO;

namespace CustomerDirectory.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CustomerDto, Customer>()
                .ReverseMap();
        }
    }
}
