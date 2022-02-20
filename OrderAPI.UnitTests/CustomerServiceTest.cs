using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
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

namespace OrderAPI.UnitTests
{
    public class CustomerServiceTest
    {
        private ICustomerDal _customerDal;
        private ICustomerService _customerService;
        private IMapper _mapper;
        private DbApiContext _dbApiContext;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<DbApiContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;

            var configMapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            _mapper = configMapper.CreateMapper();
            _dbApiContext = new DbApiContext(options);
        }

        [Test]
        public async Task AddCustomer_Returns_CustomerResponseDto()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _customerService = new CustomerService(_customerDal);
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };
            //Act
            var customerResponseDto = await _customerService.AddCustomer(customerRequestDto);
            //Assert
            Assert.IsTrue(customerResponseDto != null);
            Assert.IsTrue(customerResponseDto.UserName == customerRequestDto.UserName);
            Assert.IsTrue(customerResponseDto.CustomerId != null);


        }

        [Test]
        public void AddCustomer_InvalidCustomerRequestDto_Throws_InvalidCustomerRequestDtoException()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _customerService = new CustomerService(_customerDal);
            var customerRequestDto = new CustomerRequestDto();

            //Assert
            Assert.ThrowsAsync<InvalidCustomerRequestDtoException>(() => _customerService.AddCustomer(customerRequestDto));

        }

        [Test]
        public async Task AddCustomer_AlreadyExisting_Throws_CustomerAlreadyExistException()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _customerService = new CustomerService(_customerDal);
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };

            //Act
            var customerResponseDto = await _customerService.AddCustomer(customerRequestDto);

            //Assert
            Assert.ThrowsAsync<CustomerAlreadyExistException>(() => _customerService.AddCustomer(customerRequestDto));

        }

        [Test]
        public async Task AuthenticateCustomer_Returns_CustomerResponseDto()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _customerService = new CustomerService(_customerDal);
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };
            //Act
            var customerResponseDto = await _customerService.AddCustomer(customerRequestDto);
            var authticatedCustomer = await _customerService.AuthenticateCustomer(customerRequestDto);
            //Assert
            Assert.IsTrue(authticatedCustomer != null);
            Assert.IsTrue(authticatedCustomer.UserName == customerResponseDto.UserName);

        }

        [Test]
        public async Task GetCustomerById_Returns_CustomerResponseDto()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _customerService = new CustomerService(_customerDal);
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };

            //Act
            var customerResponseDto = await _customerService.AddCustomer(customerRequestDto);
            //Assert
            Assert.IsTrue(await _customerService.GetCustomerById(customerResponseDto.CustomerId) != null);

        }

    }
}
