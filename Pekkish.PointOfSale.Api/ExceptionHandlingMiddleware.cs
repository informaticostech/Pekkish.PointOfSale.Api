using Microsoft.Extensions.Options;
using Pekkish.PointOfSale.Api.Models.Wati;
using Pekkish.PointOfSale.Api.Services;
using System;
using System.Net;
using System.Text.Json;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Pekkish.PointOfSale.Api
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IEmailSender _emailSender;
        private readonly EmailSetting _emailSetting;
        
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IEmailSender emailSender, IOptions<EmailSetting> emailSetting)
        {
            _next = next;
            _logger = logger;
            _emailSender = emailSender;
            _emailSetting = emailSetting.Value;            
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new ErrorResponse
            {
                Success = false
            };

            switch (exception)
            {
                case ApplicationException ex:
                    if (ex.Message.Contains("Invalid Token"))
                    {
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        errorResponse.Message = ex.Message;
                        break;
                    }
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "Internal server error!";
                    break;
            }
            
            _logger.LogError(exception.Message);
            
            var result = JsonSerializer.Serialize(errorResponse);

            await context.Response.WriteAsync(result);

            //Eamil Error
            var bodyHTML = $"Error occured in Pekkish.PointOfSale.Api";
            bodyHTML += "<br/>";
            bodyHTML += "<br/>";
            bodyHTML += $"<p>Error: {exception.Message}s</p>";
            bodyHTML += "<br/>";
            bodyHTML += "<br/>";
            bodyHTML += exception.StackTrace;

            
            await _emailSender.SendEmailAsync(_emailSetting.ToEmail, "Pekkish POS API Error", bodyHTML);
        }
    }
    public class ErrorResponse
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = "";

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}