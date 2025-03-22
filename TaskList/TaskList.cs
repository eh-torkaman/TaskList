using TaskList.Commands;
using TaskList.Repository;

namespace TaskList
{
    public sealed class TaskList
    {

        private const string QUIT = "quit";
        public static readonly string startupText = "Welcome to TaskList! Type 'help' for available commands.";

        private readonly IConsole console;
        private readonly CommandFactory commandFactory;

        public static void Main(string[] args)
        {
            new TaskList(new RealConsole(), new TaskRepository()).Run();
        }

        public TaskList(IConsole console, ITaskRepository tasks)
        {
            this.console = console;
            this.commandFactory = new CommandFactory(tasks, console);
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
                Execute(command);
            }
        }

        private void Execute(string commandLine)
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



    }
}
