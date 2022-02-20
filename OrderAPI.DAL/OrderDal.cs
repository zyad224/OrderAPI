using AutoMapper;
using Microsoft.Extensions.Configuration;
using OrderAPI.DAL.Data;
using OrderAPI.DAL.Entities;
using OrderAPI.DAL.Interfaces;
using OrderAPI.Dtos.Dtos.OrderDtos;
using OrderAPI.Utilities;
using OrderAPI.Utilities.CustomExceptions;
using OrderAPI.Utilities.CustomExceptions.CustomerExceptions;
using OrderAPI.Utilities.CustomExceptions.OrderExceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.DAL
{
    public class OrderDal : IOrderDal
    {
        private readonly DbApiContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICustomerDal _customerDal;
        public OrderDal(DbApiContext dbApiContext, IMapper mapper, ICustomerDal customerDal)
        {
            _dbContext = dbApiContext;
            _mapper = mapper;
            _customerDal = customerDal;
        }

       
        public async Task<OrderResponseDto> PlaceOrder(OrderRequestDto orderRequestDto, decimal requiredBinWidth)
        {
            if ((string.IsNullOrEmpty(orderRequestDto.OrderId) 
                || string.IsNullOrEmpty(orderRequestDto.CustomerId)
                || requiredBinWidth == 0
                || orderRequestDto.ProductTypesQuantities == null)
                || orderRequestDto.ProductTypesQuantities.Count == 0)
            {
                throw new InvalidOrderRequestDtoException("Invalid OrderRequestDto Model");
            }

            try
            {
                var customer = await _customerDal.GetCustomerById(orderRequestDto.CustomerId);

            }
            catch(CustomerNotExistException)
            {
                throw new CustomerNotExistException("Customer does not Exists");

            }

            var order = _mapper.Map<Order>(orderRequestDto);
            order.RequiredBinWidth = requiredBinWidth;

            await _dbContext.Orders.AddAsync(order);

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<OrderResponseDto>(order);
            }
            else
            {
                throw new DatabaseSaveException("Database Failed To Save");
            }
        }

      
    }
}
