using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TodoManagementApplication.Domain.Models;
using TodoManagementApplication.Domain.ViewModels;
using TodoManagementApplication.UI.ExternalService;
using TodoManagementApplication.UI.Models;

namespace TodoManagementApplication.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly APIHandlerService<TodoDto> _apiHandlerService;

        public HomeController(ILogger<HomeController> logger, APIHandlerService<TodoDto> apiHandlerService)
        {
            _logger = logger;
            _apiHandlerService = apiHandlerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string? status = null, string? priority = null)
        {
            string apiUrl = "";
            if (priority != null)
                apiUrl = $"Todo/GetAllByPriority/{priority}/{pageNumber}/{pageSize}";
            else if (status != null)
                apiUrl = $"Todo/GetAllByStatus/{status}/{pageNumber}/{pageSize}";
            else
                apiUrl = $"Todo/GetAll/{pageNumber}/{pageSize}";
            var result = await _apiHandlerService.GetAllServicesAsync(apiUrl);

            if (result.DataList?.Count() is > 0)
            {
                var tasksToUpdate = result.DataList.Select(async data =>
                {
                    var status = _apiHandlerService.DetermineStatus(data.DueDate);
                    if (status != data.Status && data.Status != nameof(TodoStatus.Completed))
                    {
                        data.Status = status;
                        var updateUrl = $"Todo/{data.Id}";
                        var putResult = await _apiHandlerService.PutServicesAsync(updateUrl, data);
                        return putResult.Data!;
                    }
                    return data;
                });

                result.DataList = (await Task.WhenAll(tasksToUpdate)).ToList();
            }

            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
