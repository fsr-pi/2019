
using Firma.DataContract.Queries;
using Firma.WebApi.Models.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCD.Admin.Models.DataTables
{
  public static class SortAndFilterExtractor
  {
    public static SortInfo GetSortInfo(this DTRequest model)
    {
      SortInfo info = null;
      if (model.Order != null && model.Order.Count > 0)
      {
        info = new SortInfo();        
        foreach (var orderItem in model.Order)
        {
          int columnpos = orderItem.Column;
          bool descending = orderItem.Dir.Equals("desc", StringComparison.OrdinalIgnoreCase);

          if (model.Columns.Count > columnpos)
          {
            var column = model.Columns[columnpos];
            if (column.Orderable)
            {
              info.ColumnOrder.Add(new KeyValuePair<string, SortInfo.Order>(column.Data,
                descending ? SortInfo.Order.DESCENDING : SortInfo.Order.ASCENDING));
            }
          }
        }
      }

      return info;
    }
  }
}
