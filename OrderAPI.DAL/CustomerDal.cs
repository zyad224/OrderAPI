using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderAPI.DAL.Data;
using OrderAPI.DAL.Entities;
using OrderAPI.DAL.Interfaces;
using OrderAPI.Dtos.CustomerDtos;
using OrderAPI.Utilities.CustomExceptions;
using OrderAPI.Utilities.CustomExceptions.CustomerExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.DAL
{
    public class CustomerDal : ICustomerDal
    {
        private readonly DbApiContext _dbContext;
        private readonly IMapper _mapper;

        public CustomerDal(DbApiContext dbApiContext, IMapper mapper)
        {
            _dbContext = dbApiContext;
            _mapper = mapper;
        }
        public async Task<CustomerResponseDto> AddCustomer(CustomerRequestDto customerRequestDto)
        {

            if ((string.IsNullOrEmpty(customerRequestDto.UserName) || 
                string.IsNullOrEmpty(customerRequestDto.Password)))
            {
                throw new InvalidCustomerRequestDtoException("Invalid CustomerRequestDto Model");
            }
                

            var customerAlreadyExist = await _dbContext.Customers
                .Where(customer => customer.UserName == customerRequestDto.UserName)
                .FirstOrDefaultAsync();

            if(customerAlreadyExist != null)
            {
                throw new CustomerAlreadyExistException("Customer Already Exists");
            }

            var customer = _mapper.Map<Customer>(customerRequestDto);
            await _dbContext.Customers.AddAsync(customer);
            
            if(await _dbContext.SaveChangesAsync()>0)
            {
                return _mapper.Map<CustomerResponseDto>(customer);
            }
            else
            {
                throw new DatabaseSaveException("Database Failed To Save");
            }


        }

        public async Task<CustomerResponseDto> AuthenticateCustomer(CustomerRequestDto customerRequestDto)
        {
            if ((string.IsNullOrEmpty(customerRequestDto.UserName) ||
              string.IsNullOrEmpty(customerRequestDto.Password)))
            {
                throw new InvalidCustomerRequestDtoException("Invalid CustomerRequestDto Model");
            }

            var customer = await _dbContext.Customers
                .Where(customer => customer.UserName == customerRequestDto.UserName && customer.Password == customerRequestDto.Password)
                .FirstOrDefaultAsync();

            if(customer != null)
            {
                return _mapper.Map<CustomerResponseDto>(customer);

            }

            throw new CustomerNotAuthenticated("Customer Not Authenticated");

        }
    }
}
