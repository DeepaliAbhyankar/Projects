using CustomerDirectory.Command;
using CustomerDirectory.Data;
using CustomerDirectory.Query;
using CustomerDirectory.Repository;
using Microsoft.EntityFrameworkCore;
using System;

namespace CustomerDirectory.Test
{
    public class UnitTestBase
    {
        protected readonly CustomerDirectoryDataContext Context;
        protected readonly DbContextOptions<CustomerDirectoryDataContext> _contextOptions;
        protected ICustomerQuery _query;
        protected ICustomerCommand _command;
        protected ICustomerRepository _repositoy;
        public UnitTestBase()
        {
            _contextOptions = new DbContextOptionsBuilder<CustomerDirectoryDataContext>()
              .UseInMemoryDatabase(databaseName: "Test")
              .Options;
            Context = new CustomerDirectoryDataContext(_contextOptions);
            Seed();
        }

        protected void Seed()
        {
            var customer1 = new Customer() { FirstName = "Rosie", LastName = "Rabbit", DateOfBirth = DateTime.Parse("2003-03-03") };
            var customer2 = new Customer() { FirstName = "Emily", LastName = "Elephant", DateOfBirth = DateTime.Parse("2005-03-03") };
            var customer3 = new Customer() { FirstName = "Zoe", LastName = "Zebra", DateOfBirth = DateTime.Parse("2007-03-03") };

            Context.AddRange(customer1, customer2, customer3);
            Context.SaveChanges();
        }
    }
}