using ApplicationToDoTasks.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationToDoTasks.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TodoTask> TodoTasks { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
