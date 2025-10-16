using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ProductsApi.DTO
{
    public class ApiResponseSuccessDTO<T>
    {
        public bool Success { get; set; } = true;
        public T Data { get; set; }

        public ApiResponseSuccessDTO(T data)
        {
            Success = true;
            Data = data;
        }
    }

    public class ApiResponseErrorDTO
    {
        public string Message { get; set; }


        public ApiResponseErrorDTO(string message)
        {
            Message = message;
        }
    }
     public class ApiResponseErrorValidationDTO
    {

        [JsonPropertyName("errors")]
        public List<FieldError> Errors { get; set; }

        public ApiResponseErrorValidationDTO(ModelStateDictionary modelState)
        {
            Errors = modelState
                .Where(ms => ms.Value.Errors.Count > 0)
                .SelectMany(kv => kv.Value.Errors.Select(err => new FieldError
                {
                    Field = kv.Key.ToLower(),
                    Message = err.ErrorMessage
                }))
                .ToList();
        }

        public ApiResponseErrorValidationDTO(List<FieldError> errors)
        {
            Errors = errors;
        }
    }

    public class FieldError
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }
}
