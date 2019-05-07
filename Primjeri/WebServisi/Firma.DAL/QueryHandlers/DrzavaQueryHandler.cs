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
  public class DrzavaQueryHandler : IDrzavaQueryHandler
  {
    private readonly FirmaContext ctx;
    private readonly IMapper mapper;

    public DrzavaQueryHandler(FirmaContext ctx, IMapper mapper)
    {
      this.ctx = ctx;
      this.mapper = mapper;
    }
    public DrzavaDto Handle(DrzavaQuery query)
    {
      var drzava = ctx.Drzava.Find(query.OznDrzave);

      if (drzava != null)
      {
        var dto = mapper.Map<Drzava, DrzavaDto>(drzava);
        return dto;
      }
      else
      {
        return null;
      }     
    }

    public async Task<DrzavaDto> HandleAsync(DrzavaQuery query)
    {
      var drzava = await ctx.Drzava.FindAsync(query.OznDrzave);

      if (drzava != null)
      {
        var dto = mapper.Map<Drzava, DrzavaDto>(drzava);
        return dto;
      }
      else
      {
        return null;
      }
    }    
  }
}
