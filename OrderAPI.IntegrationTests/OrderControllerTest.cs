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
using OrderAPI.Dtos.Dtos.OrderDtos;
using OrderAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.IntegrationTests
{
    public class OrderControllerTest
    {
        private IOrderService _orderService;
        private IOrderDal _orderDal;
        private ICustomerDal _customerDal;
        private DbApiContext _dbApiContext;
        private IMapper _mapper;
        private IConfiguration _configuration;

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
        public async Task PlaceOrderEndPoint_CustomerExist_BadOrderId_Returns_400_InvalidOrderRequestDto()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);
            var orderController = new OrderController(_orderService);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };
            var customerResponseDto = await _customerDal.AddCustomer(customerRequestDto);

            var orderRequestDto = new OrderRequestDto() { OrderId = "", CustomerId = customerResponseDto.CustomerId};
            orderRequestDto.ProductTypesQuantities.Add(new ProductTypeQuantityDto() { ProductType = ProductTypeDto.photoBook, Quantity = 1 });

            var orderResponseDto = await orderController.PlaceOrder(orderRequestDto);

            var requestResponseStatusCode = (orderResponseDto.Result as ObjectResult).StatusCode;
            var requestResponse = (orderResponseDto.Result as ObjectResult).Value;


            //Assert
            Assert.IsTrue(requestResponseStatusCode == 400);
            Assert.IsTrue(requestResponse.ToString() == "Invalid OrderRequestDto Model");

        }

        [Test]
        public async Task PlaceOrderEndPoint_BadCustomerId_Returns_400_InvalidOrderRequestDto()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);
            var orderController = new OrderController(_orderService);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };
            var customerResponseDto = await _customerDal.AddCustomer(customerRequestDto);

            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = "" };
            orderRequestDto.ProductTypesQuantities.Add(new ProductTypeQuantityDto() { ProductType = ProductTypeDto.photoBook, Quantity = 1 });

            var orderResponseDto = await orderController.PlaceOrder(orderRequestDto);

            var requestResponseStatusCode = (orderResponseDto.Result as ObjectResult).StatusCode;
            var requestResponse = (orderResponseDto.Result as ObjectResult).Value;


            //Assert
            Assert.IsTrue(requestResponseStatusCode == 400);
            Assert.IsTrue(requestResponse.ToString() == "Invalid OrderRequestDto Model");

        }
        [Test]
        public async Task PlaceOrderEndPoint_BadProductTypesQuantities_Returns_400_InvalidOrderRequestDto()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);
            var orderController = new OrderController(_orderService);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };
            var customerResponseDto = await _customerDal.AddCustomer(customerRequestDto);

            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = customerResponseDto.CustomerId};

            var orderResponseDto = await orderController.PlaceOrder(orderRequestDto);

            var requestResponseStatusCode = (orderResponseDto.Result as ObjectResult).StatusCode;
            var requestResponse = (orderResponseDto.Result as ObjectResult).Value;


            //Assert
            Assert.IsTrue(requestResponseStatusCode == 400);
            Assert.IsTrue(requestResponse.ToString() == "Invalid OrderRequestDto Model");

        }

        [Test]
        public async Task PlaceOrderEndPoint_ZeroOrderBinWidth_Returns_400_InvalidOrderBinWidth()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);
            var orderController = new OrderController(_orderService);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };
            var customerResponseDto = await _customerDal.AddCustomer(customerRequestDto);

            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = customerResponseDto.CustomerId};
            orderRequestDto.ProductTypesQuantities.Add(new ProductTypeQuantityDto() { ProductType = ProductTypeDto.photoBook, Quantity = 0 });

            var orderResponseDto = await orderController.PlaceOrder(orderRequestDto);

            var requestResponseStatusCode = (orderResponseDto.Result as ObjectResult).StatusCode;
            var requestResponse = (orderResponseDto.Result as ObjectResult).Value;


            //Assert
            Assert.IsTrue(requestResponseStatusCode == 400);
            Assert.IsTrue(requestResponse.ToString() == "Invalid OrderBin Width");

        }
        [Test]
        public async Task PlaceOrderEndPoint_NegativeOrderBinWidth_Returns_400_InvalidOrderBinWidth()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);
            var orderController = new OrderController(_orderService);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };
            var customerResponseDto = await _customerDal.AddCustomer(customerRequestDto);

            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = customerResponseDto.CustomerId };
            orderRequestDto.ProductTypesQuantities.Add(new ProductTypeQuantityDto() { ProductType = ProductTypeDto.photoBook, Quantity = -1 });

            var orderResponseDto = await orderController.PlaceOrder(orderRequestDto);

            var requestResponseStatusCode = (orderResponseDto.Result as ObjectResult).StatusCode;
            var requestResponse = (orderResponseDto.Result as ObjectResult).Value;


            //Assert
            Assert.IsTrue(requestResponseStatusCode == 400);
            Assert.IsTrue(requestResponse.ToString() == "Invalid OrderBin Width");

        }

        [Test]
        public async Task PlaceOrderEndPoint_ValidOrder_CustomerNotExist_Returns_400_CustomerNotExist()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);
            var orderController = new OrderController(_orderService);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };
            var customerResponseDto = await _customerDal.AddCustomer(customerRequestDto);

            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = "1" };
            orderRequestDto.ProductTypesQuantities.Add(new ProductTypeQuantityDto() { ProductType = ProductTypeDto.photoBook, Quantity = 1 });

            var orderResponseDto = await orderController.PlaceOrder(orderRequestDto);

            var requestResponseStatusCode = (orderResponseDto.Result as ObjectResult).StatusCode;
            var requestResponse = (orderResponseDto.Result as ObjectResult).Value;


            //Assert
            Assert.IsTrue(requestResponseStatusCode == 400);
            Assert.IsTrue(requestResponse.ToString() == "Customer Does not Exist");

        }
        [Test]
        public async Task PlaceOrderEndPoint_OrderAlreadyExist_ValidCustomer_Returns_400_CustomerNotExist()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);
            var orderController = new OrderController(_orderService);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };
            var customerResponseDto = await _customerDal.AddCustomer(customerRequestDto);

            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = customerResponseDto.CustomerId };
            orderRequestDto.ProductTypesQuantities.Add(new ProductTypeQuantityDto() { ProductType = ProductTypeDto.photoBook, Quantity = 1 });
            var orderResponseDto = await orderController.PlaceOrder(orderRequestDto);

            var orderAlreadyExistResponseDto = await orderController.PlaceOrder(orderRequestDto);

            var requestResponseStatusCode = (orderAlreadyExistResponseDto.Result as ObjectResult).StatusCode;
            var requestResponse = (orderAlreadyExistResponseDto.Result as ObjectResult).Value;


            //Assert
            Assert.IsTrue(requestResponseStatusCode == 400);
            Assert.IsTrue(requestResponse.ToString() == "Order Already Exist");

        }

        [Test]
        public async Task PlaceOrderEndPoint_ValidOrder_ValidCustomer_Returns_200_CustomerNotExist()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);
            var orderController = new OrderController(_orderService);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };
            var customerResponseDto = await _customerDal.AddCustomer(customerRequestDto);

            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = customerResponseDto.CustomerId};
            orderRequestDto.ProductTypesQuantities.Add(new ProductTypeQuantityDto() { ProductType = ProductTypeDto.photoBook, Quantity = 1 });

            var orderResponseDto = await orderController.PlaceOrder(orderRequestDto);

            var requestResponseStatusCode = (orderResponseDto.Result as ObjectResult).StatusCode;
            var requestResponse = ((orderResponseDto.Result as ObjectResult).Value) as OrderResponseDto;


            //Assert
            Assert.IsTrue(requestResponseStatusCode == 200);
            Assert.IsTrue(requestResponse.OrderId == orderRequestDto.OrderId);
            Assert.IsTrue(requestResponse.RequiredBinWidth == 19);
            Assert.IsTrue(requestResponse.ProductTypesQuantities.Count() == 1);
            Assert.IsTrue(requestResponse.ProductTypesQuantities.FirstOrDefault().ProductType == ProductTypeDto.photoBook);
            Assert.IsTrue(requestResponse.ProductTypesQuantities.FirstOrDefault().Quantity == 1);

        }

        [Test]
        public async Task OrderDetailEndPoint_BadOrderId_Returns_400_OrderNotExist()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);
            var orderController = new OrderController(_orderService);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };
            var customerResponseDto = await _customerDal.AddCustomer(customerRequestDto);
            var orderResponseDto = await orderController.OrderDetail(String.Empty);

            var requestResponseStatusCode = (orderResponseDto.Result as ObjectResult).StatusCode;
            var requestResponse = (orderResponseDto.Result as ObjectResult).Value;


            //Assert
            Assert.IsTrue(requestResponseStatusCode == 400);
            Assert.IsTrue(requestResponse.ToString() == "Order Not Exist");

        }

        [Test]
        public async Task OrderDetailEndPoint_ValidOrder_Returns_200_OrderResponseDto()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);
            var orderController = new OrderController(_orderService);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };
            var customerResponseDto = await _customerDal.AddCustomer(customerRequestDto);

            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = customerResponseDto.CustomerId };
            orderRequestDto.ProductTypesQuantities.Add(new ProductTypeQuantityDto() { ProductType = ProductTypeDto.photoBook, Quantity = 1 });
            var orderResponseDto = await orderController.PlaceOrder(orderRequestDto);

            var orderDetailResponseDto = await orderController.OrderDetail(orderRequestDto.OrderId);

            var requestResponseStatusCode = (orderDetailResponseDto.Result as ObjectResult).StatusCode;
            var requestResponse = ((orderDetailResponseDto.Result as ObjectResult).Value) as OrderResponseDto;


            //Assert
            Assert.IsTrue(requestResponseStatusCode == 200);
            Assert.IsTrue(requestResponse.OrderId == orderRequestDto.OrderId);
            Assert.IsTrue(requestResponse.RequiredBinWidth == 19);
            Assert.IsTrue(requestResponse.ProductTypesQuantities.Count() == 1);
            Assert.IsTrue(requestResponse.ProductTypesQuantities.FirstOrDefault().ProductType == ProductTypeDto.photoBook);
            Assert.IsTrue(requestResponse.ProductTypesQuantities.FirstOrDefault().Quantity == 1);


        }
    }

  
}
