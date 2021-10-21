using AutoMapper;
using CustomerDirectory.Command;
using CustomerDirectory.Controllers;
using CustomerDirectory.Data;
using CustomerDirectory.DTO;
using CustomerDirectory.Query;
using CustomerDirectory.Repository;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CustomerDirectory.Test
{
    public class CustomerControllerTests : UnitTestBase
    {
        private  CustomerController _controller;
                
        public CustomerControllerTests()
        {
            Initialize();
        }

        [Fact]
        public async Task Get_Returns_OkResult()
        {
            var okResult = await _controller.Get();
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public async Task Get_Returns_AllItems()
        {
            var okResult = (await _controller.Get()) as OkObjectResult;
            var items = Assert.IsType<List<CustomerDto>>(okResult.Value);
            Assert.Contains(items, x => x.FirstName == "Rosie");
            Assert.Contains(items, x => x.FirstName == "Emily");
            Assert.Contains(items, x => x.FirstName == "Zoe");
        }

        [Fact]
        public async Task Get_Returns_NotFound()
        {
            var result = await _controller.Get(80);
            Assert.IsType<NotFoundObjectResult>(result as NotFoundObjectResult);
        }

        [Fact]
        public async Task Search_Returns_Result()
        {
            var result = await _controller.Search("Zebra") as OkObjectResult;
            var items = Assert.IsType<List<CustomerDto>>(result.Value);
            Assert.Contains(items, x => x.FirstName == "Zoe" && x.LastName == "Zebra");
        }

        [Fact]
        public async Task Search_Returns_NotFound_Result()
        {
            var result = await _controller.Search("Lion") as NotFoundObjectResult;
            Assert.IsType<NotFoundObjectResult>(result);            
            Assert.Equal("Customer with name : Lion does not exists", result.Value.ToString());
        }

        [Fact]
        public async Task Add_Cutomer_withEmptyFirstName_Returns_BadRequest()
        {
            var customer1 = new CustomerDto() { FirstName = "", LastName = "Baggins", DateOfBirth = DateTime.Parse("1999-06-01") };
            var result = await _controller.Add(customer1) as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = ((List<ValidationFailure>)result.Value)[0].ErrorMessage;
            Assert.Equal("Required First Name", errorMessage);
        }

        [Fact]
        public async Task Add_Cutomer_withEmptyLastName_Returns_BadRequest()
        {
            var customer1 = new CustomerDto() { FirstName = "Bilbo", LastName = "", DateOfBirth = DateTime.Parse("1999-06-01") };
            var result = await _controller.Add(customer1) as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = ((List<ValidationFailure>)result.Value)[0].ErrorMessage;
            Assert.Equal("Required Last Name", errorMessage);
        }

        [Fact]
        public async Task Delete_Returns_NotFound_Result()
        {
            var result = await _controller.Delete(90) as NotFoundObjectResult;
            Assert.IsType<NotFoundObjectResult>(result);            
        }

        [Fact]
        public async Task Edit_Returns_NotFound_Result()
        {
            var customer1 = new CustomerDto() { CustomerId = 90, FirstName = "Bilbo", LastName = "Baggins", DateOfBirth = DateTime.Parse("1999-06-01") };
            var result = await _controller.Edit(customer1) as NotFoundObjectResult;
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Customer with id : 90 does not exists", result.Value.ToString());
        }



        private void Initialize()
        {
            _query = new CustomerQuery(Context);
            var userProvider = new Mock<IUserProvider>();
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<Customer>(It.IsAny<CustomerDto>()))
                .Returns((CustomerDto source) =>
                {
                    return new Customer()
                    {
                        FirstName = source.FirstName,
                        LastName = source.LastName,
                        DateOfBirth = source.DateOfBirth
                    };
                });

            mapper.Setup(x => x.Map<List<CustomerDto>>(It.IsAny<List<Customer>>()))
                .Returns((List<Customer> source) =>
                {
                    var destinationList = new List<CustomerDto>();
                    source.ForEach(x =>
                    {
                        var destination = new CustomerDto()
                        {
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            DateOfBirth = x.DateOfBirth
                        };
                        destinationList.Add(destination);
                    });
                    return destinationList;
                });
            _command = new CustomerCommand(Context, userProvider.Object);
            _repositoy = new CustomerRepository(_command, mapper.Object, _query);
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();            
            httpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
            _controller = new CustomerController(_repositoy, httpContextAccessor.Object);

        }
    }
    
}
