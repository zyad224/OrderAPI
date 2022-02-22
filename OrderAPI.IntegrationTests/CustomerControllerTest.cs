using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OrderAPI.Controllers;
using OrderAPI.DAL;
using OrderAPI.DAL.AutoMapperProfiles;
using OrderAPI.DAL.Data;
using OrderAPI.DAL.Interfaces;
using OrderAPI.Dtos.CustomerDtos;
using OrderAPI.Services;
using OrderAPI.Utilities.CustomExceptions;
using OrderAPI.Utilities.CustomExceptions.CustomerExceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.IntegrationTests
{
    public class CustomerControllerTest
    {
        private  ICustomerService _customerService;
        private  ICustomerDal _customerDal;
        private  IJwtService _jwtService;
        private  DbApiContext _dbApiContext;
        private  IMapper _mapper;
        private  IConfiguration _configuration;


        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<DbApiContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;

            _configuration = new ConfigurationBuilder()
                         .AddJsonFile("appsettingstest.json")
                         .Build();

            var configMapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            _mapper = configMapper.CreateMapper();

            _dbApiContext = new DbApiContext(options);
        }

        [Test]
        public void RegisterEndPoint_BadCustomerUsername_Returns_400_InvalidCustomerRequestDto()
        {

            //Arrange
            _jwtService = new JwtService(_configuration);
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _customerService = new CustomerService(_customerDal);
            var customerController = new CustomerController(_customerService, _jwtService);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "", Password = "PaswordTest" };

            //Assert
            Assert.ThrowsAsync<InvalidCustomerRequestDtoException>(() => customerController.Register(customerRequestDto));
       
        }
        [Test]
        public void RegisterEndPoint_BadCustomerPassword_Returns_400_InvalidCustomerRequestDto()
        {

            //Arrange
            _jwtService = new JwtService(_configuration);
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _customerService = new CustomerService(_customerDal);
            var customerController = new CustomerController(_customerService, _jwtService);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "" };

            //Assert
            Assert.ThrowsAsync<InvalidCustomerRequestDtoException>(() => customerController.Register(customerRequestDto));


        }
        [Test]
        public async Task RegisterEndPoint_ValidNewCustomerData_Returns_200_CustomerResponseDto()
        {

            //Arrange
            _jwtService = new JwtService(_configuration);
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _customerService = new CustomerService(_customerDal);
            var customerController = new CustomerController(_customerService, _jwtService);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };
            var customerResponseDto = await customerController.Register(customerRequestDto);
            var requestResponseStatusCode = (customerResponseDto.Result as ObjectResult).StatusCode;
            var requestResponse = ((customerResponseDto.Result as ObjectResult).Value)as CustomerResponseDto;
            

            //Assert
            Assert.IsTrue(requestResponseStatusCode == 200);
            Assert.IsTrue(!string.IsNullOrEmpty(requestResponse.CustomerId));
            Assert.IsTrue(requestResponse.UserName == customerRequestDto.UserName);

        }
        [Test]
        public async Task RegisterEndPoint_AlreadyExistingCustomer_Returns_400_CustomerAlreadyExist()
        {

            //Arrange
            _jwtService = new JwtService(_configuration);
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _customerService = new CustomerService(_customerDal);
            var customerController = new CustomerController(_customerService, _jwtService);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };
            var customerResponseDto = await customerController.Register(customerRequestDto);


            //Assert
            Assert.ThrowsAsync<CustomerAlreadyExistException>(() => customerController.Register(customerRequestDto));


        }
        [Test]
        public void LoginEndPoint_BadCustomerUserName_Returns_400_InvalidCustomerRequestDto()
        {

            //Arrange
            _jwtService = new JwtService(_configuration);
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _customerService = new CustomerService(_customerDal);
            var customerController = new CustomerController(_customerService, _jwtService);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "", Password = "Password" };

            //Assert
            Assert.ThrowsAsync<InvalidCustomerRequestDtoException>(() => customerController.Login(customerRequestDto));


        }
        [Test]
        public void LoginEndPoint_BadCustomerPassword_Returns_400_InvalidCustomerRequestDto()
        {

            //Arrange
            _jwtService = new JwtService(_configuration);
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _customerService = new CustomerService(_customerDal);
            var customerController = new CustomerController(_customerService, _jwtService);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "" };

            //Assert
            Assert.ThrowsAsync<InvalidCustomerRequestDtoException>(() => customerController.Login(customerRequestDto));


        }
        [Test]
        public async Task LoginEndPoint_ValidCustomer_WrongUserName_Returns_403_CustomerNotAuthenticated()
        {

            //Arrange
            _jwtService = new JwtService(_configuration);
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _customerService = new CustomerService(_customerDal);
            var customerController = new CustomerController(_customerService, _jwtService);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "Password" };
            var customerResponseDto = await customerController.Register(customerRequestDto);

            var customerLoginRequestDto = new CustomerRequestDto() { UserName = "UserTest1", Password = "Password" };


            //Assert
            Assert.ThrowsAsync<CustomerNotAuthenticated>(() => customerController.Login(customerLoginRequestDto));


        }
        [Test]
        public async Task LoginEndPoint_ValidCustomer_WrongPassword_Returns_403_CustomerNotAuthenticated()
        {

            //Arrange
            _jwtService = new JwtService(_configuration);
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _customerService = new CustomerService(_customerDal);
            var customerController = new CustomerController(_customerService, _jwtService);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "Password" };
            var customerResponseDto = await customerController.Register(customerRequestDto);

            var customerLoginRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "Password1" };

            //Assert
            Assert.ThrowsAsync<CustomerNotAuthenticated>(() => customerController.Login(customerLoginRequestDto));


        }
        [Test]
        public async Task LoginEndPoint_ValidCustomer_Returns_200_CustomerResponseDto_JwtToken()
        {

            //Arrange
            _jwtService = new JwtService(_configuration);
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _customerService = new CustomerService(_customerDal);
            var customerController = new CustomerController(_customerService, _jwtService);

            //Act
            var customerRegisterRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "Password" };
            var customerRegisterResponseDto = await customerController.Register(customerRegisterRequestDto);

            var customerLoginRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "Password" };
            var customerLoginResponseDto = await customerController.Login(customerLoginRequestDto);

            var requestResponseStatusCode = (customerLoginResponseDto.Result as ObjectResult).StatusCode;
            var requestResponse = ((customerLoginResponseDto.Result as ObjectResult).Value) as CustomerResponseDto;


            //Assert
            Assert.IsTrue(requestResponseStatusCode == 200);
            Assert.IsTrue(!string.IsNullOrEmpty(requestResponse.JwtToken));
            Assert.IsTrue(!string.IsNullOrEmpty(requestResponse.UserName));
            Assert.IsTrue(!string.IsNullOrEmpty(requestResponse.CustomerId));
            Assert.IsTrue(requestResponse.CustomerId == 
                (((customerRegisterResponseDto.Result as ObjectResult).Value) as CustomerResponseDto).CustomerId);
        }
    }
}
