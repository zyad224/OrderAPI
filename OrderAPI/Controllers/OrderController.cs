using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Dtos.Dtos.OrderDtos;
using OrderAPI.Services;
using OrderAPI.Utilities.CustomExceptions;
using OrderAPI.Utilities.CustomExceptions.CustomerExceptions;
using OrderAPI.Utilities.CustomExceptions.OrderExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderAPI.Controllers
{
    [Route("api/Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {          
            _orderService = orderService;
        }

        [HttpPost]
        [Authorize]
        [Route("PlaceOrder")]
        public async Task<ActionResult<OrderResponseDto>> PlaceOrder([FromBody] OrderRequestDto orderRequestDto)
        {

            try
            {
                var orderBinWidth = _orderService.CalculateOrderBinWidth(orderRequestDto);
                var orderResponseDto = await _orderService.PlaceOrder(orderRequestDto, orderBinWidth);
                return StatusCode(200, orderResponseDto);

            }
            catch (InvalidOrderRequestDtoException)
            {
                return StatusCode(400, "Invalid OrderRequestDto Model");

            }
            catch (InvalidOrderBinWidthException)
            {
                return StatusCode(400, "Invalid OrderBin Width");

            }
            catch (CustomerNotExistException)
            {
                return StatusCode(400, "Customer Does not Exist");

            }
            catch (OrderAlreadyExistException)
            {
                return StatusCode(400, "Order Already Exist");

            }
            catch (ArgumentNullException)
            {
                return StatusCode(500, "Internal Server Error");

            }
            catch (DatabaseSaveException)
            {
                return StatusCode(500, "Internal Server Error");

            }
         

        }

        [HttpGet]
        [Route("OrderDetail")]
        public async Task<ActionResult<OrderResponseDto>> OrderDetail([FromQuery] string orderId)
        {

            try
            {
                var orderResponseDto = await _orderService.OrderDetail(orderId);
                return StatusCode(200, orderResponseDto);

            }
            catch (OrderNotExistException)
            {
                return StatusCode(400, "Order Not Exist");

            }
        }
    }
}
