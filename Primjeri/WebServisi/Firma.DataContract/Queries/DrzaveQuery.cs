using CommandQueryCore;
using Firma.DataContract.DTOs;
using System.Collections.Generic;

namespace Firma.DataContract.Queries
{
  public class DrzaveQuery : IQuery<IEnumerable<DrzavaDto>>
  {
    public string SearchText { get; set; }
    public int? From { get; set; }
    public int? Count { get; set; }
    public SortInfo Sort { get; set; }   
  }
  
}
