namespace a2p.Shared.Core.Entities.Models
{
    public class A2POrder
    {


        public string Order { get; set; } = string.Empty;
        public int SalesDocNumber { get; set; } = 0;
        public int SalesDocVersion { get; set; } = 0;
        public List<A2POrderFile> OrderFiles { get; set; } = [];




    }
}
