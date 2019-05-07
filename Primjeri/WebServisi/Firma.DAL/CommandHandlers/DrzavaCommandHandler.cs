using AutoMapper;
using CommandQueryCore;
using Firma.DAL.Models;
using Firma.DataContract.Commands;
using System;
using System.Threading.Tasks;

namespace Firma.DAL.CommandHandlers
{
  public class DrzavaCommandHandler : ICommandHandler<DeleteDrzava>, ICommandHandler<AddDrzava>, ICommandHandler<UpdateDrzava>
  {
    private readonly FirmaContext ctx;
    private readonly IMapper mapper;

    public DrzavaCommandHandler(FirmaContext ctx, IMapper mapper)
    {
      this.ctx = ctx;
      this.mapper = mapper;
    }   

    public async Task HandleAsync(DeleteDrzava command)
    {
      var drzava = await ctx.Drzava.FindAsync(command.OznDrzave);
      if (drzava != null)
      {
        ctx.Entry(drzava).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        await ctx.SaveChangesAsync();
      }
    }

    public async Task HandleAsync(AddDrzava command)
    {
      var drzava = new Drzava
      {
        Iso3drzave = command.Iso3drzave,
        NazDrzave = command.NazDrzave,
        OznDrzave = command.OznDrzave,
        SifDrzave = command.SifDrzave
      };
      ctx.Add(drzava);
      await ctx.SaveChangesAsync();
    }

    public async Task HandleAsync(UpdateDrzava command)
    {
      var drzava = await ctx.Drzava.FindAsync(command.OznDrzave);
      if (drzava != null)
      {
        drzava.Iso3drzave = command.Iso3drzave;
        drzava.NazDrzave = command.NazDrzave;
        drzava.SifDrzave = command.SifDrzave;
        await ctx.SaveChangesAsync();
      }
      else 
        throw new ArgumentException($"Nepostojeća oznaka država: {command.OznDrzave}");
    }
  }
}
