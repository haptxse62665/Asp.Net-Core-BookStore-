using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ChoMoiApi.DTOs
{
    public class ErrorModel
    {
        //public List<string> Errors = new List<string>();
        //public bool IsEmpty
        //{
        //    get
        //    {
        //        return !this.Errors.Any();
        //    }
        //}

        //public void Add(string error)
        //{
        //    Errors.Add(error);
        //}

        public string Message { get; }
        public int StatusCode { get; }

        public ErrorModel(string message, HttpStatusCode statusCode)
        {
            Message = message;
            StatusCode = (int)statusCode;
        }
    }




}
