using OrderAPI.DAL.Interfaces;
using OrderAPI.Dtos.CustomerDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerDal _customerDal;

        public CustomerService(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
        }

        public async Task<CustomerResponseDto> AddCustomer(CustomerRequestDto customerRequestDto)
        {
           
           return await _customerDal.AddCustomer(customerRequestDto);
          
        }

        public async Task<CustomerResponseDto> AuthenticateCustomer(CustomerRequestDto customerRequestDto)
        {
            
           return await _customerDal.AuthenticateCustomer(customerRequestDto);
       
        }

        public async Task<CustomerResponseDto> GetCustomerById(string customerId)
        {
            
           return await _customerDal.GetCustomerById(customerId);
                       
        }
    }
}
