namespace a2p.Domain.Exception
{
    public sealed class OrderLockedException : DomainException
    {
        public OrderLockedException(string OrderNumber)
            : base($"Order '{OrderNumber}' ia locked, please ensure that no prchase orders created in PrefSuite ")
        { }
    }

}
