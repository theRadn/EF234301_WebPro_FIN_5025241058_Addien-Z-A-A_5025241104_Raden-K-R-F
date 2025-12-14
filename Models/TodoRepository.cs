using System.Collections.Generic;
using System.Linq;

namespace TodoApp.Models
{
    public static class TodoRepository
    {
        private static List<TodoItem> _items = new();
        private static int _nextId = 1;

        public static List<TodoItem> GetAll()
        {
            return _items;
        }

        public static TodoItem? GetById(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public static void Add(string title, DateTime? deadline)
        {
            _items.Add(new TodoItem
            {
                Id = _nextId++,
                Title = title,
                IsCompleted = false,
                Deadline = deadline
            });
        }

        public static void Update(int id, string newTitle, DateTime? deadline)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item != null && !string.IsNullOrWhiteSpace(newTitle))
            {
                item.Title = newTitle;
                item.Deadline = deadline;
            }
        }

        public static void ToggleComplete(int id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                item.IsCompleted = !item.IsCompleted;
            }
        }

        public static void Delete(int id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                _items.Remove(item);
            }
        }
    }
}

