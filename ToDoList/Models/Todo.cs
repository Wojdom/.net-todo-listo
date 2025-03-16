using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models;

/// <summary>
///  Represent a todo item.
///  </summary>
public class Todo
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Description { get; set; } = string.Empty;

    [DisplayName("Completed")]
    public bool IsDone { get; set; }

    [DisplayName("Created At")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Updated At")]
    [DataType(DataType.Date)]
    public DateTime UpdatedAt { get; set; }

    [DisplayName("Due Date")]
    [DataType(DataType.Date)]
    public DateTime? DueDate { get; set; }

    public int? TasksListId { get; set; }

    public virtual TasksList? TasksList { get; set; }

    public Todo()
    {
        CreatedAt = UpdatedAt = DateTime.Now;
    }
}