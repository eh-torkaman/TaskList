namespace TaskList
{
    public class Task
    {
        public long Id { get; set; }

        public string Description { get; set; }

        public bool Done { get; set; }

        public DateOnly? Deadline { get; set; }

    }

    public class Project
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public IList<Task> Tasks { get; set; }
        public Project()
        {
            Tasks = new List<Task>();
        }
    }
}
