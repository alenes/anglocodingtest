using System;
using System.Net;
using AA.CommoditiesDashboard.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AA.CommoditiesDashboard.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case ValidationException exception:
                    HandleException(context, HttpStatusCode.BadRequest, exception.Message);
                    break;
                default:
                    HandleException(context, HttpStatusCode.InternalServerError, "An unexpected error occurred");
                    break;
            }
        }
        
        private void HandleException(ExceptionContext context, HttpStatusCode statusCode, string errorMessage = null)
        {

            var json = new JsonErrorResponse
            {
                Messages = new[] { errorMessage ?? context.Exception.Message }
            };

            context.Result = new JsonResult(json)
            {
                StatusCode = (int)statusCode
            };
        
            context.ExceptionHandled = true;
        }

    }
}