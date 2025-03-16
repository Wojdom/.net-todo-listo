using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models;

public class Todo
{
    public int Id { get; set; }

    public string Description { get; set; }

    [DisplayName("Is Done")]
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

    public Todo()
    {
        CreatedAt = UpdatedAt = DateTime.Now;
    }
}