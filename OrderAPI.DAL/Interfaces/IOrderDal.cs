using OrderAPI.Dtos.Dtos.OrderDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.DAL.Interfaces
{
    public interface IOrderDal
    {
        public Task<OrderResponseDto> PlaceOrder(OrderRequestDto orderRequestDto,decimal requiredBinWidth);

        public Task<OrderResponseDto> OrderDetail(string orderId);

    }
}
