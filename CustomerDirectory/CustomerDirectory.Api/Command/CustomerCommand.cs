using CustomerDirectory.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerDirectory.Command
{
    public interface ICustomerCommand
    {
        Task Add(Customer customer);
        Task Delete(Customer customer);
        Task Update(Customer customer);
        Task BulkAdd(List<Customer> customers);
    }
    public class CustomerCommand : ICustomerCommand
    {
        private readonly CustomerDirectoryDataContext _context;
        private readonly IUserProvider _userProvider;
        public CustomerCommand(CustomerDirectoryDataContext context, IUserProvider userProvider)
        {
            _context = context;
            _userProvider = userProvider;
        }
        public async Task Add(Customer customer)
        {
           _context.Customers.Add(customer);           
          await _context.SaveChangesAsync(_userProvider.Username);          
        }

        public async Task BulkAdd(List<Customer> customers)
        {
            _context.Customers.AddRange(customers);
            await _context.SaveChangesAsync(_userProvider.Username);
        }

        public async Task Delete(Customer customer)
        {
            _context.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync(_userProvider.Username);
        }
    }
}
