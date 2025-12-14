using System;

namespace TodoApp.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime? Deadline { get; set; }

        public string UserId { get; set; } = string.Empty;

    }
}

