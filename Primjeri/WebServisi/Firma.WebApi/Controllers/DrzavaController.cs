using AutoMapper;
using Firma.WebApi.Models;
using Firma.DataContract.DTOs;
using Firma.DataContract.QueryHandlers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Firma.WebApi.Models.DataTables;
using Firma.DataContract.Queries;
using FCD.Admin.Models.DataTables;

namespace Firma.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DrzavaController : ControllerBase
  {
    private const string GetDrzavaRouteName = "DohvatiDrzavu"; //potrebno kod CreatedAtRoute    
    private readonly IDrzavaQueryHandler queryHandler;
    private readonly IDrzavaCountQueryHandler drzavaCountQueryHandler;
    private readonly IMapper mapper;

    /// <summary>
    /// Konstruktor
    /// </summary>
    /// <param name="queryHandler">query handler za dohvat država</param>
    /// <param name="mapper">AutoMapper objekt za preslikavanje među objektima različitih klasa</param>
    public DrzavaController(IDrzavaQueryHandler queryHandler, IDrzavaCountQueryHandler drzavaCountQueryHandler, IMapper mapper)
    {            
      this.queryHandler = queryHandler;
      this.drzavaCountQueryHandler = drzavaCountQueryHandler;
      this.mapper = mapper;
    }

    // GET: api/drzava
    /// <summary>
    /// Postupak za dohvat svih država. 
    /// </summary>
    /// <returns>Popis svih država sortiran po nazivu država</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Drzava>), (int)HttpStatusCode.OK)]
    public async Task<DTResponse> Get(DTRequest request)
    {
      var query = new DrzavaQuery();
      if (request.Search != null)
      {
        query.SearchText = request.Search.Value.Trim();
      }

      SortInfo sortInfo = request.GetSortInfo();
      if (sortInfo != null && sortInfo.ColumnOrder != null && sortInfo.ColumnOrder.Count > 0)
      {
        var keyMappings = new Dictionary<string, string> {
          { nameof(Drzava.Iso3drzave).ToLower(), nameof(DrzavaDto.Iso3drzave) },
          { nameof(Drzava.NazDrzave).ToLower(), nameof(DrzavaDto.NazDrzave) },
          { nameof(Drzava.SifDrzave).ToLower(), nameof(DrzavaDto.SifDrzave) },
          { nameof(Drzava.OznDrzave).ToLower(), nameof(DrzavaDto.OznDrzave) }
        };
        query.Sort = new SortInfo();

        foreach (var sort in sortInfo.ColumnOrder)
        {
          if (keyMappings.TryGetValue(sort.Key.ToLower(), out string dtokey))
          {
            query.Sort.ColumnOrder.Add(new KeyValuePair<string, SortInfo.Order>(dtokey, sort.Value));
          }
        }
      }

      query.From = request.Start;
      query.Count = request.Length;
     
      List<Drzava> list = new List<Drzava>();
      var data = queryHandler.Handle(query);
      foreach (var drzava in data)
      {
        list.Add(mapper.Map<DrzavaDto, Drzava>(drzava));
      }

      int totalRecords = await drzavaCountQueryHandler.HandleAsync(new DrzavaCountQuery());
      int totalFiltered = await drzavaCountQueryHandler.HandleAsync(new DrzavaCountQuery() { SearchText = query.SearchText });
      var response = new DTResponse()
      {
        recordsFiltered = totalFiltered,
        recordsTotal = totalRecords,
        data = list.ToArray(),
        draw = request.Draw
      };

      

      return response;
    }
/*
    // GET api/drzava/HR
    /// <summary>
    /// Postupak za dohvat države čija je oznaka jednaka poslanom parametru
    /// </summary>
    /// <param name="oznDrzave">oznaka države</param>
    /// <returns>objekt tipa Firma.Api.Dto.Drzava ili NotFound ako država s traženom oznakom ne postoji</returns>
    [HttpGet("{oznDrzave}", Name = GetDrzavaRouteName)]
    [ProducesResponseType(typeof(Firma.Api.Dto.Drzava), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Get(string oznDrzave)
    {
      var drzava = await ctx.Drzava
                            .AsNoTracking()
                            .Where(d => d.OznDrzave == oznDrzave)
                            .FirstOrDefaultAsync();
      if (drzava == null)
      {
        return NotFound("Tražena država ne postoji");
      }
      else
      {
        var model = new Firma.Api.Dto.Drzava
        {
          SifDrzave = drzava.SifDrzave,
          OznDrzave = drzava.OznDrzave,
          Iso3drzave = drzava.Iso3drzave,
          NazDrzave = drzava.NazDrzave
        };
        return Ok(model);
      }
    }

    // POST api/drzava
    /// <summary>
    /// Postupak kojim se unosi nova država
    /// </summary>
    /// <remarks>
    /// Primjer poziva:
    ///
    ///     POST /Todo
    ///     {    
    ///        "oznDrzave": "A1",
    ///        "nazDrzave" : "Testna država A1",
    ///        "iso3drzave": "123",
    ///        "sifDrzave": 1234
    ///        
    ///     }
    ///
    /// </remarks>
    /// <param name="model">Podaci o novoj državi</param>
    /// <returns>201 ako je država uspješno pohranjena, te se ujedno vraća i spremljeni objekt, 400 u slučaju neispravnog modela</returns>
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create([FromBody] Firma.Api.Dto.Drzava model)
    {
      if (model != null && ModelState.IsValid)
      {
        Drzava drzava = new Drzava
        {
          SifDrzave = model.SifDrzave,
          OznDrzave = model.OznDrzave,
          Iso3drzave = model.Iso3drzave,
          NazDrzave = model.NazDrzave
        };

        //ovdje bi slijedila validacija u poslovnom sloju prije snimanja (da smo napravili takav sloj)
        ctx.Add(drzava);
        await ctx.SaveChangesAsync();

        return CreatedAtRoute(GetDrzavaRouteName, new { oznDrzave = drzava.OznDrzave }, model);
      }
      else
      {
        return BadRequest(ModelState);
      }

    }

    // PUT api/drzava
    /// <summary>
    /// Ažurira državu s oznakom države oznDrzave temeljem parametera iz zahtjeva
    /// </summary>
    /// <param name="oznDrzave">oznaka države koju treba ažurirati</param>
    /// <param name="model">model s podacima o državi</param>
    /// <returns>204 ako je država uspješno ažurirana, 404 ako država s traženom šifrom ne postoji ili 400 ako model nije ispravan</returns>
    [HttpPut("{oznDrzave}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(string oznDrzave, [FromBody] Firma.Api.Dto.Drzava model)
    {
      if (model == null || model.OznDrzave != oznDrzave || !ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      else
      {
        var drzava = await ctx.Drzava.FindAsync(oznDrzave);
        if (drzava == null)
        {
          return NotFound("Tražena država ne postoji");
        }
        else
        {
          drzava.SifDrzave = model.SifDrzave;
          drzava.Iso3drzave = model.Iso3drzave;
          drzava.NazDrzave = model.NazDrzave;
          await ctx.SaveChangesAsync();
          return NoContent();
        };
      }
    }

    // DELETE api/drzava/HR
    /// <summary>
    /// briše državu s oznakom predanom u adresi zahtjeva
    /// </summary>
    /// <param name="oznDrzave">oznaka države koju treba obrisati</param>
    /// <returns>404 ili 204 ako je brisanje uspješno</returns>          
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [HttpDelete("{oznDrzave}")]

    public async Task<IActionResult> Delete(string oznDrzave)
    {
      var drzava = await ctx.Drzava.FindAsync(oznDrzave);
      if (drzava == null)
      {
        return NotFound("Tražena država ne postoji");
      }
      else
      {
        ctx.Remove(drzava);
        await ctx.SaveChangesAsync();
        return NoContent();
      };
    }
    */
  }
}
