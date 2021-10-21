using CustomerDirectory.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CustomerDirectory.Query
{
    public interface ICustomerQuery
    {
        Task<List<Customer>> Get();
        Task<List<Customer>> Get(Expression<Func<Customer, bool>> customer);
        Task<bool> Exists(int customerId);
    }
    public class CustomerQuery : ICustomerQuery
    {
        private readonly CustomerDirectoryDataContext _context;
        public CustomerQuery(CustomerDirectoryDataContext context)
        {
            _context = context;
        }

        public async Task<bool> Exists(int customerId)
        {
            var customerCount =  await _context.Customers
                        .Where(x => x.CustomerId == customerId)
                        .AsNoTracking()
                        .CountAsync();
            return customerCount > 0;
        }

        public async Task<List<Customer>> Get()
        {
            return await _context.Customers
                         .AsNoTracking()
                         .ToListAsync();
        }

        public async Task<List<Customer>> Get(Expression<Func<Customer, bool>> predicate)
        {
            var items = await _context.Customers                             
                             .Where(predicate)
                             .AsNoTracking()
                             .ToListAsync();
            return items;
                           
        }
    }

    
}
