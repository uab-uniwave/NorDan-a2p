namespace a2p.Infrastructure.Exceptions
{
    public sealed class ServiceException : InfrastructureException
    {
        public ServiceException(string OrderNumber)
            : base($"Service  cant processs '{OrderNumber}'")
        { }
    }

}
