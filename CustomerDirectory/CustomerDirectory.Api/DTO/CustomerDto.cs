using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;

namespace CustomerDirectory.DTO
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class CustomerValidator : AbstractValidator<CustomerDto>
    {
        public CustomerValidator(IHttpContextAccessor httpContextAccessor)
        {
            var httpMethod = httpContextAccessor.HttpContext.Request.Method;
            switch(httpMethod)
            {
                case "PUT":
                    RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage("Customer not found");
                    break;
                    
            }            
            RuleFor(x => x.FirstName).MaximumLength(20);
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Required First Name");
            RuleFor(x => x.LastName).MaximumLength(20);
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Required Last Name");
            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Required Birth Date");
            RuleFor(x => x.DateOfBirth).GreaterThan(DateTime.MinValue);
            RuleFor(x => x.DateOfBirth).LessThan(DateTime.MaxValue);
        }
    }
}