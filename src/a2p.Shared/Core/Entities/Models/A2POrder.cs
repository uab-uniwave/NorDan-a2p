using System.ComponentModel.DataAnnotations;

namespace a2p.Shared.Core.Entities.Models
{
    public class A2POrder
    {



        //===============================================
        [Required] required public string Order { get; set; }
        public int SalesDocNumber { get; set; }
        public int SalesDocVersion { get; set; }
        public string? Currency { get; set; }

        public decimal ExchangeRate { get; set; }

        public DateTime? OrderExists { get; set; } = null;
        public bool OverwriteOrder { get; set; } = false;

        public List<A2POrderError> ReadErrors { get; set; } = [];
        public List<A2POrderError> WriteErrors { get; set; } = [];

        public List<A2PFile> Files { get; set; } = [];




    }
}
