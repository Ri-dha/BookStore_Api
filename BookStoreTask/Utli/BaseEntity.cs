using System.ComponentModel.DataAnnotations;

namespace BookStoreTask.Utli;

public class BaseEntity<TId>
{
    [Key]
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TId Id { get; set; }

    public bool Deleted { get; set; } = false;
    private DateTime _createdAt = DateTime.UtcNow;
    public DateTime CreatedAt
    {
        get => _createdAt;
        set => _createdAt = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }

    private DateTime? _updatedAt;
    public DateTime? UpdatedAt
    {
        get => _updatedAt;
        set => _updatedAt = value.HasValue
            ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
            : value;
    }
    
}