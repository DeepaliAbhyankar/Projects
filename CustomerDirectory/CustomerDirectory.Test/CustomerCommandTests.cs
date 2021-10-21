using CustomerDirectory.Command;
using CustomerDirectory.Data;
using CustomerDirectory.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CustomerDirectory.Test
{
    public class CustomerCommandUnitTests : UnitTestBase
    {
        public CustomerCommandUnitTests()
        {
            Initialize();
        }

        [Fact]
        public async Task Can_Add_Customer()
        {
            var customer = new Customer() { FirstName = "Samwise", LastName = "Hobbit", DateOfBirth = DateTime.Parse("1999-06-01") };
            await _command.Add(customer);
            var items = await _query.Get();
            Assert.Contains(items, x => x.FirstName == "Samwise");
        }       

        [Fact]
        public async Task Can_Delete_Customer()
        {
            var items = await _query.Get(x => x.LastName == "Rabbit");
            await _command.Delete(items.First());
            var remainingItems = await _query.Get();
            Assert.NotEqual(remainingItems, items);           
        }

        [Fact]
        public async Task Can_Bulk_Add_Customer()
        {
            var itemsBeforeBulk = await _query.Get();

            var customer1 = new Customer() { FirstName = "Gilmil", LastName = "Dwarf", DateOfBirth = DateTime.Parse("1999-06-01") };
            var customer2 = new Customer() { FirstName = "Legolas", LastName = "Elf", DateOfBirth = DateTime.Parse("1999-06-01") };
            var customer3 = new Customer() { FirstName = "Gendalf", LastName = "Wizard", DateOfBirth = DateTime.Parse("1999-06-01") };
            var customers = new List<Customer>
            {
                customer1,
                customer2,
                customer3
            };
            await _command.BulkAdd(customers);
            var itemsAfterBulk = await _query.Get();
            Assert.Contains(itemsAfterBulk, x => x.LastName == "Wizard");
        }

        [Fact]
        public async Task Can_Edit_Customer()
        {
            var items = await _query.Get(x => x.LastName == "Zebra");
            var customer = items.First();
            customer.LastName = "Giraffe";
            await _command.Update(customer);
            var editedItem = (await _query.Get(x => x.LastName == "Giraffe")).First();
            Assert.Equal(customer.CustomerId, editedItem.CustomerId);
         }

        private void Initialize()
        {
            _query = new CustomerQuery(Context);
            var userProvider = new Mock<IUserProvider>();
            _command = new CustomerCommand(Context, userProvider.Object);
        }
    }
}
