using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OprawaObrazow.Data.Models;

[Table("frames", Schema = "oprawa")]
[Index("SupplierId", Name = "IX_frames_supplier_id")]
public partial class Frame : ISoftDelete
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("color")]
    public int Color { get; set; }

    [Column("price", TypeName = "decimal(18, 0)")]
    public decimal Price { get; set; }

    [Column("supplier_id")]
    public int SupplierId { get; set; }

    [Column("width")]
    public int Width { get; set; }

    [Column("has_decoration")]
    public bool HasDecoration { get; set; }

    [Column("storage_location")]
    [StringLength(10)]
    public string? StorageLocation { get; set; }

    [Column("code")]
    [StringLength(20)]
    public string Code { get; set; } = null!;
    
    [Column("is_deleted")]
    [DefaultValue(false)]
    public bool IsDeleted { get; set; }

    [InverseProperty("Frame")]
    public virtual ICollection<FramePiece> FramePieces { get; set; } = new List<FramePiece>();

    [ForeignKey("SupplierId")]
    [InverseProperty("Frames")]
    public virtual Supplier Supplier { get; set; } = null!;
}
