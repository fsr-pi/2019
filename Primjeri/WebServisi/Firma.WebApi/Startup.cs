using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Swashbuckle.AspNetCore.Swagger;

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

      #region Swagger setup     
      services.AddSwaggerGen(c =>
      {
        //c.DocInclusionPredicate((name, apidescription) => apidescription.RelativePath.StartsWith($"api"));

        c.SwaggerDoc("v1", new Info
        {
          Title = "Firma.Mvc API",
          Description = "Jednostan primjer Web API-a nad državama",
          Version = "v1"
        });

        //Set the comments path for the swagger json and ui.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);

        //xmlFile = "Firma.Api.Dto.xml";
        //xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        //c.IncludeXmlComments(xmlPath);
      });


      #endregion
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      #region Swagger setup
      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
      // specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Firma.Mvc API V1");
        c.RoutePrefix = string.Empty;
      });      
      #endregion

      app.UseStaticFiles();
      app.UseMvc();
    }
  }
}
