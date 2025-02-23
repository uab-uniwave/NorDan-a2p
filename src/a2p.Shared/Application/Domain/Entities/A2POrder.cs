using a2p.Shared.Application.DTO;
using a2p.Shared.Application.Services.Domain.Entities;
using a2p.Shared.Domain.Enums;

using System.ComponentModel.DataAnnotations;

namespace a2p.Shared.Domain.Entities
{
    public class A2POrder
    {



        //===============================================
        [Required] required public string Order { get; set; }
        //===================================================================================
        public SourceAppType SourceAppType { get; set; } = SourceAppType.Unknown;
        //===================================================================================
        public DateTime? OrderExists { get; set; } = null;
        public bool OverwriteOrder { get; set; } = false;
        public List<A2PFile> Files { get; set; } = new List<A2PFile>();
        //===================================================================================
        public List<ItemDTO> Items { get; set; } = new List<ItemDTO>();
        public List<MaterialDTO> Materials { get; set; } = new List<MaterialDTO>();
        //===================================================================================
        public List<A2POrderError> ReadErrors { get; set; } = new List<A2POrderError>();  
        public List<A2POrderError> WriteErrors { get; set; } = new List<A2POrderError>();
        //===================================================================================
        public int SalesDocNumber { get; set; }
        public int SalesDocVersion { get; set; }
        //===================================================================================
        public string? Currency { get; set; }
        public decimal ExchangeRate { get; set; }
        

    }
}
