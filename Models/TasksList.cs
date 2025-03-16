using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models;

/// <summary>
///   Represents a list of tasks.
/// </summary>
public class TasksList
{
    public int Id { get; set; }


    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    public List<Todo> Todos { get; set; } = [];

    [DisplayName("Created At")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Updated At")]
    [DataType(DataType.Date)]
    public DateTime UpdatedAt { get; set; }

    public TasksList()
    {
        CreatedAt = UpdatedAt = DateTime.Now;
    }
}