using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OprawaObrazow.Data.Base;

namespace OprawaObrazow.Data.Order;

[Table( "orders", Schema = "oprawa" )]
public sealed class Order : BaseEntity, ISoftDelete
{
  [Column( "price", TypeName = "decimal(18, 0)" )]
  public decimal Price { get; set; }

  [Column( "date_due" )]
  public DateOnly DateDue { get; set; }

  [Column( "status" )]
  public OrderStatus Status { get; set; }

  [Column( "picture_width" )]
  public int PictureWidth { get; set; }

  [Column( "picture_height" )]
  public int PictureHeight { get; set; }

  [Column( "client_name" )]
  [StringLength( 200 )]
  public string ClientName { get; set; } = null!;

  [Column( "phone_number" )]
  [StringLength( 15 )]
  public string? PhoneNumber { get; set; }

  [Column( "email_address" )]
  [StringLength( 50 )]
  public string? EmailAddress { get; set; }

  [InverseProperty( "Order" )]
  public ICollection<FramePiece.FramePiece> FramePieces { get; set; } = new List<FramePiece.FramePiece>();

  [Column( "is_deleted" )]
  [DefaultValue( false )]
  public bool IsDeleted { get; set; }
}