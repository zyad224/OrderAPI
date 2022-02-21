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
            try
            {
                return await _customerDal.AddCustomer(customerRequestDto);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<CustomerResponseDto> AuthenticateCustomer(CustomerRequestDto customerRequestDto)
        {
            try
            {
                return await _customerDal.AuthenticateCustomer(customerRequestDto);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<CustomerResponseDto> GetCustomerById(string customerId)
        {
            try
            {
                return await _customerDal.GetCustomerById(customerId);
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
