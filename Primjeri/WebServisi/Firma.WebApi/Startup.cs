using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Firma.DAL.Models;
using Firma.DAL.QueryHandlers;
using Firma.DataContract.QueryHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Firma.WebApi
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;     
    }
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {      
      services.AddMvc();
      string connectionString = Configuration.GetConnectionString("Firma");
      connectionString = connectionString.Replace("sifra", Configuration["FirmaSqlPassword"]);

      services.AddDbContext<FirmaContext>(options => options.UseSqlServer(connectionString));

      #region query handler setup
      //umjesto repetitvnog pisanja može se riješiti refleksijom tražeći i registrirajući sve handlere (klase koje implementiraju neki IQueryHandler<,>)
      //iz typeof(ArtiklQueryHandler).Assembly
      services.AddTransient<IDrzavaQueryHandler, DrzavaQueryHandler>();     
      #endregion

      services.AddAutoMapper(typeof(Startup));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseStaticFiles();
      app.UseMvc();
    }
  }
}
