using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GTBack.Core.Results
{
    public class ErrorDataResults<T> : DataResult<T>
    {
        public ErrorDataResults(T data) : base(data, false)
        {
        }

        public ErrorDataResults(T data, string? message, HttpStatusCode statusCode) : base(data, false, message,
            statusCode)
        {
        }


        public ErrorDataResults(string? message, HttpStatusCode statusCode) : base(default, false, message, statusCode)
        {
        }

        public ErrorDataResults() : base(default, false)
        {
        }

        public ErrorDataResults(string? message) : base(default, false, message)
        {
        }

        public ErrorDataResults(HttpStatusCode statusCode, Dictionary<string?, string?> errors) : base(default, false, statusCode, errors)
        {
        }
    }
}
