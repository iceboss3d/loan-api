using Newtonsoft.Json;

namespace Loan.Api.Models.DTOs;

public class ResponseDTO<T>
{
    public T Data { get; set; }
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }

    public ResponseDTO(int statusCode, bool success, string msg, T data)
    {
        Data = data;
        Succeeded = success;
        StatusCode = statusCode;
        Message = msg;
    }
    public ResponseDTO()
    {
    }
    /// <summary>
    /// Sets the data to the appropriate response
    /// at run time
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public static ResponseDTO<T> Fail(string errorMessage, int statusCode = 404)
    {
        return new ResponseDTO<T> { Succeeded = false, Message = errorMessage, StatusCode = statusCode };
    }
    public static ResponseDTO<T> Success(string successMessage, T data, int statusCode = 200)
    {
        return new ResponseDTO<T> { Succeeded = true, Message = successMessage, Data = data, StatusCode = statusCode };
    }
    public override string ToString() => JsonConvert.SerializeObject(this);
}

public class PagedResult<T>
{
    public T PageItems { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int NumberOfPages { get; set; }
    public int PreviousPage { get; set; }
}