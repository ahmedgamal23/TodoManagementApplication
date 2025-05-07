using Newtonsoft.Json;
using System.Text;
using TodoManagementApplication.Domain.Models;
using TodoManagementApplication.UI.Models;

namespace TodoManagementApplication.UI.ExternalService
{
    public class APIHandlerService<T> where T : class
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _baseUrl;
        private const string JsonContentType = "application/json";

        public APIHandlerService(HttpClient httpClient, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
            _baseUrl = configuration["BaseUrl"] ?? throw new ArgumentNullException("BaseUrl not configured.");
        }

        public async Task<BaseModel<T>> GetAllServicesAsync(string api)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{api}");
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return ErrorResponse(api, content);

                return JsonConvert.DeserializeObject<BaseModel<T>>(content)!;
            }
            catch (Exception ex)
            {
                return new BaseModel<T> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<BaseModel<T>> PostServicesAsync(string api, T model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, JsonContentType);
            var response = await _httpClient.PostAsync($"{_baseUrl}/{api}", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return ErrorResponse(api, responseBody);

            return JsonConvert.DeserializeObject<BaseModel<T>>(responseBody)!;
        }

        public async Task<BaseModel<T>> PutServicesAsync(string api, T model)
        {
            //UpdateStatusIfNeeded(model);

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, JsonContentType);
            var response = await _httpClient.PutAsync($"{_baseUrl}/{api}", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return ErrorResponse(api, responseBody);

            return JsonConvert.DeserializeObject<BaseModel<T>>(responseBody)!;
        }

        public async Task<BaseModel<T>> DeleteServicesAsync(string api)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{api}");
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return ErrorResponse(api, responseBody);

            return JsonConvert.DeserializeObject<BaseModel<T>>(responseBody)!;
        }

        private void UpdateStatusIfNeeded(T model)
        {
            var dueDate = model.GetType().GetProperty("DueDate")?.GetValue(model) as DateTime?;
            var createdDate = model.GetType().GetProperty("CreatedDate")?.GetValue(model) as DateTime?;
            var currentStatus = model.GetType().GetProperty("Status")?.GetValue(model) as string;

            var calculatedStatus = DetermineStatus(dueDate);

            if (calculatedStatus != currentStatus)
            {
                model.GetType().GetProperty("Status")?.SetValue(model, calculatedStatus);
            }
        }

        private BaseModel<T> ErrorResponse(string api, string? responseBody)
        {
            return new BaseModel<T>
            {
                Success = false,
                Message = $"API call failed: {_baseUrl}/{api}. Response: {responseBody}"
            };
        }

        public string DetermineStatus(DateTime? dueDate)
        {
            var now = DateTime.UtcNow;
            if (dueDate > now) // in future
                return TodoStatus.InProgress.ToString();
            return TodoStatus.Pending.ToString();  // task finished (not started in it)
        }
    }

}
