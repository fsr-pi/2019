using System;
using System.Collections.Generic;
using System.Text;

namespace Firma.DataContract.Queries
{
  public class SortInfo
  {
    public enum Order
    {
      ASCENDING, DESCENDING
    }
    /// <summary>
    /// Pair columnName, order
    /// </summary>
    public List<KeyValuePair<string, Order>> ColumnOrder { get; set; } = new List<KeyValuePair<string, Order>>();
  }
}
