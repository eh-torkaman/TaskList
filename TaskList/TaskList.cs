using TaskList.Commands;
using TaskList.Repository;

namespace TaskList
{
    public sealed class TaskList
    {

        // todo implement undo ...
        //private IList<ICommand> _commandHistory= new List<ICommand>();
        // save state....
        private const string QUIT = "quit";
        public static readonly string startupText = "Welcome to TaskList! Type 'help' for available commands.";

        private readonly IConsole console;
        private readonly ITaskRepository taskRepo;
        private readonly CommandFactory commandFactory;

        public static void Main(string[] args)
        {
            new TaskList(new RealConsole(), new TaskRepository()).Run();
        }

        public TaskList(IConsole console, ITaskRepository taskRepo)
        {
            this.console = console;
            this.taskRepo = taskRepo;
            this.commandFactory = new CommandFactory(taskRepo, console);
        }

        public void Run()
        {
            console.WriteLine(startupText);
            while (true)
            {
                console.Write("> ");
                var command = console.ReadLine();
                if (command == QUIT)
                {
                    break;
                }
                if (!string.IsNullOrWhiteSpace(command))
                {
                    Execute(command);
                }
            }
        }

        public void Execute(string commandLine)
        {
            try
            {
                var command = commandFactory.CreateCommand(commandLine);
                try
                {
                    command.Execute();
                }
                catch (TaskOperationException ex)
                {
                    console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                console.WriteLine("Error!=> " + ex.Message);
            }

        }

        public ITaskRepository Repo => this.taskRepo;


    }
}
