using System;

namespace Hosts.Domain.Exceptions
{
    public class UnexpectedResponseException : Exception
    {
        public UnexpectedResponseException(string service) : base($"Unexpected response from {service}")
        {

        }
    }
}