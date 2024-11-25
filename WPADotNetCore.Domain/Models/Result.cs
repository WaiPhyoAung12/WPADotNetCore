using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPADotNetCore.Domain.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public bool IsError { get { return !IsSuccess; } }
        public EnumRespType Type { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }

        public static Result<T> Success(string message,T data)
        {
            return new Result<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                Type = EnumRespType.Success,
            };
        }
        public static Result<T> Error(string message, T? data=default)
        {
            return new Result<T>
            {
                IsSuccess = false,
                Message = message,
                Data = data,
                Type = EnumRespType.Error,
            };
        }
        public static Result<T> ValidationError(string message, T? data = default)
        {
            return new Result<T>
            {
                IsSuccess = false,
                Message = message,
                Data = data,
                Type = EnumRespType.ValidationError,
            };
        }
        public static Result<T> SystemError(string message, T? data = default)
        {
            return new Result<T>
            {
                IsSuccess = false,
                Message = message,
                Data = data,
                Type = EnumRespType.SystemError,
            };
        }
    }
}
