
namespace TaskList.Repository
{
    public interface ITaskRepository
    {
        void AddProject(string projectName);
        void AddTask(string projectName, Task task);
        bool ContainsProject(string projectName);
        IDictionary<string, IList<Task>> GetAll();
        IList<Task> GetAllTasks();
        IList<Task> GetTasksByProject(string projectName);
        void SetTaskDone(long taskId, bool done);
        void SetTaskDeadline(long taskId, DateOnly? deadLine);


    }
}