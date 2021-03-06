using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
using System.Linq;
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
                || orderRequestDto.ProductTypesQuantities == null)
                || orderRequestDto.ProductTypesQuantities.Count == 0)
            {
                throw new InvalidOrderRequestDtoException("Invalid OrderRequestDto Model");
            }

            if(requiredBinWidth <= 0)
            {
                throw new InvalidOrderBinWidthException("Invalid Order Bin Width");
            }


            var orderAlreadyExist = await _dbContext.Orders.Where(order => order.OrderId == orderRequestDto.OrderId).FirstOrDefaultAsync();

            if (orderAlreadyExist != null)
            {
                throw new OrderAlreadyExistException("Order Already Exist");
            }

            var customer = await _customerDal.GetCustomerById(orderRequestDto.CustomerId);

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

        public async Task<OrderResponseDto> OrderDetail(string orderId)
        {

            var order = await _dbContext.Orders.Where(order => order.OrderId == orderId)
                              .Include(order => order.ProductTypesQuantities)
                              .Include(order => order.Customer)
                              .FirstOrDefaultAsync();

            if(order == null)
            {
                throw new OrderNotExistException("Order Not Exist");
            }

            return _mapper.Map<OrderResponseDto>(order);

        }



    }
}
