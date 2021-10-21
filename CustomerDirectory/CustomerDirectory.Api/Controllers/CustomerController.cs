using CustomerDirectory.DTO;
using CustomerDirectory.Exceptions;
using CustomerDirectory.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerDirectory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly ICustomerRepository _customerRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CustomerController(ICustomerRepository customerRepo, IHttpContextAccessor httpContextAccessor)
        {
            _customerRepo = customerRepo;
            _httpContextAccessor = httpContextAccessor;
        }
       
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var customers = await _customerRepo.Get();
                return Ok(customers);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        
        [HttpGet("{customerId}")]
        public async Task<IActionResult> Get(int customerId)
        {
            try
            {
                var customers = await _customerRepo.Get(customerId);
                return Ok(customers);
            }
            catch(CustomerNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Search/{searchText}")]
        public async Task<IActionResult> Search(string searchText)
        {
            try
            {
                var customers = await _customerRepo.Search(searchText);
                return Ok(customers);
            }
            catch (CustomerNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]       
        public async Task<IActionResult> Add([FromBody] CustomerDto customerToAdd)
        {
            try
            {
                var validator = new CustomerValidator(_httpContextAccessor);
                var result = validator.Validate(customerToAdd);
                if(!result.IsValid)
                {                    
                    return BadRequest(result.Errors);
                }
                await _customerRepo.Add(customerToAdd);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Bulk")]
        public async Task<IActionResult> BulkAdd([FromBody] List<CustomerDto> customersToAdd)
        {
            try
            {              
                await _customerRepo.BulkAdd(customersToAdd);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] CustomerDto customerToUpdate)
        {
            try
            {
                var validator = new CustomerValidator(_httpContextAccessor);
                var result = validator.Validate(customerToUpdate);
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }
                await _customerRepo.Update(customerToUpdate);
                return Ok();
            }
            catch (CustomerNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
      
        [HttpDelete("{customerId}")]
        public async Task<IActionResult> Delete(int customerId)
        {
            try
            {
                await _customerRepo.Delete(customerId);
                return Ok();
            }
            catch(CustomerNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }      
    }
}
