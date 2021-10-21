using AutoMapper;
using CustomerDirectory.Command;
using CustomerDirectory.Data;
using CustomerDirectory.DTO;
using CustomerDirectory.Exceptions;
using CustomerDirectory.Query;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerDirectory.Repository
{
    public interface ICustomerRepository
    {
        Task<bool> Add(CustomerDto customerToAdd);
        Task<bool> BulkAdd(List<CustomerDto> customersToAdd);
        Task<List<CustomerDto>> Get();
        Task<CustomerDto> Get(int customerId);
        Task<List<CustomerDto>> Search(string searchText);
        Task<bool> Delete(int customerId);
        Task<bool> Update(CustomerDto customerToUpdate);
    }
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ICustomerCommand _customerCommand;
        private readonly ICustomerQuery _customerQuery;
        private readonly IMapper _mapper;
        public CustomerRepository(ICustomerCommand customerCommand, IMapper mapper, ICustomerQuery customerQuery)
        {
            _customerCommand = customerCommand;
            _customerQuery = customerQuery;
            _mapper = mapper;
        }
        public async Task<bool> Add(CustomerDto customerToAdd)
        {
            var customer = _mapper.Map<Customer>(customerToAdd);
            await  _customerCommand.Add(customer);
            return true;
        }
        public async Task<bool> BulkAdd(List<CustomerDto> customersToAdd)
        {

            var customers = _mapper.Map<List<Customer>>(customersToAdd);
            await _customerCommand.BulkAdd(customers);
            return true;
        }
        public async Task<bool> Update(CustomerDto customerToUpdate)
        {
            var customerExists = await _customerQuery.Exists(customerToUpdate.CustomerId);
            if(!customerExists)
            {
                throw new CustomerNotFoundException($"Customer with id : {customerToUpdate.CustomerId} does not exists");
            }
            var customer = _mapper.Map<Customer>(customerToUpdate);           
            await _customerCommand.Update(customer);
            return true;
        }

        public async Task<bool> Delete(int customerId)
        {
            var customer = await GetCustomer(customerId);            
            await _customerCommand.Delete(customer);
            return true;
        }

        public async Task<List<CustomerDto>> Get()
        {
            var items = await _customerQuery.Get();
            return _mapper.Map<List<CustomerDto>>(items);
        }

        public async Task<CustomerDto> Get(int customerId)
        {
            var customer = await GetCustomer(customerId);
            return _mapper.Map<CustomerDto>(customer);
        }
       
        public async Task<List<CustomerDto>> Search(string searchText)
        {
            var wildcardSearchText = $"%{searchText}%";
            var searchExpression = PredicateBuilder.New<Customer>(x => EF.Functions.Like(x.FirstName, wildcardSearchText));
            searchExpression.Or(x => EF.Functions.Like(x.LastName, wildcardSearchText));
            var items = await _customerQuery.Get(searchExpression);
            if (!items.Any())
            {
                throw new CustomerNotFoundException($"Customer with name : {searchText} does not exists");
            }
            return _mapper.Map<List<CustomerDto>>(items);
        }

       

        #region Private
        private async Task<Customer> GetCustomer(int customerId)
        {
            var items = await _customerQuery.Get(x => x.CustomerId == customerId);
            if (!items.Any())
            {
                throw new CustomerNotFoundException($"Customer with id : {customerId} does not exists");
            }
            return items.First();
        }     

        #endregion
    }
}
