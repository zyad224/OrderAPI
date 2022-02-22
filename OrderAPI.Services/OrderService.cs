using Microsoft.Extensions.Configuration;
using OrderAPI.DAL.Interfaces;
using OrderAPI.Dtos.Dtos.OrderDtos;
using OrderAPI.Utilities;
using OrderAPI.Utilities.CustomExceptions.OrderExceptions;
using OrderAPI.Utilities.Extenstions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderDal _orderDal;
        private readonly IConfiguration _configuration;


        public OrderService(IOrderDal orderDal, IConfiguration configuration)
        {
            _orderDal = orderDal;
            _configuration = configuration;
        }

        public decimal CalculateOrderBinWidth(OrderRequestDto orderRequestDto)
        {
            if ((string.IsNullOrEmpty(orderRequestDto.OrderId)
                || string.IsNullOrEmpty(orderRequestDto.CustomerId)
                || orderRequestDto.ProductTypesQuantities == null)
                || orderRequestDto.ProductTypesQuantities.Count == 0)
            {
                throw new InvalidOrderRequestDtoException("Invalid OrderRequestDto Model");
            }

            Dictionary<string,string> productBinWidthDict = _configuration.GetSection("ProductWidthConfig").GetChildren().ToDictionary(x=> x.Key, x=> x.Value);
            var productTypesQuantities = orderRequestDto.ProductTypesQuantities;
            decimal orderBinWidth = 0;

            foreach (var product in productTypesQuantities)
            {
               
              orderBinWidth += CalculateBinWidthNeededForSpecificProduct(product, productBinWidthDict);
                
            }

            return orderBinWidth;
        }

        private decimal CalculateBinWidthNeededForSpecificProduct(ProductTypeQuantityDto product, Dictionary<string,string> productBinWidthDict)
        {
            decimal binWidthNeeded = 0;
          
            productBinWidthDict.TryGetValue(product.ProductType.GetStringFromProductTypeDtoEnum(), out string binWidthPerProduct);

            if (product.ProductType == ProductTypeDto.mug)
              {
                 var mugStacksNedded = Math.Ceiling(Convert.ToDecimal(product.Quantity) / Convert.ToDecimal(4));
                 binWidthNeeded = mugStacksNedded * decimal.Parse(binWidthPerProduct);
              }
            else
              {
                 binWidthNeeded = product.Quantity * decimal.Parse(binWidthPerProduct);

              }

            return binWidthNeeded;
        }

        public async Task<OrderResponseDto> PlaceOrder(OrderRequestDto orderRequestDto, decimal requiredBinWidth)
        {
                    
          return await _orderDal.PlaceOrder(orderRequestDto, requiredBinWidth);
            
        }

        public async Task<OrderResponseDto> OrderDetail(string orderId)
        {
           
          return await _orderDal.OrderDetail(orderId);
                     
        }

    }
}
