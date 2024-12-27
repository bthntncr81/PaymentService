using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GTBack.Core.Results
{
    public class DataResult<T> : Result, IDataResults<T>
    {
        public DataResult(T data, bool success, string? message, HttpStatusCode statusCode) : base(success, message, statusCode)
        {
            Data = data;
        }
        public DataResult(T data, bool success, HttpStatusCode statusCode) : base(success, statusCode)
        {
            Data = data;
        }
        public DataResult(T data, bool success) : base(success)
        {
            Data = data;
        }

        public DataResult(T data, bool success, string? message) : base(success, message)
        {
            Data = data;
        }

        public DataResult(T data, bool success, int totalCount) : base(success, totalCount)
        {
            Data = data;
        }

        public DataResult(T data, bool success, HttpStatusCode statusCode, Dictionary<string?, string?> modelStateERrors) : base(statusCode, modelStateERrors)
        {
            Data = data;
        }
        public T Data { get; }

    }
}
