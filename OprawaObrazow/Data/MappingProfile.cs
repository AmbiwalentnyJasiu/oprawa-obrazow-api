using AutoMapper;
using OprawaObrazow.Modules.Color.Dto;
using OprawaObrazow.Modules.Frame.Dto;
using OprawaObrazow.Modules.FramePiece.Dto;
using OprawaObrazow.Modules.Order.Dto;
using OprawaObrazow.Modules.Supplier.Dto;

namespace OprawaObrazow.Data;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    // Supplier
    CreateMap<Supplier.Supplier, SupplierViewDto>();
    CreateMap<SupplierEditDto, Supplier.Supplier>();

    // Frame
    CreateMap<Frame.Frame, FrameViewDto>();
    CreateMap<Frame.Frame, FrameListDto>()
      .ForMember( dest => dest.SupplierName, opt => opt.MapFrom( src => src.Supplier.Name ) )
      .ForMember( dest => dest.ColorName, opt => opt.MapFrom( src => src.Color.Name ) );
    CreateMap<FrameEditDto, Frame.Frame>();

    // FramePiece
    CreateMap<FramePiece.FramePiece, FramePieceViewDto>()
      .ForMember( dest => dest.FrameCode, opt => opt.MapFrom( src => src.Frame.Code ) );
    CreateMap<FramePieceEditDto, FramePiece.FramePiece>();

    // Color
    CreateMap<Color.Color, ColorViewDto>();
    CreateMap<ColorEditDto, Color.Color>();

    // Order
    CreateMap<Order.Order, OrderViewDto>();
    CreateMap<Order.Order, OrderListDto>().ConstructUsing( order => new OrderListDto( order ) );
    CreateMap<OrderEditDto, Order.Order>();
  }
}