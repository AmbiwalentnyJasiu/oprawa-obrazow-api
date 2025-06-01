using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OprawaObrazow.Data.Models;

[Table("frame_pieces", Schema = "oprawa")]
[Index("DeliveryId", Name = "IX_frame_pieces_delivery_id")]
[Index("FrameId", Name = "IX_frame_pieces_frame_id")]
[Index("OrderId", Name = "IX_frame_pieces_order_id")]
public partial class FramePiece : ISoftDelete
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("length")]
    public int Length { get; set; }

    [Column("is_damaged")]
    public bool IsDamaged { get; set; }

    [Column("frame_id")]
    public int FrameId { get; set; }

    [Column("storage_location")]
    [StringLength(10)]
    public string? StorageLocation { get; set; }

    [Column("is_in_stock")]
    public bool IsInStock { get; set; }

    [Column("delivery_id")]
    public int DeliveryId { get; set; }

    [Column("order_id")]
    public int? OrderId { get; set; }
    
    [Column("is_deleted")]
    [DefaultValue(false)]
    public bool IsDeleted { get; set; }

    [ForeignKey("DeliveryId")]
    [InverseProperty("FramePieces")]
    public virtual Delivery Delivery { get; set; } = null!;

    [ForeignKey("FrameId")]
    [InverseProperty("FramePieces")]
    public virtual Frame Frame { get; set; } = null!;

    [ForeignKey("OrderId")]
    [InverseProperty("FramePieces")]
    public virtual Order? Order { get; set; }
}
