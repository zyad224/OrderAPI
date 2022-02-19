using OrderAPI.Dtos.CustomerDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.DAL.Interfaces
{
    public interface ICustomerDal
    {
        public Task<CustomerResponseDto> AddCustomer(CustomerRequestDto customerRequestDto);
        public Task<CustomerResponseDto> AuthenticateCustomer(CustomerRequestDto customerRequestDto);
    }
}
