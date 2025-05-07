using System.ComponentModel.DataAnnotations;
using TodoManagementApplication.Domain.Validation;

namespace TodoManagementApplication.Domain.ViewModels
{
    public class TodoDto
    {
        public string? Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? Status { get; set; }

        [Required]
        public string Priority { get; set; } = string.Empty;

        [FutureDate("CreatedDate")]
        public DateTime? DueDate { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastModifiedDate { get; set; } = DateTime.Now;
    }
}
