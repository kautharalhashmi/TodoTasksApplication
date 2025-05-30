using Microsoft.AspNetCore.Mvc.Rendering;

namespace ApplicationToDoTasks.Models
{
    public class TodoTaskViewModel
    {
        public TodoTask NewTask { get; set; } = new();
        public List<TodoTask> Tasks { get; set; } = new();
        public SelectList Categories { get; set; } = null!;
        public SelectList Priorities { get; set; } = null!;
    }
}
