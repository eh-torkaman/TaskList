using Microsoft.AspNetCore.Mvc;
using TaskList.CommandAndQuary.Commands;
using TaskList.Dtos;

namespace TaskList;

[ApiController]
[Route("projects")]
public class TaskController : ControllerBase
{
    public TaskController(TaskList taskList)
    {
        TaskList = taskList;
    }

    /*
     [HttpGet()]
     public ActionResult<IDictionary<string, IList<Task>>> GetAllTasks()
     {
         // here we can extend the FakeConsole to capture the output of the command/Query and return it as a response
         // so we can put the output in the response body and return it
    
       var q=new ShowQuery();
        q.Run(TaskList.Repo);
         return Ok(Iconsole.Response);
     }
     */

    [HttpGet("view_by_deadline")]
    public ActionResult<IList<GroupedTaskByDeadlineDto>> GetViewByDeadline()
    {
        return Ok(TaskList.Repo.GetGroupedTaskListByDeadLine());
    }

    [HttpPost]
    public ActionResult CreateProject([FromBody] string project_id)
    {
        var command = new AddProjectCommand(project_id);
        command.Execute(TaskList.Repo);
        return Ok();
    }
    [HttpPost("{project_id}/tasks")]
    public ActionResult CreateTask([FromBody] string taskDescription, [FromRoute] string project_id)
    {
        var command = new AddTaskCommand(project_id, taskDescription);
        command.Execute(TaskList.Repo);
        return Ok();
    }

    [HttpPut("{project_id}/tasks/{task_id}")]
    public ActionResult SetTaskDeadline([FromQuery] DateOnly deadline, [FromRoute] string project_id, [FromRoute] int task_id)
    {
        var command = new TaskDeadlineCommand(task_id, deadline);
        command.Execute(TaskList.Repo);
        return Ok();
    }


    private TaskList TaskList { get; }

}

//public class Loger : IConsole
//{
//    public string Response = "";
//    public string ReadLine()
//    {
//        throw new NotImplementedException();
//    }

//    public void Write(string format, params object[] args)
//    {
//        Response += string.Format(format, args);
//    }

//    public void WriteLine(string message)
//    {
//        Response += message + "\n";
//    }

//    public void WriteLine(string format, params object[] args)
//    {
//        Response += string.Format(format, args) + "\n";
//    }

//    public void WriteLine()
//    {
//        Response += "\n";
//    }
//}
