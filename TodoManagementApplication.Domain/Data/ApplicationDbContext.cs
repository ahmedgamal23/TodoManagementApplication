using Microsoft.EntityFrameworkCore;
using TodoManagementApplication.Domain.Models;

namespace TodoManagementApplication.Domain.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Todo> Todos { get; set; }


    }
}
