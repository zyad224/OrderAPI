using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OrderAPI.DAL;
using OrderAPI.DAL.AutoMapperProfiles;
using OrderAPI.DAL.Data;
using OrderAPI.DAL.Interfaces;
using OrderAPI.Dtos.CustomerDtos;
using OrderAPI.Dtos.Dtos.OrderDtos;
using OrderAPI.Services;
using OrderAPI.Utilities.CustomExceptions.OrderExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.UnitTests
{
    public class OrderServiceTest
    {
        private IOrderDal _orderDal;
        private IConfiguration _configuration;
        private ICustomerDal _customerDal;
        private IMapper _mapper;
        private IOrderService _orderService;
        private DbApiContext _dbApiContext;

     
        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<DbApiContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;

            _configuration= new ConfigurationBuilder()
                         .AddJsonFile("appsettingstest.json")
                         .Build();

            var configMapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            _mapper = configMapper.CreateMapper();

            _dbApiContext = new DbApiContext(options);
        }

        [Test]
        public void CalculateOrderBinWidth_1PhotoCopy_Returns_19mm()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);

            //Act
            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = "123", };
            orderRequestDto.ProductTypesQuantities.Add(new ProductTypeQuantityDto() { ProductType = ProductTypeDto.photoBook, Quantity = 1 });

            decimal orderBinWidth= _orderService.CalculateOrderBinWidth(orderRequestDto);

            //Assert
            Assert.IsTrue(orderBinWidth == 19);
        }

        [Test]
        public void CalculateOrderBinWidth_2PhotoCopy_Returns_38mm()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);

            //Act
            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = "123", };
            orderRequestDto.ProductTypesQuantities.Add(new ProductTypeQuantityDto() { ProductType = ProductTypeDto.photoBook, Quantity = 2 });

            decimal orderBinWidth = _orderService.CalculateOrderBinWidth(orderRequestDto);

            //Assert
            Assert.IsTrue(orderBinWidth == 38);
        }

        [Test]
        public void CalculateOrderBinWidth_1PhotoCopy2Calendars1mug_Returns_133mm()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);

            //Act
            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = "123", };

            orderRequestDto.ProductTypesQuantities
                .Add(
                new ProductTypeQuantityDto() { ProductType = ProductTypeDto.photoBook, Quantity = 1 }
                );
            orderRequestDto.ProductTypesQuantities
               .Add(
               new ProductTypeQuantityDto() { ProductType = ProductTypeDto.calendar, Quantity = 2 }
               );
            orderRequestDto.ProductTypesQuantities
             .Add(
             new ProductTypeQuantityDto() { ProductType = ProductTypeDto.mug, Quantity = 1 }
             );

            decimal orderBinWidth = _orderService.CalculateOrderBinWidth(orderRequestDto);

            //Assert
            Assert.IsTrue(orderBinWidth == 133);
        }
        [Test]
        public void CalculateOrderBinWidth_1PhotoCopy2Calendars2mug_Returns_133mm()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);

            //Act
            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = "123", };

            orderRequestDto.ProductTypesQuantities
                .Add(
                new ProductTypeQuantityDto() { ProductType = ProductTypeDto.photoBook, Quantity = 1 }
                );
            orderRequestDto.ProductTypesQuantities
               .Add(
               new ProductTypeQuantityDto() { ProductType = ProductTypeDto.calendar, Quantity = 2 }
               );
            orderRequestDto.ProductTypesQuantities
             .Add(
             new ProductTypeQuantityDto() { ProductType = ProductTypeDto.mug, Quantity = 2 }
             );

            decimal orderBinWidth = _orderService.CalculateOrderBinWidth(orderRequestDto);

            //Assert
            Assert.IsTrue(orderBinWidth == 133);
        }

        [Test]
        public void CalculateOrderBinWidth_4mugs_Returns_94mm()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);

            //Act
            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = "123", };
       
            orderRequestDto.ProductTypesQuantities
             .Add(
             new ProductTypeQuantityDto() { ProductType = ProductTypeDto.mug, Quantity = 4 }
             );

            decimal orderBinWidth = _orderService.CalculateOrderBinWidth(orderRequestDto);

            //Assert
            Assert.IsTrue(orderBinWidth == 94);
        }

        [Test]
        public void CalculateOrderBinWidth_5mugs_Returns_188mm()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);

            //Act
            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = "123", };

            orderRequestDto.ProductTypesQuantities
             .Add(
             new ProductTypeQuantityDto() { ProductType = ProductTypeDto.mug, Quantity = 5 }
             );

            decimal orderBinWidth = _orderService.CalculateOrderBinWidth(orderRequestDto);

            //Assert
            Assert.IsTrue(orderBinWidth == 188);
        }

        [Test]
        public void CalculateOrderBinWidth_1mug_Returns_94mm()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);

            //Act
            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = "123", };

            orderRequestDto.ProductTypesQuantities
             .Add(
             new ProductTypeQuantityDto() { ProductType = ProductTypeDto.mug, Quantity = 1 }
             );

            decimal orderBinWidth = _orderService.CalculateOrderBinWidth(orderRequestDto);

            //Assert
            Assert.IsTrue(orderBinWidth == 94);
        }

        [Test]
        public void CalculateOrderBinWidth_InvalidOrder_Returns_InvalidOrderRequestDtoException()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);

            //Act
            var orderRequestDto = new OrderRequestDto() { OrderId = "", CustomerId = "123", };

            //Assert
            Assert.Throws<InvalidOrderRequestDtoException>(()=>_orderService.CalculateOrderBinWidth(orderRequestDto));
        }

        [Test]
        public void PlaceOrder_InvalidOrder_Returns_InvalidOrderRequestDtoException()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);

            //Act
            var orderRequestDto = new OrderRequestDto() { OrderId = "", CustomerId = "123", };
            decimal requiredBinWidth = 10;

            //Assert
            Assert.ThrowsAsync<InvalidOrderRequestDtoException>(() => _orderService.PlaceOrder(orderRequestDto, requiredBinWidth));
        }

        [Test]
        public void PlaceOrder_InvalidBinWidth_Returns_InvalidOrderRequestDtoException()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);

            //Act
            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = "123", };

            orderRequestDto.ProductTypesQuantities
             .Add(
             new ProductTypeQuantityDto() { ProductType = ProductTypeDto.mug, Quantity = 1 }
             );

            decimal requiredBinWidth = 0;

            //Assert
            Assert.ThrowsAsync<InvalidOrderBinWidthException>(() => _orderService.PlaceOrder(orderRequestDto, requiredBinWidth));
        }

        [Test]
        public async Task PlaceOrder_ValidOrder_Returns_OrderResponseDto()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };
            var customerResponseDto = await _customerDal.AddCustomer(customerRequestDto);
            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = customerResponseDto.CustomerId, };
            orderRequestDto.ProductTypesQuantities
             .Add(
             new ProductTypeQuantityDto() { ProductType = ProductTypeDto.photoBook, Quantity = 1 }
             );

            var requiredBinWidth = _orderService.CalculateOrderBinWidth(orderRequestDto);
            var orderResponseDto = await _orderService.PlaceOrder(orderRequestDto, requiredBinWidth);

            //Assert

            Assert.IsTrue(orderResponseDto != null);
            Assert.IsTrue(orderResponseDto.OrderId == orderResponseDto.OrderId);
        }
        [Test]
        public void OrderDetail_InvalidOrder_Returns_OrderNotExistException()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);

            //Act
            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = "123", };

            orderRequestDto.ProductTypesQuantities
             .Add(
             new ProductTypeQuantityDto() { ProductType = ProductTypeDto.mug, Quantity = 1 }
             );

            decimal requiredBinWidth = 0;

            //Assert
            Assert.ThrowsAsync<OrderNotExistException>(() => _orderService.OrderDetail(orderRequestDto.OrderId));
        }
        [Test]
        public async Task OrderDetail_ValidOrder_Returns_OrderResponseDto()
        {

            //Arrange
            _customerDal = new CustomerDal(_dbApiContext, _mapper);
            _orderDal = new OrderDal(_dbApiContext, _mapper, _customerDal);
            _orderService = new OrderService(_orderDal, _configuration);

            //Act
            var customerRequestDto = new CustomerRequestDto() { UserName = "UserTest", Password = "PaswordTest" };
            var customerResponseDto = await _customerDal.AddCustomer(customerRequestDto);
            var orderRequestDto = new OrderRequestDto() { OrderId = "1", CustomerId = customerResponseDto.CustomerId, };
            orderRequestDto.ProductTypesQuantities
             .Add(
             new ProductTypeQuantityDto() { ProductType = ProductTypeDto.photoBook, Quantity = 1 }
             );

            var requiredBinWidth = _orderService.CalculateOrderBinWidth(orderRequestDto);
            var orderResponseDto = await _orderService.PlaceOrder(orderRequestDto, requiredBinWidth);

            var orderDetails = await _orderService.OrderDetail(orderRequestDto.OrderId);

            //Assert

            Assert.IsTrue(orderDetails != null);
            Assert.IsTrue(orderDetails.OrderId == orderRequestDto.OrderId);
            Assert.IsTrue(orderDetails.RequiredBinWidth == requiredBinWidth);
            Assert.IsTrue(orderDetails.ProductTypesQuantities.Count() == 1);
            Assert.IsTrue(orderDetails.ProductTypesQuantities.FirstOrDefault().ProductType == ProductTypeDto.photoBook);
            Assert.IsTrue(orderDetails.ProductTypesQuantities.FirstOrDefault().Quantity == 1);
        }

    }
}
