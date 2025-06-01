using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OprawaObrazow.Data.Models;

[Table("deliveries", Schema = "oprawa")]
[Index("SupplierId", Name = "IX_deliveries_supplier_id")]
public partial class Delivery : ISoftDelete
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("supplier_id")]
    public int SupplierId { get; set; }

    [Column("date_due")]
    public DateOnly DateDue { get; set; }

    [Column("is_delivered")]
    public bool IsDelivered { get; set; }
    
    [Column("is_deleted")]
    [DefaultValue(false)]
    public bool IsDeleted { get; set; }

    [InverseProperty("Delivery")]
    public virtual ICollection<FramePiece> FramePieces { get; set; } = new List<FramePiece>();

    [ForeignKey("SupplierId")]
    [InverseProperty("Deliveries")]
    public virtual Supplier Supplier { get; set; } = null!;
}
