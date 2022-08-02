using System;

namespace FullCycle.Catalogo.Domain.Exceptions
{
    public class DomainEntityException : Exception
    {
        public DomainEntityException(string? message) : base(message)
        {

        }
    }
}
