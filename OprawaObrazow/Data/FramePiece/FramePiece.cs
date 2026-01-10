using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using OprawaObrazow.Data.Base;

namespace OprawaObrazow.Data.FramePiece;

[Table( "frame_pieces", Schema = "oprawa" )]
[Index( "FrameId", Name = "IX_frame_pieces_frame_id" )]
[Index( "OrderId", Name = "IX_frame_pieces_order_id" )]
public sealed class FramePiece : BaseEntity, ISoftDelete
{
  [Column( "length" )]
  public int Length { get; set; }

  [Column( "is_damaged" )]
  public bool IsDamaged { get; set; }

  [Column( "frame_id" )]
  public int FrameId { get; set; }

  [Column( "storage_location" )]
  [StringLength( 10 )]
  public string? StorageLocation { get; set; }

  [Column( "is_in_stock" )]
  public bool IsInStock { get; set; }

  [Column( "order_id" )]
  public int? OrderId { get; set; }

  [ForeignKey( "FrameId" )]
  [InverseProperty( "FramePieces" )]
  public Frame.Frame Frame { get; set; } = null!;

  [ForeignKey( "OrderId" )]
  [InverseProperty( "FramePieces" )]
  public Order.Order? Order { get; set; }

  [Column( "is_deleted" )]
  [DefaultValue( false )]
  public bool IsDeleted { get; set; }
}