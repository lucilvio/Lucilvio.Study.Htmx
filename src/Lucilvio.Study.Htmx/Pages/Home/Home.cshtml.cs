using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lucilvio.Study.Htmx.Pages.Home;

public class HomeModel : PageModel
{
    public IEnumerable<TaskList> TaskLists { get; set; } = [];

    public void OnGet()
    {
        this.TaskLists = Data.TaskLists;
    }

    public PartialViewResult OnGetTasks(Guid selectedTaskListId)
    {
        var taskList = Data.TaskLists.FirstOrDefault();

        if (selectedTaskListId != Guid.Empty)
            taskList = Data.TaskLists.FirstOrDefault(tl => tl.Id == selectedTaskListId);

        return this.Partial("_Tasks", new TasksModel(taskList));
    }

    public PartialViewResult OnGetTaskLists(Guid selectedTaskListId)
    {
        return this.Partial("_TaskLists", new TaskListsModel(selectedTaskListId, Data.TaskLists));
    }

    public ActionResult OnPostAddTaskList(string newTaskListName)
    {
        var taskList = new TaskList
        {
            Id = Guid.NewGuid(),
            Name = newTaskListName,
            Tasks = new List<TaskItem>()
        };

        Data.TaskLists.Add(taskList);

        return HtmxResult.OkWithTriggers(
            new HtmxTrigger("updateTaskLists"),
            new HtmxTrigger("updateTasks"));
    }

    public ActionResult OnPostAddTask(Guid taskListId, string newTaskName)
    {
        var taskList = Data.TaskLists.FirstOrDefault(tl => tl.Id == taskListId);

        if (taskList is null)
            return HtmxResult.Error("Task list not found");

        var newTask = new TaskItem
        {
            Id = Guid.NewGuid(),
            TaskListId = taskListId,
            Name = newTaskName,
            IsCompleted = false
        };

        taskList.Tasks.Add(newTask);

        return HtmxResult.OkWithTriggers(
            new HtmxTrigger("updateTaskLists"),
            new HtmxTrigger("updateTasks", new { taskListId }));
    }

    public ActionResult OnPutToggleTaskState(Guid taskListId, Guid taskId)
    {
        var taskList = Data.TaskLists.FirstOrDefault(tl => tl.Id == taskListId);

        if (taskList is null)
            return HtmxResult.Error("Task list not found");

        var task = taskList.Tasks.FirstOrDefault(t => t.Id == taskId);

        if (task is null)
            return this.Partial("_Tasks", taskList.Tasks);

        if (task.IsCompleted)
            task.IsCompleted = false;
        else
            task.IsCompleted = true;

        return HtmxResult.OkWithTriggers(
            new HtmxTrigger("updateTaskLists"),
            new HtmxTrigger("updateTasks", new { taskListId }));
    }

    public ActionResult OnDeleteRemoveTaskList(Guid taskListId)
    {
        var taskList = Data.TaskLists.FirstOrDefault(tl => tl.Id == taskListId);

        if (taskList is null)
            return HtmxResult.Error("Task list not found");

        Data.TaskLists.Remove(taskList);

        return HtmxResult.OkWithTriggers(
            new HtmxTrigger("updateTaskLists"),
            new HtmxTrigger("updateTasks"));
    }

    public ActionResult OnDeleteRemoveTask(Guid taskListId, Guid taskId)
    {
        var taskList = Data.TaskLists.FirstOrDefault(tl => tl.Id == taskListId);

        if (taskList is null)
            return HtmxResult.Error("Task list not found");

        taskList.Tasks = taskList.Tasks.Where(t => t.Id != taskId).ToList();

        return HtmxResult.OkWithTriggers(
            new HtmxTrigger("updateTaskLists"),
            new HtmxTrigger("updateTasks", new { taskListId }));
    }

    public ActionResult OnPostSelectTaskList(Guid taskListId)
    {
        return HtmxResult.OkWithTriggers(
            new HtmxTrigger("updateTaskLists", new { taskListId }),
            new HtmxTrigger("updateTasks", new { taskListId }));
    }
}