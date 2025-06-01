using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OprawaObrazow.Data.Models;

[Table("suppliers", Schema = "oprawa")]
public partial class Supplier : ISoftDelete
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Column("last_supply")]
    public DateOnly? LastSupply { get; set; }

    [Column("phone_number")]
    [StringLength(15)]
    public string? PhoneNumber { get; set; }

    [Column("email_address")]
    [StringLength(50)]
    public string? EmailAddress { get; set; }
    
    [Column("is_deleted")]
    [DefaultValue(false)]
    public bool IsDeleted { get; set; }

    [InverseProperty("Supplier")]
    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    [InverseProperty("Supplier")]
    public virtual ICollection<Frame> Frames { get; set; } = new List<Frame>();
}
