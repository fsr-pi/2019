using CommandQueryCore;
using Firma.DataContract.DTOs;
using Firma.DataContract.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firma.DataContract.QueryHandlers
{
  public interface IDrzavaCountQueryHandler : IQueryHandler<DrzavaCountQuery, int>
  {
  }
}
