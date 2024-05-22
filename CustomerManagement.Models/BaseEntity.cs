namespace CustomerManagement.Models
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public bool Active { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
