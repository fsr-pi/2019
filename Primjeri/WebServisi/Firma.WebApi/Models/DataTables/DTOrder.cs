using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firma.WebApi.Models.DataTables
{
  /// <summary>
  /// Represents a column and it's order direction
  /// </summary>
  public sealed class DTOrder
  {
    /// <summary>
    /// Column to which ordering should be applied. This is an index reference to the 
    /// columns array of information that is also submitted to the server
    /// </summary>
    public int Column { get; set; }

    /// <summary>
    /// Ordering direction for this column. It will be asc or desc to indicate ascending
    /// ordering or descending ordering, respectively
    /// </summary>
    public string Dir { get; set; }
  }
}
