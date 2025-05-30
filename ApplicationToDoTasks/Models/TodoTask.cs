using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationToDoTasks.Models
{
    public class TodoTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public Priority Priority { get; set; }

        // Foreign key
        [ForeignKey("CategoryId")]
   public Category? Category { get; set; }
        public int CategoryId { get; set; }
     
    }

    public enum Priority
    {
        Low,
        Medium,
        High
    }
}
