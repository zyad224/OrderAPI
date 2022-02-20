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

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<CustomerResponseDto>> Register([FromBody] CustomerRequestDto customerRequestDto)
        {
           
            try
            {
                var customerResponseDto = await _customerService.AddCustomer(customerRequestDto);
                return StatusCode(200, customerResponseDto);

            }
            catch (CustomerAlreadyExistException)
            {
                return StatusCode(400, "Customer Already Exist");

            }
            catch (DatabaseSaveException)
            {
                return StatusCode(500, "Internal Server Error");

            }


        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<CustomerResponseDto>> Login([FromBody] CustomerRequestDto customerRequestDto)
        {

            try
            {
                var customerResponseDto = await _customerService.AuthenticateCustomer(customerRequestDto);            
                customerResponseDto.JwtToken = _jwtService.GenerateJWT();
                return StatusCode(200, customerResponseDto);
                
            }          
            catch (CustomerNotAuthenticated)
            {
                return StatusCode(403, "Customer Not Authenticated");

            }

        }

    }
}
