using TaskList;
using TaskList.Repository;

namespace Tasks
{
    [TestFixture]
    public sealed class ApplicationTest
    {
        public const string PROMPT = "> ";

        private FakeConsole console;
        private System.Threading.Thread applicationThread;
        private TaskRepository repo;

        [SetUp]
        public void StartTheApplication()
        {
            this.console = new FakeConsole();
            this.repo = new TaskRepository();
            var taskList = new TaskList.TaskList(console, repo);
            this.applicationThread = new System.Threading.Thread(() => taskList.Run());
            applicationThread.Start();
            ReadLines(TaskList.TaskList.startupText);
        }

        [TearDown]
        public void KillTheApplication()
        {
            if (applicationThread == null || !applicationThread.IsAlive)
            {
                return;
            }

            applicationThread.Abort();
            throw new Exception("The application is still running.");
        }

        [Test, Timeout(1000)]
        public void HelpCommand()
        {
            Execute("help");
            ReadLines(
                  "Commands:",
            "  show",
            "  add project <project name>",
            "  add task <project name> <task description>",
            "  check <task ID>",
            "  uncheck <task ID>",
            "  deadline <ID> <date>",
            "  today",
            "  view-by-deadline",
            ""
            );
            Execute("quit");
        }

        [Test, Timeout(1000)]
        public void TaskDeadlineCommand()
        {
            Execute("show");

            Execute("add project secrets");
            Execute("add task secrets Eat more donuts.");
            Execute("add task secrets Destroy all humans.");
            Execute("deadline 2 01-01-2025");
            Execute("show");
            ReadLines(
                "secrets",
                "    [ ] 1: Eat more donuts.",
                "    [ ] 2: Destroy all humans. 01-01-2025",
                ""
            );
            Execute("quit");
        }


        [Test, Timeout(1000)]
        public void TodayCommand()
        {
            Execute("show");
            string today = DateOnly.FromDateTime(DateTime.Now).ToString("dd-MM-yyyy");
            Execute("add project secrets");
            Execute("add task secrets Eat more donuts.");
            Execute("add task secrets Destroy all humans.");
            Execute($"deadline 2 {today}");
            Execute("today");
            ReadLines(
                "secrets",
               $"    [ ] 2: Destroy all humans. {today}",
                ""
            );
            Execute("quit");
        }

        [Test, Timeout(1000)]
        public void ViewByDeadlineCommand_simple()
        {
            Execute("show");

            string tommorow = DateOnly.FromDateTime(DateTime.Now.AddDays(1)).ToString("dd-MM-yyyy");

            Execute("add project secrets");
            Execute("add task secrets Eat more donuts.");
            Execute($"deadline 1 {tommorow}");

            Execute("view-by-deadline");
            ReadLines(
                $"{tommorow}:",
                 $"   secrets:",
                 $"      1: Eat more donuts."

            );
            Execute("quit");
        }

        [Test, Timeout(1000)]
        public void ViewByDeadlineCommand()
        {
            Execute("show");

            string today = DateOnly.FromDateTime(DateTime.Now).ToString("dd-MM-yyyy");
            string yesterday = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)).ToString("dd-MM-yyyy");
            string tommorow = DateOnly.FromDateTime(DateTime.Now.AddDays(1)).ToString("dd-MM-yyyy");

            Execute("add project secrets");
            Execute("add task secrets Eat more donuts.");
            Execute("add task secrets Destroy all humans.");
            Execute("add task secrets Task3");

            Execute("add project training");
            Execute("add task training Four Elements of Simple Design");
            Execute("add task training SOLID");

            Execute("add project emptyProject");

            Execute($"deadline 1 {tommorow}");
            Execute($"deadline 2 {today}");
            Execute($"deadline 5 {yesterday}");
            Execute("view-by-deadline");
            ReadLines(
                 $"{yesterday}:",
                  $"   training:",
                  $"      5: SOLID",
                  $"{today}:",
                  $"   secrets:",
                  $"      2: Destroy all humans.",
                  $"{tommorow}:",
                  $"   secrets:",
                  $"      1: Eat more donuts.",
                  "No deadline:",
                  "   secrets:",
                  "      3: Task3",
                  "   training:",
                  "      4: Four Elements of Simple Design"


             );
            Execute("quit");
        }

        [Test, Timeout(1000)]
        public void ItWorks()
        {
            Execute("show");

            Execute("add project secrets");
            Execute("add task secrets Eat more donuts.");
            Execute("add task secrets Destroy all humans.");

            Execute("show");
            ReadLines(
                "secrets",
                "    [ ] 1: Eat more donuts.",
                "    [ ] 2: Destroy all humans.",
                ""
            );

            Execute("add project training");
            Execute("add task training Four Elements of Simple Design");
            Execute("add task training SOLID");
            Execute("add task training Coupling and Cohesion");
            Execute("add task training Primitive Obsession");
            Execute("add task training Outside-In TDD");
            Execute("add task training Interaction-Driven Design");

            Execute("check 1");
            Execute("check 3");
            Execute("check 5");
            Execute("check 6");

            Execute("show");
            ReadLines(
                "secrets",
                "    [x] 1: Eat more donuts.",
                "    [ ] 2: Destroy all humans.",
                "",
                "training",
                "    [x] 3: Four Elements of Simple Design",
                "    [ ] 4: SOLID",
                "    [x] 5: Coupling and Cohesion",
                "    [x] 6: Primitive Obsession",
                "    [ ] 7: Outside-In TDD",
                "    [ ] 8: Interaction-Driven Design",
                ""
            );

            Execute("quit");
        }

        private void Execute(string command)
        {
            Read(PROMPT);
            Write(command);
        }

        private void Read(string expectedOutput)
        {
            var length = expectedOutput.Length;
            var actualOutput = console.RetrieveOutput(expectedOutput.Length);
            Assert.AreEqual(expectedOutput, actualOutput);
        }

        private void ReadLines(params string[] expectedOutput)
        {
            foreach (var line in expectedOutput)
            {
                Read(line + Environment.NewLine);
            }
        }

        private void Write(string input)
        {
            console.SendInput(input + Environment.NewLine);
        }
    }
}
