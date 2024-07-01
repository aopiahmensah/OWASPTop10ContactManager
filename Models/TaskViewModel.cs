namespace OWASPTaskManager.Models
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? UserId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
