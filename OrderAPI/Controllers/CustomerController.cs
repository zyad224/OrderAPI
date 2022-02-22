using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrderAPI.Dtos.CustomerDtos;
using OrderAPI.Services;
using OrderAPI.Utilities.CustomExceptions;
using OrderAPI.Utilities.CustomExceptions.CustomerExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderAPI.Controllers
{
    [Route("api/Customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IJwtService _jwtService;


        public CustomerController(ICustomerService customerService, IJwtService jwtService)
        {
            _customerService = customerService;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Register New Customer.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Customer/Register
        ///     {        
        ///       "UserName": "Zyad",
        ///       "Password": "123"
        ///     }
        /// </remarks>
        /// <returns> New CustomerResponseDto</returns>
        /// /// <response code="200"> New CustomerResponseDto</response>
        /// <response code="400">Invalid CustomerRequestDto</response> 
        ///
        ///  
        // POST: api/Customer/Register
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<CustomerResponseDto>> Register([FromBody] CustomerRequestDto customerRequestDto)
        {
           
          if (!ModelState.IsValid)
                return StatusCode(400, "Invalid CustomerRequestDto");

          var customerResponseDto = await _customerService.AddCustomer(customerRequestDto);
          return StatusCode(200, customerResponseDto);

        }

        /// <summary>
        /// Registered Customer Login.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Customer/Login
        ///     {        
        ///       "UserName": "Zyad",
        ///       "Password": "123"
        ///     }
        /// </remarks>
        /// <returns> Logged CustomerResponseDto</returns>
        /// /// <response code="200"> Logged CustomerResponseDto</response>
        /// <response code="400">Invalid CustomerRequestDto</response> 
        /// <response code="403">Customer Not Authenticated</response> 
        // POST: api/Customer/Login
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<CustomerResponseDto>> Login([FromBody] CustomerRequestDto customerRequestDto)
        {

            if (!ModelState.IsValid)
                return StatusCode(400, "Invalid CustomerRequestDto");

            var customerResponseDto = await _customerService.AuthenticateCustomer(customerRequestDto);            
            customerResponseDto.JwtToken = _jwtService.GenerateJWT();
            return StatusCode(200, customerResponseDto);           
        }

    }
}
