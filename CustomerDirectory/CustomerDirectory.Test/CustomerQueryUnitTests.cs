using CustomerDirectory.Query;
using System.Threading.Tasks;
using Xunit;

namespace CustomerDirectory.Test
{
    public class CustomerQueryUnitTests : UnitTestBase
    {
       
        [Fact]
        public async Task Can_get_Customers()
        {            
            _query = new CustomerQuery(Context);
            var items = await _query.Get();
            Assert.Contains(items, x => x.FirstName == "Rosie");
        }

        [Fact]
        public async Task Can_get_Customers_withExpression()
        {
            _query = new CustomerQuery(Context);
            var items = await _query.Get(x => x.FirstName == "Rosie");
            Assert.Contains(items, x => x.FirstName == "Rosie");
        }

        [Fact]
        public async Task Check_Customer_withId_Not_Exists()
        {
            _query = new CustomerQuery(Context);
            var exists = await _query.Exists(90);
            Assert.False(exists);
        }

        [Fact]
        public async Task Check_Customer_withId_Exists()
        {
            _query = new CustomerQuery(Context);
            var exists = await _query.Exists(3);
            Assert.True(exists);
        }
    }
}
