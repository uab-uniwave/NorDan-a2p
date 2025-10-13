namespace a2p.Domain.Enums
{
    [Flags]
    public enum OrderState
    {
        None = 0,
        SalesDocumentExist = 1 << 0,
        A2PItemsImported = 1 << 1,
        A2PMaterialsImported = 1 << 2,
        ItemsCreated = 1 << 3,
        MaterialNeedsInserted = 1 << 4,
        PurchaseOrdersExist = 1 << 5
    }
}
