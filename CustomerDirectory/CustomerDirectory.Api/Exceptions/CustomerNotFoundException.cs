using System;

namespace CustomerDirectory.Exceptions
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException(string message) : base(message)  { }
    }
}
