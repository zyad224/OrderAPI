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

        /// <summary>
        /// PlaceOrder for Authorized Customers (JWT).
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Post api/Order/PlaceOrder
        ///     {        
        ///           "OrderId": "3",
        ///           "CustomerId": "4b2ed032-ffff-4699-93eb-0e4aa2f5e211",
        ///           "ProductTypesQuantities": [
        ///              {
        ///                "ProductType": 1,
        ///                "Quantity": 1
        ///              },
        ///              {
        ///              "ProductType": 2,
        ///              "Quantity": 1
        ///              }
        ///          ]
        ///     }
        /// </remarks>
        /// <returns> New OrderResponseDto</returns>
        /// /// <response code="200"> New OrderResponseDto</response>
        /// <response code="400">Invalid OrderRequestDto Model</response>
        // POST: api/Order/PlaceOrder
        [HttpPost]
        [Authorize]
        [Route("PlaceOrder")]
        public async Task<ActionResult<OrderResponseDto>> PlaceOrder([FromBody] OrderRequestDto orderRequestDto)
        {

            if (!ModelState.IsValid)
                return StatusCode(400, "Invalid OrderRequestDto");

            var orderBinWidth = _orderService.CalculateOrderBinWidth(orderRequestDto);
            var orderResponseDto = await _orderService.PlaceOrder(orderRequestDto, orderBinWidth);
            return StatusCode(200, orderResponseDto);
       
        }

        /// <summary>
        /// Get Order Detail.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/Order/OrderDetail
        ///     {        
        ///       "orderId": "10"
        ///     }
        /// </remarks>
        /// <returns> New OrderResponseDto</returns>
        /// /// <response code="200"> New OrderResponseDto</response>
        /// <response code="400">Order Not Exist</response>        
        // GET: api/Order/OrderDetail
        [HttpGet]
        [Route("OrderDetail")]
        public async Task<ActionResult<OrderResponseDto>> OrderDetail([FromQuery] string orderId)
        {

            if (string.IsNullOrEmpty(orderId))
                return StatusCode(400, "Order Not Exist");

            var orderResponseDto = await _orderService.OrderDetail(orderId);
            return StatusCode(200, orderResponseDto);
          
        }
    }
}
