using System;

namespace Hosts.Domain.Exceptions
{
    //Comment: this should be a custom Exception Type and handled in a middleware to return a custom response 
    public class UnexpectedResponseException : Exception
    {
        public UnexpectedResponseException(string service) : base($"Unexpected response from {service}")
        {

        }
    }
}