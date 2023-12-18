using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos
{
  public class ResponseDto<T> where T : class
  {
    public T Data { get; private set; }
    public int StatusCode { get; private set; }
    public ErrorDto Error { get; private set; }
    [JsonIgnore]
    public bool IsSuccessful { get; private set; }


    public static ResponseDto<T> Success(T data, int statusCode)
    {
      return new ResponseDto<T> { Data = data, StatusCode = statusCode , IsSuccessful=true};
    }

    public static ResponseDto<T> Success(int statusCode)
    {
      return new ResponseDto<T> { StatusCode = statusCode, IsSuccessful=true };
    }

    public static ResponseDto<T> Fail(int statusCode, ErrorDto error)
    {
      return new ResponseDto<T> { StatusCode = statusCode, Error = error, IsSuccessful=false };
    }

    public static ResponseDto<T> Fail(int statusCode, string errorMessage, bool isShown)
    {
      var errorDto = new ErrorDto(errorMessage, isShown);
      return new ResponseDto<T> { StatusCode = statusCode, Error = errorDto , IsSuccessful = false  };
    }

  }
}
