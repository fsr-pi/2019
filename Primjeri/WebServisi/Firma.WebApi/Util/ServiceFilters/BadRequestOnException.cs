using Firma.WebApi.Util.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FCD.WebApi.Util.ServiceFilters
{
  public class BadRequestOnException : ExceptionFilterAttribute
  {
    private readonly ILogger<BadRequestOnException> logger;

    public BadRequestOnException(ILogger<BadRequestOnException> logger)
    {
      this.logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
      logger.LogError("Neobrađena iznimka u upravljaču: {0}", context.Exception.CompleteExceptionMessage());
      context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;           
      context.ExceptionHandled = true;
      context.Result = new JsonResult(context.Exception.CompleteExceptionMessage());
      base.OnException(context);
    }   
  }
}
