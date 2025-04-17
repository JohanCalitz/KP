namespace API.KP.Common
{
    using Microsoft.AspNetCore.Mvc;

    public class ApiResponse : IActionResult
    {
        public object? Data { get; set; }
        public string? ErrorMessage { get; set; }

        public ApiResponse(object? data, string errorMessage = "")
        {
            Data = data;
            ErrorMessage = errorMessage;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            HttpResponse response = context.HttpContext.Response;

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                await response.WriteAsJsonAsync(new { message = ErrorMessage });
            }
            else if (Data == null)
            {
                response.StatusCode = StatusCodes.Status204NoContent;
            }
            else
            {
                response.StatusCode = StatusCodes.Status200OK;
                await response.WriteAsJsonAsync(new { data = Data });
            }
        }
    }
}
