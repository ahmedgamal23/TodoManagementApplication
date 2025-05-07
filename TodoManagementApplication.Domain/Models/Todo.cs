using System.ComponentModel.DataAnnotations;
using TodoManagementApplication.Domain.Validation;

namespace TodoManagementApplication.Domain.Models
{
    public class Todo
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public string Status { get; set; } = nameof(TodoStatus.Pending);

        [Required]
        public string Priority { get; set; } = string.Empty;

        [FutureDate("CreatedDate")]
        public DateTime? DueDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime LastModifiedDate { get; set; } = DateTime.Now;

        [Required]
        public bool IsDeleted { get; set; } = false;
    }

    public enum TodoStatus
    {
        Pending,
        InProgress,
        Completed
    }

    public enum TodoPriority
    {
        Low, Medium, High
    }

}
