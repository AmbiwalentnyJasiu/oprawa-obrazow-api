using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using OprawaObrazow.Data.Base;

namespace OprawaObrazow.Data.Frame;

[Table( "frames", Schema = "oprawa" )]
[Index( "SupplierId", Name = "IX_frames_supplier_id" )]
[Index( "ColorId", Name = "IX_frames_color_id" )]
public sealed class Frame : BaseEntity, ISoftDelete
{
  [Column( "color_id" )]
  public int ColorId { get; set; }

  [Column( "price", TypeName = "decimal(18, 0)" )]
  public decimal Price { get; set; }

  [Column( "supplier_id" )]
  public int SupplierId { get; set; }

  [Column( "width" )]
  public int Width { get; set; }

  [Column( "has_decoration" )]
  public bool HasDecoration { get; set; }

  [Column( "code" )]
  [StringLength( 20 )]
  public string Code { get; set; } = null!;

  [InverseProperty( "Frame" )]
  public ICollection<FramePiece.FramePiece> FramePieces { get; set; } = new List<FramePiece.FramePiece>();

  [ForeignKey( "SupplierId" )]
  [InverseProperty( "Frames" )]
  public Supplier.Supplier Supplier { get; set; } = null!;

  [ForeignKey( "ColorId" )]
  [InverseProperty( "Frames" )]
  public Color.Color Color { get; set; } = null!;

  [Column( "is_deleted" )]
  [DefaultValue( false )]
  public bool IsDeleted { get; set; }
}