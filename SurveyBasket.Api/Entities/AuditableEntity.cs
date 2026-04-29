namespace SurveyBasket.Api.Entities;

public class AuditableEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string CreatedById { get; set; } = string.Empty;
    public string? UpdatedById { get; set; } 
    public ApplicationUser CreatedBy { get; set; } = default!;
    public ApplicationUser? UpdatedBy { get; set; }

}
