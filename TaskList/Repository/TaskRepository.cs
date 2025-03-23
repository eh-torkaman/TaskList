using TaskList.Dtos;

namespace TaskList.Repository;

public class TaskOperationException : Exception
{
    public TaskOperationException() : base()
    {

    }
    public TaskOperationException(string msg) : base(msg)
    {

    }
}
public class TaskRepository : ITaskRepository
{
    private readonly Dictionary<string, IList<Task>> tasks;

    public TaskRepository()
    {
        tasks = new Dictionary<string, IList<Task>>();
    }

    public bool ContainsProject(string projectName)
    {
        return tasks.ContainsKey(projectName);
    }

    public void AddProject(string projectName)
    {
        if (ContainsProject(projectName))
            throw new TaskOperationException(string.Format("The project \"{0}\" already exists.", projectName));

        tasks.Add(projectName, new List<Task>());
    }

    public void AddTask(string projectName, Task task)
    {
        if (!tasks.TryGetValue(projectName, out var projectTasks))
            throw new TaskOperationException(string.Format("Could not find a project with the name \"{0}\".", projectName));


        task.Id = GetMaxTaskId() + 1;
        projectTasks.Add(task);
    }

    public IList<Task> GetTasksByProject(string projectName)
    {
        if (!ContainsProject(projectName))
            throw new TaskOperationException(string.Format("Could not find a project with the name \"{0}\".", projectName));

        return tasks[projectName];
    }

    public Task GetTaskById(long taskId)
    {
        var task = tasks.Values.SelectMany(t => t).FirstOrDefault(t => t.Id == taskId);
        if (task == null)
            throw new TaskOperationException(string.Format("Could not find a task with an ID of {0}.", taskId));
        return task;
    }

    public void SetTaskDone(long taskId, bool done)
    {
        GetTaskById(taskId).Done = done;
    }

    public void SetTaskDeadline(long taskId, DateOnly? deadLine)
    {
        GetTaskById(taskId).Deadline = deadLine;
    }
    public IDictionary<string, IList<Task>> GetAll()
    {
        return tasks;
    }

    public IList<GroupedTaskByDeadlineDto> GetGroupedTaskListByDeadLine()
    {
        var rs = tasks
         .SelectMany(kvp => kvp.Value.Select(task => new { Project = kvp.Key, Task = task }))
         .GroupBy(item => item.Task.Deadline)
         .OrderBy(group => group.Key ?? DateOnly.MaxValue) // Ensure null deadlines are last
         .Select(dateGroup => new GroupedTaskByDeadlineDto
         {
             Deadline = dateGroup.Key?.ToString("dd-MM-yyyy") ?? "No deadline",
             GroupdProjectTasks = dateGroup
                 .GroupBy(item => item.Project)
                 .Select(projectGroup => new GroupedTaskByProjectDto
                 {
                     Project = projectGroup.Key,
                     Task = projectGroup.Select(item => item.Task).OrderBy(it => it.Id).ToList()
                 })
                 .ToList()
         })
         .ToList();
        return rs;
    }
    private long GetMaxTaskId()
    {
        return tasks.Values.SelectMany(t => t).Select(t => t.Id).DefaultIfEmpty(0).Max();
    }


}
