using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerDirectory.Data
{   
    public class Customer
    {
        public int CustomerId { get; set; }         
        public string FirstName { get; set; }         
        public string LastName { get; set; }       
        public DateTime DateOfBirth { get; set; }      
        public string CreatedBy { get; set; }       
        public string ModifiedBy { get; set; }        
        public DateTime CreatedDate { get; set; }     
        public DateTime ModifiedDate { get; set; } 
    }
}
