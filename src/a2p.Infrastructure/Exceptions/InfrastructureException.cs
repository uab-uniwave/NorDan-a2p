namespace Infrastructure.Exceptions
{
    public abstract class InfrastructureException : System.Exception
    {

        protected InfrastructureException(string message) : base(message) { }

    }
}

