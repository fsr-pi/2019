using CommandQueryCore;
using Firma.DataContract.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firma.DataContract.Queries
{
  public class DrzavaCountQuery : IQuery<int>
  {
    public string SearchText { get; set; }
  }
  
}
