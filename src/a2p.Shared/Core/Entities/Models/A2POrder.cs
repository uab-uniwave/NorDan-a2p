namespace a2p.Shared.Core.Entities.Models
{
    public class A2POrder
    {


        public string OrderNumber { get; set; } = string.Empty;
        public int SalesDocNumber { get; set; } = 0;
        public int SalesDocVersion { get; set; } = 0;
        public string OrderCurrency { get; set; } = string.Empty;
        public List<A2POrderFile> OrderFiles { get; set; } = [];




    }
}
