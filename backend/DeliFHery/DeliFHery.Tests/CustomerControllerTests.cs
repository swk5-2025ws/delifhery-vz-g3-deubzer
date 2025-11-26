using Castle.Core.Resource;
using DeliFHery.API.Controllers;
using DeliFHery.API.Interfaces;
using DeliFHery.API.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel;
namespace DeliFHery.Tests
{
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomerRepo> _customerRepoMock;
        private readonly CustomersController _controller;
        private readonly IEnumerable<Customer> _customers;
        private readonly Customer _customer;

        public CustomerControllerTests()
        {
            _customerRepoMock = new Mock<ICustomerRepo>();
            _controller = new CustomersController(_customerRepoMock.Object);
            _customers = new List<Customer> {
                new Customer {customerId = 1, username = "test1", identityProviderUserId = "t-1"},
                new Customer {customerId = 2, username = "test2", identityProviderUserId = "t-2"}
            };
            _customer = new Customer { customerId = 3, username = "test3", identityProviderUserId = "t-3" };
            
        }

        [Fact]
        public async Task GetAllCustomersAsyncTest_ReturnOk()
        {
            //Arrange
            var customers = _customers;
            

            _customerRepoMock.Setup(r => r.GetAllCustomersAsync(It.IsAny<CancellationToken>())).ReturnsAsync(customers);

            //Act
            var result = await _controller.GetAll();

            //Assert

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Customer>>(okResult.Value);
            Assert.Collection(model,
                c => Assert.Equal("test1", c.username),
                c => Assert.Equal("test2", c.username));
        }

        [Fact]
        public async Task GetAllCustomerAsync_ReturnNotFound_WhenRepoIsNull()
        {
            //Arrange
            _customerRepoMock
                .Setup(r => r.GetAllCustomersAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<Customer>)null!);
            //Act
            var result = await _controller.GetAll();

            //Assert
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetCustomerByIdAsync_ReturnsOk()
        {
            //arrange
            var customer = _customer;

            _customerRepoMock
                .Setup(r => r.GetByIdAsync(3, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            //Act
            var result = await _controller.GetById(3);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal(3, model.customerId);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ReturnsNotFound_WhenCustomerIsNull()
        {
            //Arrange
            _customerRepoMock
                .Setup(r => r.GetByIdAsync(4, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Customer?)null!);

            //Act
            var result = await _controller.GetById(4);

            //Assert
            Assert.IsType<NotFoundResult>(result);

        }
        
    }
}
