using OprawaObrazow.Data.Order;
using OprawaObrazow.Modules.FramePiece.Dto;

namespace OprawaObrazow.Modules.Order.Dto;

public class OrderViewDto
{
  public int Id { get; set; }
  public decimal Price { get; set; }
  public DateOnly DateDue { get; set; }
  public OrderStatus Status { get; set; }
  public int PictureWidth { get; set; }
  public int PictureHeight { get; set; }
  public string ClientName { get; set; } = null!;
  public string? PhoneNumber { get; set; }
  public string? EmailAddress { get; set; }
  public List<FramePieceViewDto> FramePieces { get; set; } = [];
}