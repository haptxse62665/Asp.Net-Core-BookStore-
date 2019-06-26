using ChoMoiApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChoMoiApi.Infrastructure
{
    public class ErrorActionResult: IActionResult
    {
        private HttpResponseMessage _response;

        public ErrorActionResult(HttpRequestMessage request, HttpStatusCode statusCode, ErrorModel errors)
        {
            string content = "{Errors: [" + string.Join(", ", errors.Message) + "]}";
            StringContent tmp = new StringContent(content);
            _response = request.CreateResponse(statusCode);
            _response.Content = tmp;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            return Task.FromResult(_response);
        }

        public HttpResponseMessage GetResponse()
        {
            return _response;
        }
    }
}
