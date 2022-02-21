using OrderAPI.Dtos.Dtos.OrderDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Services
{
    public interface IOrderService
    {
        decimal CalculateOrderBinWidth(OrderRequestDto orderRequestDto);
        Task<OrderResponseDto> PlaceOrder(OrderRequestDto orderRequestDto, decimal requiredBinWidth);
        Task<OrderResponseDto> OrderDetail(string orderId);

    }
}
