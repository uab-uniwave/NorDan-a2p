using a2p.Shared.Core.DTO;


namespace a2p.Shared.Core.Interfaces.Repository.SubSql
{
 public interface ISqlSapa_v2
 {
  //Task<string> GetColorAsync(string color);
  //Task<int> InsertItemAsync(ItemDTO position);

  Task<int> ReadMaterialAsync(MaterialDTO material);
  //Task<int> InsertGlassAsync(GlassDTO glass);

  //Task<int> InsertPanelAsync(PanelDTO panel);


 }


}
