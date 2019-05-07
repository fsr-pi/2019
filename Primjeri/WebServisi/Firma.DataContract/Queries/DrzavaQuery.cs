using CommandQueryCore;
using Firma.DataContract.DTOs;

namespace Firma.DataContract.Queries
{
  public class DrzavaQuery : IQuery<DrzavaDto>
  {
    public DrzavaQuery(string oznDrzave)
    {
      OznDrzave = oznDrzave;
    }    

    public string OznDrzave { get; set; }   
  }
  
}
