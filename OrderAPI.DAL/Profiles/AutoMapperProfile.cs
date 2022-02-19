using AutoMapper;
using OrderAPI.DAL.Entities;
using OrderAPI.Dtos.CustomerDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAPI.DAL.AutoMapperProfiles
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CustomerRequestDto, Customer>();

            CreateMap<Customer, CustomerResponseDto>();
        }
    }
}
