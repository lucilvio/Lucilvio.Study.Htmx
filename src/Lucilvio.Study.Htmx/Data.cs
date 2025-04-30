namespace Lucilvio.Study.Htmx;

public static class Data
{
    public static IList<TaskList> TaskLists { get; set; } =
    [
        new TaskList
        {
            Id = new Guid("CCB72564-C681-46E5-96DB-A397CA0DB889"),
            Name = "To do",
            Predefined = true
        }
    ];
}

public class TaskItem
{
    public Guid Id { get; set; }
    public Guid TaskListId { get; set; }
    public string Name { get; set; }
    public bool IsCompleted { get; set; }
}

public class TaskList
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool Predefined { get; set; }
    public IList<TaskItem> Tasks { get; set; } = [];
}