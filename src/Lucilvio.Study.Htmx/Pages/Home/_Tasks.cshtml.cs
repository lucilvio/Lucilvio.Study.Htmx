using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lucilvio.Study.Htmx.Pages.Home;

public class TasksModel : PageModel
{
    public TasksModel(TaskList taskList) 
    {
        this.Tasks = taskList.Tasks;
        this.TaskListName = taskList.Name;
        this.TaskListId = taskList.Id;
    }

    public Guid TaskListId { get; set; }
    public string TaskListName { get; private set; }
    public IEnumerable<TaskItem> Tasks { get; set; } = [];

    public int TotalTasks => this.Tasks.Count();
    public int TotalCompletedTasks => this.Tasks.Count(t => t.IsCompleted);
}