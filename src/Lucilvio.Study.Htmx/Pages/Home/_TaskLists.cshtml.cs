using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lucilvio.Study.Htmx.Pages.Home;

public class TaskListsModel : PageModel
{
    public TaskListsModel(Guid taskListId, IEnumerable<TaskList> taskLists)
    {
        this.TaskListId = taskListId;

        this.TaskLists = taskLists.Where(tl => !tl.Predefined);
        this.PredefinedTaskLists = taskLists.Where(tl => tl.Predefined);
    }

    public Guid TaskListId { get; }

    public IEnumerable<TaskList> TaskLists { get; set; } = [];
    public IEnumerable<TaskList> PredefinedTaskLists { get; set; } = [];
}