namespace a2p.Domain.Exceptions
{
    public abstract class DomainException : System.Exception
    {

        protected DomainException(string message) : base(message) { }

    }
}

