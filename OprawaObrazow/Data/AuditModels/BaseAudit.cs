using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OprawaObrazow.Data.AuditModels;

public abstract class BaseAudit
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("change_type")]
    public string ChangeType { get; set; } = null!;
    
    [Column("changed_at")]
    public DateTime ChangedAt { get; set; }
    
    [Column("record_id")]
    public int RecordId { get; set; }
    
    [Column("entity_data", TypeName = "json")]
    public string EntityData { get; set; } = null!;

}