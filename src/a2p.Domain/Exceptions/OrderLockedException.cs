namespace a2p.Domain.Exceptions
{
    public sealed class OrderExistsException : DomainException
    {
        public OrderExistsException(string orderNumber, int salesdDcumentNumber, int slesDocumentVersion)
            : base($"Order '{orderNumber}'already assigned to another Sales Document '{salesdDcumentNumber}/{salesdDcumentNumber}' in PrefSuite")
        { }
    }

}
