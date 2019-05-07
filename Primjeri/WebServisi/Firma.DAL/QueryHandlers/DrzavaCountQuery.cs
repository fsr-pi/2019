using AutoMapper;
using Firma.DAL.Models;
using Firma.DataContract.DTOs;
using Firma.DataContract.Queries;
using Firma.DataContract.QueryHandlers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Firma.DAL.QueryHandlers
{
  public class DrzavaCountQueryHandler : IDrzavaCountQueryHandler
  {
    private readonly FirmaContext ctx;    

    public DrzavaCountQueryHandler(FirmaContext ctx)
    {
      this.ctx = ctx;      
    }

    public int Handle(DrzavaCountQuery query)
    {
      IQueryable<Drzava> dbquery = PrepareDbQuery(query);
      return dbquery.Count();
    }

    public async Task<int> HandleAsync(DrzavaCountQuery query)
    {
      IQueryable<Drzava> dbquery = PrepareDbQuery(query);

      int count = await dbquery.CountAsync();
      return count;
    }

    private IQueryable<Drzava> PrepareDbQuery(DrzavaCountQuery query)
    {
      var dbquery = ctx.Drzava
                             .AsNoTracking();
      if (!string.IsNullOrWhiteSpace(query.SearchText))
      {
        dbquery = dbquery.Where(d => d.NazDrzave.Contains(query.SearchText)
                                  || d.Iso3drzave.Contains(query.SearchText)
                                  || d.SifDrzave.ToString().Contains(query.SearchText)
                                  || d.OznDrzave.Contains(query.SearchText));
      }

      return dbquery;
    }
  }
}
