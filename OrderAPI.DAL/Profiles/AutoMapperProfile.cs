using AutoMapper;
using OrderAPI.DAL.Entities;
using OrderAPI.Dtos.CustomerDtos;
using OrderAPI.Dtos.Dtos.OrderDtos;
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

            CreateMap<ProductTypeDto, ProductType>();
            CreateMap<ProductTypeQuantityDto, ProductTypeQuantity>();
            CreateMap<OrderRequestDto, Order>();

            CreateMap<ProductType, ProductTypeDto>();
            CreateMap<ProductTypeQuantity, ProductTypeQuantityDto>();
            CreateMap<Order, OrderResponseDto>();
        }
    }
}
