using OrderAPI.Dtos.CustomerDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Services
{
    public interface ICustomerService
    {
        public Task<CustomerResponseDto> AddCustomer(CustomerRequestDto customerRequestDto);
        public Task<CustomerResponseDto> AuthenticateCustomer(CustomerRequestDto customerRequestDto);
        public Task<CustomerResponseDto> GetCustomerById(string customerId);
    }
}
