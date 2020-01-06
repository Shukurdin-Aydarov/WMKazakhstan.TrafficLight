using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

using WMKazakhstan.TrafficLight.Web.Models;

namespace WMKazakhstan.TrafficLight.Web.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch(Exception error)
            {
                await HandleErrorAsync(error, context);
            }
        }

        private Task HandleErrorAsync(Exception error, HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(new Error(error.Message)));
        }
    }
}
