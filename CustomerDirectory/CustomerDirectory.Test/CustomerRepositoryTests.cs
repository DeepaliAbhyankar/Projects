using AutoMapper;
using CustomerDirectory.Command;
using CustomerDirectory.Data;
using CustomerDirectory.DTO;
using CustomerDirectory.Exceptions;
using CustomerDirectory.Query;
using CustomerDirectory.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CustomerDirectory.Test
{
    public class CustomerRepositoryTests : UnitTestBase
    {       
        public CustomerRepositoryTests()
        {
            Initialize();
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

        }
        [Fact]
        public async Task Can_Add_Customer()
        { 
            
            var customer = new CustomerDto() { FirstName = "Danny", LastName = "Dog", DateOfBirth = DateTime.Parse("1999-06-01") };
            await _repositoy.Add(customer);
            var items = await _repositoy.Get();
            Assert.Contains(items, x => x.FirstName == "Danny");
        }

        [Fact]
        public async Task Delete_Customer_ThatDoNotExists()
        {
            await Assert.ThrowsAsync<CustomerNotFoundException>(() => _repositoy.Delete(100));
        }

        [Fact]
        public async Task Search_Customer_ThatDoNotExists()
        {
            await Assert.ThrowsAsync<CustomerNotFoundException>(() => _repositoy.Search("abcd"));
        }
    }
}
