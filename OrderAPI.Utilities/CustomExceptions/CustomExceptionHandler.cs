using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using OrderAPI.Utilities.CustomExceptions.CustomerExceptions;
using OrderAPI.Utilities.CustomExceptions.OrderExceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Metadata;
using System.Text;

namespace OrderAPI.Utilities.CustomExceptions
{
    public class CustomExceptionHandler : ExceptionFilterAttribute
    {
      
        public override void OnException(ExceptionContext context)
        {
        
            HttpStatusCode statusCode = error(context.Exception.GetType());

            string errorMessage = context.Exception.Message;

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)statusCode;
            response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(
                new
                {
                    errorMessage = errorMessage,
                    errorCode = statusCode,
                });
          
            response.ContentLength = result.Length;
            response.WriteAsync(result);
        }

        private HttpStatusCode error(Type exceptionType)
        {

            if (exceptionType.Name == typeof(DatabaseSaveException).Name)
                return HttpStatusCode.InternalServerError;

            if (exceptionType.Name == typeof(OrderNotExistException).Name)
                return HttpStatusCode.BadRequest;

            if (exceptionType.Name == typeof(OrderAlreadyExistException).Name)
                return HttpStatusCode.BadRequest;

            if (exceptionType.Name == typeof(InvalidOrderRequestDtoException).Name)
                return HttpStatusCode.BadRequest;

            if (exceptionType.Name == typeof(InvalidOrderBinWidthException).Name)
                return HttpStatusCode.BadRequest;

            if (exceptionType.Name == typeof(InvalidCustomerRequestDtoException).Name)
                return HttpStatusCode.BadRequest;

            if (exceptionType.Name == typeof(CustomerNotExistException).Name)
                return HttpStatusCode.BadRequest;

            if (exceptionType.Name == typeof(CustomerNotAuthenticated).Name)
                return HttpStatusCode.Forbidden;

            if (exceptionType.Name == typeof(CustomerAlreadyExistException).Name)
                return HttpStatusCode.BadRequest;

            return HttpStatusCode.InternalServerError;

        }
    }
}
