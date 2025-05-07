using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TodoManagementApplication.Application.Core;
using TodoManagementApplication.Application.Interface;
using TodoManagementApplication.Domain.Models;
using TodoManagementApplication.Domain.ViewModels;

namespace TodoManagementApplication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<TodoController> _logger;

        public TodoController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TodoController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("GetAll/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _unitOfWork.TodoRepository.GetAllAsync(
                    pageNumber, pageSize,
                    filter: x => !x.IsDeleted
                );
                result.Message = "Data returned successfully";
                result.Success = true;
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all todos");
                return StatusCode(500, new BaseModel<Todo>
                {
                    Success = false,
                    Message = "Internal server error"
                });
            }
        }

        [HttpGet("GetAllByStatus/{status}/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetAllByStatus(string status, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _unitOfWork.TodoRepository.GetAllAsync(
                    pageNumber, pageSize,
                    filter: x => x.Status == status && !x.IsDeleted,
                    orderBy: x => x.OrderByDescending(d => d.CreatedDate)
                );

                result.Message = "Filtered by status successfully";
                result.Success = true;
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching todos by status");
                return StatusCode(500, new BaseModel<Todo>
                {
                    Success = false,
                    Message = "Internal server error"
                });
            }
        }

        [HttpGet("GetAllByPriority/{priority}/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetAllByPriority(string priority, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _unitOfWork.TodoRepository.GetAllAsync(
                    pageNumber, pageSize,
                    filter: x => x.Priority == priority && !x.IsDeleted,
                    orderBy: x => x.OrderByDescending(d => d.DueDate)
                );

                result.Message = "Filtered by priority successfully";
                result.Success = true;
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching todos by priority");
                return StatusCode(500, new BaseModel<Todo>
                {
                    Success = false,
                    Message = "Internal server error"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                var result = await _unitOfWork.TodoRepository.GetAsync(x => x.Id == id && !x.IsDeleted);

                if (result == null)
                {
                    return NotFound(new BaseModel<Todo>
                    {
                        Success = false,
                        Message = $"Item with ID '{id}' not found"
                    });
                }

                return Ok(new BaseModel<Todo>
                {
                    Data = result,
                    Success = true,
                    Message = "Data returned successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting todo with ID {id}");
                return StatusCode(500, new BaseModel<Todo>
                {
                    Success = false,
                    Message = "Internal server error"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TodoDto createTodoDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .ToDictionary(
                        e => e.Key,
                        e => e.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new BaseModel<object>
                {
                    Success = false,
                    Message = "Validation failed",
                    Data = createTodoDto,
                    Errors = errors!
                });
            }

            try
            {
                var todo = _mapper.Map<Todo>(createTodoDto);
                todo.CreatedDate = DateTime.UtcNow;
                //todo.Status = DetermineStatus(todo.DueDate, todo.CreatedDate);

                await _unitOfWork.TodoRepository.CreateAsync(todo);
                var rows = await _unitOfWork.SaveChangesAsync();

                if (rows <= 0)
                {
                    return BadRequest(new BaseModel<Todo>
                    {
                        Success = false,
                        Message = "Failed to add todo",
                        Data = todo
                    });
                }

                return Ok(new BaseModel<Todo>
                {
                    Success = true,
                    Message = "Added successfully",
                    Data = todo
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating todo");
                return StatusCode(500, new BaseModel<Todo>
                {
                    Success = false,
                    Message = "Internal server error"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] TodoDto todoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseModel<object>
                {
                    Success = false,
                    Message = "Invalid data",
                    Data = todoDto
                });
            }

            try
            {
                var existingTodo = await _unitOfWork.TodoRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
                if (existingTodo == null)
                {
                    return NotFound(new BaseModel<object>
                    {
                        Success = false,
                        Message = "Todo not found"
                    });
                }

                var todo = _mapper.Map(todoDto, existingTodo);
                todo!.LastModifiedDate = DateTime.UtcNow;
                //todo.Status = DetermineStatus(todo.DueDate, todo.LastModifiedDate);

                var updated = await _unitOfWork.TodoRepository.UpdateAsync(todo);
                var rows = await _unitOfWork.SaveChangesAsync();

                if (!updated || rows <= 0)
                {
                    return BadRequest(new BaseModel<Todo>
                    {
                        Success = false,
                        Message = "Failed to update",
                        Data = todo
                    });
                }

                return Ok(new BaseModel<Todo>
                {
                    Success = true,
                    Message = "Updated successfully",
                    Data = todo
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating todo with ID {id}");
                return StatusCode(500, new BaseModel<Todo>
                {
                    Success = false,
                    Message = "Internal server error"
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new BaseModel<TodoDto>
                {
                    Success = false,
                    Message = "ID is required"
                });
            }

            try
            {
                var result = await _unitOfWork.TodoRepository.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(new BaseModel<Todo>
                    {
                        Success = false,
                        Message = "Item not found or already deleted"
                    });
                }

                var rows = await _unitOfWork.SaveChangesAsync();

                if (rows <= 0)
                {
                    return StatusCode(500, new BaseModel<TodoDto>
                    {
                        Success = false,
                        Message = "Failed to commit deletion"
                    });
                }

                return Ok(new BaseModel<TodoDto>
                {
                    Success = true,
                    Message = $"Deleted successfully: {id}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting todo with ID {id}");
                return StatusCode(500, new BaseModel<TodoDto>
                {
                    Success = false,
                    Message = "Internal server error"
                });
            }
        }
    }
}
