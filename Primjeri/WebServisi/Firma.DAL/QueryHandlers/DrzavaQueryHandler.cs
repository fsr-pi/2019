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

namespace Firma.DAL.QueryHandlers
{
  public class DrzavaQueryHandler : IDrzavaQueryHandler
  {
    private readonly FirmaContext ctx;
    private readonly IMapper mapper;

    public DrzavaQueryHandler(FirmaContext ctx, IMapper mapper)
    {
      this.ctx = ctx;
      this.mapper = mapper;
    }
    public IEnumerable<DrzavaDto> Handle(DrzavaQuery query)
    {
      List<DrzavaDto> list = new List<DrzavaDto>();
      var dbquery = ctx.Drzava
                     .AsNoTracking();
      foreach (var drzava in dbquery)
      {
        var dto = mapper.Map<Drzava, DrzavaDto>(drzava);
        list.Add(dto);
      }
      return list;
    }

    public Task<IEnumerable<DrzavaDto>> HandleAsync(DrzavaQuery query)
    {
      return Task.FromResult(Handle(query));
    }
  }
}
