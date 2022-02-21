using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OrderAPI.DAL;
using OrderAPI.DAL.AutoMapperProfiles;
using OrderAPI.DAL.Data;
using OrderAPI.DAL.Interfaces;
using OrderAPI.Dtos.CustomerDtos;
using OrderAPI.Dtos.Dtos.OrderDtos;
using OrderAPI.Utilities.CustomExceptions.CustomerExceptions;
using OrderAPI.Utilities.CustomExceptions.OrderExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.UnitTests
{
    public class OrderDalTest
    {
        private ICustomerDal _customerDal;
        private IOrderDal _orderDal;
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
        public void PlaceOrder_InvalidOrderId_Returns_InvalidOrderRequestDtoException()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            
            //Act
            var orderRequestDto = new OrderRequestDto() { OrderId = "", CustomerId = "123", };
            decimal requiredBinWidth = 1;

            //Assert
            Assert.ThrowsAsync<InvalidOrderRequestDtoException>(() => _orderDal.PlaceOrder(orderRequestDto, requiredBinWidth));
        }
        [Test]
        public void PlaceOrder_InvalidCustomerId_Returns_InvalidOrderRequestDtoException()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);

            //Act
            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = "", };
            decimal requiredBinWidth = 1;

            //Assert
            Assert.ThrowsAsync<InvalidOrderRequestDtoException>(() => _orderDal.PlaceOrder(orderRequestDto, requiredBinWidth));
        }
       
        [Test]
        public void PlaceOrder_InvalidProductTypesQuantities_Returns_InvalidOrderRequestDtoException()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);

            //Act
            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = "123", };
            decimal requiredBinWidth = 10;

            //Assert
            Assert.ThrowsAsync<InvalidOrderRequestDtoException>(() => _orderDal.PlaceOrder(orderRequestDto, requiredBinWidth));
        }
        [Test]
        public void PlaceOrder_ValidOrder_CustomerNotExists_Returns_CustomerNotExistException()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);

            //Act
            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = "123", };
            orderRequestDto.ProductTypesQuantities.Add(new ProductTypeQuantityDto() { ProductType = ProductTypeDto.photoBook, Quantity = 1 });
            decimal requiredBinWidth = 10;

            //Assert
            Assert.ThrowsAsync<CustomerNotExistException>(() => _orderDal.PlaceOrder(orderRequestDto, requiredBinWidth));
        }
        [Test]
        public async Task PlaceOrder_ValidOrder_ValidCustomer_Returns_OrderResponseDto()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };
            var customerResponseDto = await _customerDal.AddCustomer(customerRequestDto);

            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = customerResponseDto.CustomerId, };
            orderRequestDto.ProductTypesQuantities.Add(new ProductTypeQuantityDto() { ProductType = ProductTypeDto.photoBook, Quantity = 1 });
            decimal requiredBinWidth = 10;
            var orderResponseDto = await _orderDal.PlaceOrder(orderRequestDto, requiredBinWidth);
            //Assert

            Assert.IsTrue(orderResponseDto != null);
            Assert.IsTrue(orderResponseDto.RequiredBinWidth == requiredBinWidth);
            Assert.IsTrue(orderResponseDto.ProductTypesQuantities.Count() == 1);
            Assert.IsTrue(orderResponseDto.ProductTypesQuantities.FirstOrDefault().ProductType == ProductTypeDto.photoBook);
            Assert.IsTrue(orderResponseDto.ProductTypesQuantities.FirstOrDefault().Quantity == 1);
      }
        [Test]
        public void PlaceOrder_InvalidRequiredBinWidth_Returns_InvalidOrderRequestDtoException()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);

            //Act
            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = "123", };
            orderRequestDto.ProductTypesQuantities.Add(new ProductTypeQuantityDto() { ProductType = ProductTypeDto.photoBook, Quantity = 1 });

            decimal requiredBinWidth = 0;

            //Assert
            Assert.ThrowsAsync<InvalidOrderBinWidthException>(() => _orderDal.PlaceOrder(orderRequestDto, requiredBinWidth));
        }

        [Test]
        public void OrderDetail_InvalidOrder_Returns_OrderNotExistException()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);

            //Act
            string orderId = "inValid";

            //Assert
            Assert.ThrowsAsync<OrderNotExistException>(() => _orderDal.OrderDetail(orderId));
        }

        [Test]
        public async Task OrderDetail_ValidOrder_ValidCustomer_Returns_OrderRespondeDto()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };
            var customerResponseDto = await _customerDal.AddCustomer(customerRequestDto);

            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = customerResponseDto.CustomerId, };
            orderRequestDto.ProductTypesQuantities.Add(new ProductTypeQuantityDto() { ProductType = ProductTypeDto.photoBook, Quantity = 1 });
            decimal requiredBinWidth = 10;
            var orderResponseDto = await _orderDal.PlaceOrder(orderRequestDto, requiredBinWidth);

            var orderDetails = await _orderDal.OrderDetail(orderRequestDto.OrderId);
            //Assert

            Assert.IsTrue(orderDetails != null);
            Assert.IsTrue(orderDetails.RequiredBinWidth == requiredBinWidth);
            Assert.IsTrue(orderDetails.ProductTypesQuantities.Count() == 1);
            Assert.IsTrue(orderDetails.ProductTypesQuantities.FirstOrDefault().ProductType == ProductTypeDto.photoBook);
            Assert.IsTrue(orderDetails.ProductTypesQuantities.FirstOrDefault().Quantity == 1);
        }
    }
}
