using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OprawaObrazow.Data.Models;

[Table("orders", Schema = "oprawa")]
[Index("ClientId", Name = "IX_orders_client_id")]
public partial class Order : ISoftDelete
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("price", TypeName = "decimal(18, 0)")]
    public decimal Price { get; set; }

    [Column("date_due")]
    public DateOnly DateDue { get; set; }

    [Column("is_finished")]
    public bool IsFinished { get; set; }

    [Column("is_closed")]
    public bool IsClosed { get; set; }

    [Column("planned_date")]
    public DateOnly? PlannedDate { get; set; }

    [Column("client_id")]
    public int ClientId { get; set; }

    [Column("picture_width")]
    public int PictureWidth { get; set; }

    [Column("picture_height")]
    public int PictureHeight { get; set; }
    
    [Column("is_deleted")]
    [DefaultValue(false)]
    public bool IsDeleted { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("Orders")]
    public virtual Client Client { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<FramePiece> FramePieces { get; set; } = new List<FramePiece>();
}
