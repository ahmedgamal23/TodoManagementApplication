using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TodoManagementApplication.Domain.Models;
using TodoManagementApplication.Domain.ViewModels;
using TodoManagementApplication.UI.ExternalService;

namespace TodoManagementApplication.UI.Controllers
{
    public class TodoController : Controller
    {
        private readonly APIHandlerService<TodoDto> _apiHandlerService;

        public TodoController(APIHandlerService<TodoDto> apiHandlerService)
        {
            _apiHandlerService = apiHandlerService;
        }

        private void SetPriorityViewBag(string? selected = null)
        {
            ViewBag.AllPriority = new SelectList(Enum.GetValues(typeof(TodoPriority)).Cast<TodoPriority>(), selected);
        }

        [HttpPost]
        public async Task<IActionResult> Completed(string id)
        {
            string apiUrl = $"Todo/{id}";
            var result = await _apiHandlerService.GetAllServicesAsync(apiUrl);

            if (result?.Data != null)
            {
                result.Data.Status = nameof(TodoStatus.Completed);
                await _apiHandlerService.PutServicesAsync(apiUrl, result.Data);

                return Json(new { success = true, newStatus = result.Data.Status });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            string apiUrl = $"Todo/{id}";
            var result = await _apiHandlerService.GetAllServicesAsync(apiUrl);
            var data = result.Data;

            if (data == null) return NotFound();

            var status = _apiHandlerService.DetermineStatus(data.DueDate);
            if (status != data.Status && data.Status != nameof(TodoStatus.Completed))
            {
                data.Status = status;
                result = await _apiHandlerService.PutServicesAsync(apiUrl, data);
            }

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new TodoDto
            {
                CreatedDate = DateTime.Now
            };
            SetPriorityViewBag();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TodoDto createTodoDto)
        {
            if (!ModelState.IsValid)
            {
                SetPriorityViewBag();
                return View(createTodoDto);
            }

            string apiUrl = "Todo";
            var result = await _apiHandlerService.PostServicesAsync(apiUrl, createTodoDto);

            if (result.Success == true)
                return RedirectToAction("Index", "Home");

            SetPriorityViewBag();
            return View(createTodoDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            string apiUrl = $"Todo/{id}";
            var result = await _apiHandlerService.GetAllServicesAsync(apiUrl);
            if (result.Data == null) return NotFound();
            
            result.Data.LastModifiedDate = DateTime.Now;
            SetPriorityViewBag(result.Data.Priority);
            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, TodoDto todo)
        {
            string apiUrl = $"Todo/{id}";
            var result = await _apiHandlerService.PutServicesAsync(apiUrl, todo);

            if (result.Success == true)
                return RedirectToAction("Index", "Home");

            SetPriorityViewBag(todo.Priority);
            ViewBag.MessageError = result.Message ?? "Update failed.";
            return View(todo);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            string apiUrl = $"Todo/{id}";
            var result = await _apiHandlerService.DeleteServicesAsync(apiUrl);

            if (result.Success == true)
                return RedirectToAction("Index", "Home");

            ViewBag.MessageError = result.Message ?? "Delete failed.";
            return View();
        }
    }

}
