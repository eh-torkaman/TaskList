using TaskList.CommandAndQuary.Commands;
using TaskList.Contracts;
using TaskList.Exceptions;
using TaskList.Repository;
namespace Tasks;


[TestFixture]
public sealed class CommandTests
{

    private ITaskRepository repo;

    [SetUp]
    public void SetUp()
    {
        //or a mock..
        this.repo = new TaskRepository();
    }

    [TearDown]
    public void TearDown()
    {
    }

    [Test]
    public void AddProjectCommand()
    {
        ICommand command = new AddProjectCommand("Training");
        command.Execute(repo);
        Assert.True(repo.ContainsProject("Training"));
    }

    [Test]
    public void AddDuplicatedProject_ThorwException()
    {
        ICommand command = new AddProjectCommand("Training");
        command.Execute(repo);
        Assert.True(repo.ContainsProject("Training"));
        Assert.Throws<TaskOperationException>(() => command.Execute(repo));
    }

    [Test]
    public void AddTaskCommandToNoexistingProject()
    {
        ICommand command = new AddTaskCommand("Training", "Task1");
        Assert.Throws<TaskOperationException>(() => command.Execute(repo));
    }

    [Test]
    public void AddTaskCommand()
    {
        ICommand command = new AddProjectCommand("Training");
        command.Execute(repo);
        command = new AddTaskCommand("Training", "Task1");
        command.Execute(repo);

        Assert.True(repo.GetTasksByProject("Training").Any(x => x.Description == "Task1"));
    }


    [Test]
    public void AddTaskDeadlineCommand()
    {
        ICommand command = new AddProjectCommand("Training");
        command.Execute(repo);
        command = new AddTaskCommand("Training", "Task1");
        command.Execute(repo);

        command = new TaskDeadlineCommand(1, new DateOnly());
        command.Execute(repo);
        Assert.True(repo.GetTasksByProject("Training").Any(x => x.Description == "Task1" && x.Deadline == new DateOnly()));
    }


    [Test]
    public void AddCheckCommand()
    {
        ICommand command = new AddProjectCommand("Training");
        command.Execute(repo);

        command = new AddTaskCommand("Training", "Task1");
        command.Execute(repo);

        command = new CheckCommand(1, true);
        command.Execute(repo);

        Assert.True(repo.GetTasksByProject("Training").Any(x => x.Description == "Task1" && x.Done));

        command = new CheckCommand(1, false);
        command.Execute(repo);

        Assert.True(repo.GetTasksByProject("Training").Any(x => x.Description == "Task1" && !x.Done));
    }
}