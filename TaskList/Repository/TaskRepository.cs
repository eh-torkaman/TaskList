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
    private readonly List<Project> projects;

    public TaskRepository()
    {
        projects = new();
    }

    public bool ContainsProject(string projectName)
    {
        return projects.Any(Project => Project.Name == projectName);
    }

    public void AddProject(string projectName)
    {
        if (ContainsProject(projectName))
            throw new TaskOperationException(string.Format("The project \"{0}\" already exists.", projectName));

        projects.Add(new Project { Name = projectName });
    }

    public void AddTask(string projectName, Task task)
    {
        if (!ContainsProject(projectName))
            throw new TaskOperationException(string.Format("Could not find a project with the name \"{0}\".", projectName));


        task.Id = GetMaxTaskId() + 1;
        projects.First(x => x.Name == projectName).Tasks.Add(task);
    }

    public IList<Task> GetTasksByProject(string projectName)
    {
        if (!ContainsProject(projectName))
            throw new TaskOperationException(string.Format("Could not find a project with the name \"{0}\".", projectName));

        return projects.First(x => x.Name == projectName).Tasks;
    }

    public Task GetTaskById(long taskId)
    {
        var task = projects.SelectMany(t => t.Tasks).FirstOrDefault(t => t.Id == taskId);
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
    public IList<Project> GetAll()
    {
        return projects;
    }

    public IList<GroupedTaskByDeadlineDto> GetGroupedTaskListByDeadLine()
    {
        var rs = projects
              .SelectMany(p => p.Tasks.Select(t => new
              {
                  ProjectName = p.Name,
                  ProjectCreatedAt = p.CreatedAt,
                  Task = t,

              }))
         .GroupBy(item => item.Task.Deadline)
         .OrderBy(group => group.Key ?? DateOnly.MaxValue) // Ensure null deadlines are last
         .Select(dateGroup => new GroupedTaskByDeadlineDto
         {
             Deadline = dateGroup.Key?.ToString("dd-MM-yyyy") ?? "No deadline",
             GroupdProjectTasks = dateGroup
                 .GroupBy(item => item.ProjectName)
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
        return projects.SelectMany(p => p.Tasks).Select(t => t.Id).DefaultIfEmpty(0).Max();
    }


}
