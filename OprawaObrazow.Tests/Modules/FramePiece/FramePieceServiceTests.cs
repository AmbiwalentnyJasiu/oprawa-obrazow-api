using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Moq;
using OprawaObrazow.Data;
using OprawaObrazow.Modules.Base;
using OprawaObrazow.Modules.FramePiece;
using OprawaObrazow.Modules.FramePiece.Dto;

namespace OprawaObrazow.Tests.Modules.FramePiece;

public class FramePieceServiceTests
{
  private readonly IMapper _mapper;
  private readonly Mock<IBaseRepository<Data.FramePiece.FramePiece>> _repositoryMock;
  private readonly FramePieceService _sut;

  public FramePieceServiceTests()
  {
    _repositoryMock = new Mock<IBaseRepository<Data.FramePiece.FramePiece>>();
    var config = new MapperConfiguration( cfg => cfg.AddProfile<MappingProfile>() );
    _mapper = config.CreateMapper();
    _sut = new FramePieceService( _repositoryMock.Object, _mapper );
  }

  [Fact]
  public async Task AttachToOrder_ShouldUpdateOrderId_WhenFramePieceExists()
  {
    // Arrange
    var id = 1;
    var orderId = 10;
    var framePiece = new Data.FramePiece.FramePiece { Id = id, OrderId = null };
    _repositoryMock.Setup( r => r.GetByIdAsync( id ) ).ReturnsAsync( framePiece );

    // Act
    await _sut.AttachToOrder( id, orderId );

    // Assert
    framePiece.OrderId.Should().Be( orderId );
    _repositoryMock.Verify( r => r.UpdateAsync( framePiece ), Times.Once );
  }

  [Fact]
  public async Task AttachToOrder_ShouldThrowArgumentException_WhenFramePieceDoesNotExist()
  {
    // Arrange
    var id = 1;
    _repositoryMock.Setup( r => r.GetByIdAsync( id ) ).ReturnsAsync( ( Data.FramePiece.FramePiece )null! );

    // Act
    var act = () => _sut.AttachToOrder( id, 10 );

    // Assert
    await act.Should().ThrowAsync<ArgumentException>()
             .WithMessage( "Frame piece with provided id not found" );
  }

  [Fact]
  public async Task DetachFromOrder_ShouldClearOrderId_WhenFramePieceExists()
  {
    // Arrange
    var id = 1;
    var framePiece = new Data.FramePiece.FramePiece { Id = id, OrderId = 10 };
    _repositoryMock.Setup( r => r.GetByIdAsync( id ) ).ReturnsAsync( framePiece );

    // Act
    await _sut.DetachFromOrder( id );

    // Assert
    framePiece.OrderId.Should().BeNull();
    _repositoryMock.Verify( r => r.UpdateAsync( framePiece ), Times.Once );
  }

  [Fact]
  public async Task AddAsync_ShouldThrowArgumentException_WhenLengthIsZeroOrLess()
  {
    // Arrange
    var dto = new FramePieceEditDto { Length = 0 };

    // Act
    var act = () => _sut.AddAsync( dto );

    // Assert
    await act.Should().ThrowAsync<ArgumentException>()
             .WithMessage( "*Długość musi być większa od zera*" );
  }

  [Fact]
  public async Task AddAsync_ShouldCallRepositoryAdd_WhenDataIsValid()
  {
    // Arrange
    var dto = new FramePieceEditDto { Length = 100, FrameId = 1 };

    // Act
    await _sut.AddAsync( dto );

    // Assert
    _repositoryMock.Verify( r => r.AddAsync( It.Is<Data.FramePiece.FramePiece>( fp => fp.Length == 100 ) ),
      Times.Once );
  }

  [Fact]
  public async Task GetAllAsync_ShouldApplyFilters()
  {
    // Arrange
    var filters = new FramePieceFiltersDto
    {
      FrameId = 1,
      OrderId = 10,
      IsDamaged = true,
      IsInStock = false,
      IsOrdered = true,
      MinLength = 50,
      MaxLength = 150
    };

    _repositoryMock.Setup( r => r.GetAllAsync(
                     It.IsAny<Expression<Func<Data.FramePiece.FramePiece, bool>>>(),
                     It.IsAny<Func<IQueryable<Data.FramePiece.FramePiece>,
                       IOrderedQueryable<Data.FramePiece.FramePiece>>>(),
                     It.IsAny<int>(),
                     It.IsAny<int>() ) )
                   .ReturnsAsync( ( new List<Data.FramePiece.FramePiece>(), 0 ) );

    // Act
    await _sut.GetAllAsync( filters );

    // Assert
    _repositoryMock.Verify( r => r.GetAllAsync(
      It.Is<Expression<Func<Data.FramePiece.FramePiece, bool>>>( exp =>
        exp.ToString().Contains( "FrameId == 1" ) &&
        exp.ToString().Contains( "OrderId == 10" ) &&
        exp.ToString().Contains( "IsDamaged == True" ) &&
        exp.ToString().Contains( "IsInStock == False" ) &&
        exp.ToString().Contains( "OrderId != null" ) &&
        exp.ToString().Contains( "Length >= 50" ) &&
        exp.ToString().Contains( "Length <= 150" )
      ),
      It.IsAny<Func<IQueryable<Data.FramePiece.FramePiece>, IOrderedQueryable<Data.FramePiece.FramePiece>>>(),
      It.IsAny<int>(),
      It.IsAny<int>() ), Times.Once );
  }

  [Fact]
  public async Task GetAllAsync_ShouldApplySorting()
  {
    // Arrange
    var filters = new FramePieceFiltersDto
    {
      Sort = "length desc"
    };

    _repositoryMock.Setup( r => r.GetAllAsync(
                     It.IsAny<Expression<Func<Data.FramePiece.FramePiece, bool>>>(),
                     It.IsAny<Func<IQueryable<Data.FramePiece.FramePiece>,
                       IOrderedQueryable<Data.FramePiece.FramePiece>>>(),
                     It.IsAny<int>(),
                     It.IsAny<int>() ) )
                   .ReturnsAsync( ( new List<Data.FramePiece.FramePiece>(), 0 ) );

    // Act
    await _sut.GetAllAsync( filters );

    // Assert
    _repositoryMock.Verify( r => r.GetAllAsync(
      It.IsAny<Expression<Func<Data.FramePiece.FramePiece, bool>>>(),
      It.Is<Func<IQueryable<Data.FramePiece.FramePiece>, IOrderedQueryable<Data.FramePiece.FramePiece>>>( sort =>
        sort != null
      ),
      It.IsAny<int>(),
      It.IsAny<int>() ), Times.Once );
  }
}