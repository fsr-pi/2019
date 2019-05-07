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
using System.Linq.Expressions;

namespace Firma.DAL.QueryHandlers
{
  public class DrzaveQueryHandler : IDrzaveQueryHandler
  {
    private readonly FirmaContext ctx;
    private readonly IMapper mapper;

    public DrzaveQueryHandler(FirmaContext ctx, IMapper mapper)
    {
      this.ctx = ctx;
      this.mapper = mapper;
    }
    public IEnumerable<DrzavaDto> Handle(DrzaveQuery query)
    {
      List<DrzavaDto> list = new List<DrzavaDto>();
      IQueryable<Drzava> dbquery = PrepareDbQuuery(query);

      foreach (var drzava in dbquery)
      {
        var dto = mapper.Map<Drzava, DrzavaDto>(drzava);
        list.Add(dto);
      }
      return list;
    }

    public async Task<IEnumerable<DrzavaDto>> HandleAsync(DrzaveQuery query)
    {
      List<DrzavaDto> list = new List<DrzavaDto>();
      IQueryable<Drzava> dbquery = PrepareDbQuuery(query);

      await dbquery.ForEachAsync(drzava =>
      {
        var dto = mapper.Map<Drzava, DrzavaDto>(drzava);
        list.Add(dto);
      });

      return list;
    }

    private IQueryable<Drzava> PrepareDbQuuery(DrzaveQuery query)
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

      if (query.Sort != null && query.Sort.ColumnOrder.Count > 0)
      {
        var first = query.Sort.ColumnOrder[0];
        var orderSelector = GetOrderSelector(first.Key);
        if (orderSelector != null)
        {
          var orderedQuery = first.Value == SortInfo.Order.ASCENDING ?
                       dbquery.OrderBy(orderSelector) :
                       dbquery.OrderByDescending(orderSelector);

          for (int i = 1; i < query.Sort.ColumnOrder.Count; i++)
          {
            var sort = query.Sort.ColumnOrder[i];
            orderSelector = GetOrderSelector(sort.Key);
            if (orderSelector != null)
            {
              orderedQuery = sort.Value == SortInfo.Order.ASCENDING ?
                                 orderedQuery.ThenBy(orderSelector) :
                                 orderedQuery.ThenByDescending(orderSelector);
            }
          }
          dbquery = orderedQuery;
        }
      }

      if (query.From.HasValue)
      {
        dbquery = dbquery.Skip(query.From.Value);
      }
      if (query.Count.HasValue)
      {
        dbquery = dbquery.Take(query.Count.Value);
      }
      return dbquery;
    }  

    private Expression<Func<Drzava, object>> GetOrderSelector(string columnName) {
      Expression<Func<Drzava, object>> orderSelector = null;
      switch (columnName) {
        case nameof(DrzavaDto.SifDrzave):
          orderSelector = d => d.SifDrzave;
          break;
        case nameof(DrzavaDto.OznDrzave):
          orderSelector = d => d.OznDrzave;
          break;
        case nameof(DrzavaDto.NazDrzave):
          orderSelector = d => d.NazDrzave;
          break;
        case nameof(DrzavaDto.Iso3drzave):
          orderSelector = d => d.Iso3drzave;
          break;
      }
      return orderSelector;
    }
  }
}
