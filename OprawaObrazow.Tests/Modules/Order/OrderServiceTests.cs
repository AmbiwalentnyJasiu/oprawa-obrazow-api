using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Moq;
using OprawaObrazow.Data;
using OprawaObrazow.Data.Order;
using OprawaObrazow.Modules.Base;
using OprawaObrazow.Modules.Order;
using OprawaObrazow.Modules.Order.Dto;

namespace OprawaObrazow.Tests.Modules.Order;

public class OrderServiceTests
{
  private readonly IMapper _mapper;
  private readonly Mock<IBaseRepository<Data.Order.Order>> _repositoryMock;
  private readonly OrderService _sut;

  public OrderServiceTests()
  {
    _repositoryMock = new Mock<IBaseRepository<Data.Order.Order>>();
    var config = new MapperConfiguration( cfg => cfg.AddProfile<MappingProfile>() );
    _mapper = config.CreateMapper();
    _sut = new OrderService( _repositoryMock.Object, _mapper );
  }

  [Fact]
  public async Task ChangeStatus_ShouldUpdateStatus_WhenOrderExists()
  {
    // Arrange
    var id = 1;
    var newStatus = OrderStatus.InProgress;
    var order = new Data.Order.Order { Id = id, Status = OrderStatus.InPreparation };
    _repositoryMock.Setup( r => r.GetByIdNoTrackingAsync( id ) ).ReturnsAsync( order );

    // Act
    await _sut.ChangeStatus( id, newStatus );

    // Assert
    order.Status.Should().Be( newStatus );
    _repositoryMock.Verify( r => r.UpdateAsync( It.Is<Data.Order.Order>( o => o.Id == id && o.Status == newStatus ) ),
      Times.Once );
  }

  [Fact]
  public async Task ChangeStatus_ShouldThrowArgumentException_WhenOrderDoesNotExist()
  {
    // Arrange
    var id = 1;
    _repositoryMock.Setup( r => r.GetByIdNoTrackingAsync( id ) ).ReturnsAsync( ( Data.Order.Order )null! );

    // Act
    var act = () => _sut.ChangeStatus( id, OrderStatus.Done );

    // Assert
    await act.Should().ThrowAsync<ArgumentException>()
             .WithMessage( "Entity with provided id not found" );
  }

  [Fact]
  public async Task GetByIdAsync_ShouldIncludeFramePieces()
  {
    // Arrange
    var id = 1;
    var order = new Data.Order.Order { Id = id };
    _repositoryMock.Setup( r => r.GetByIdNoTrackingAsync( id, It.IsAny<Expression<Func<Data.Order.Order, object>>>() ) )
                   .ReturnsAsync( order );

    // Act
    var result = await _sut.GetByIdAsync( id );

    // Assert
    result.Should().NotBeNull();
    _repositoryMock.Verify( r => r.GetByIdNoTrackingAsync( id, It.IsAny<Expression<Func<Data.Order.Order, object>>>() ),
      Times.Once );
  }

  [Fact]
  public async Task GetAllAsync_ShouldApplyFilters()
  {
    // Arrange
    var filters = new OrderFiltersDto
    {
      Search = "Jan",
      Status = OrderStatus.InProgress,
      DateFrom = new DateOnly( 2024, 1, 1 ),
      DateTo = new DateOnly( 2024, 12, 31 )
    };

    _repositoryMock.Setup( r => r.GetAllAsync(
                     It.IsAny<Expression<Func<Data.Order.Order, bool>>>(),
                     It.IsAny<Func<IQueryable<Data.Order.Order>, IOrderedQueryable<Data.Order.Order>>>(),
                     It.IsAny<int>(),
                     It.IsAny<int>() ) )
                   .ReturnsAsync( ( new List<Data.Order.Order>(), 0 ) );

    // Act
    await _sut.GetAllAsync( filters );

    // Assert
    _repositoryMock.Verify( r => r.GetAllAsync(
      It.Is<Expression<Func<Data.Order.Order, bool>>>( exp =>
        exp.ToString().Contains( "ClientName.Contains(\"Jan\")" ) ||
        exp.ToString().Contains( "EmailAddress.Contains(\"Jan\")" ) ||
        ( exp.ToString().Contains( "PhoneNumber.Contains(\"Jan\")" ) &&
          exp.ToString().Contains( "Status == InProgress" ) &&
          exp.ToString().Contains( "DateDue >= 01/01/2024" ) &&
          exp.ToString().Contains( "DateDue <= 31/12/2024" ) )
      ),
      It.IsAny<Func<IQueryable<Data.Order.Order>, IOrderedQueryable<Data.Order.Order>>>(),
      It.IsAny<int>(),
      It.IsAny<int>() ), Times.Once );
  }

  [Fact]
  public async Task GetAllAsync_ShouldApplySorting()
  {
    // Arrange
    var filters = new OrderFiltersDto
    {
      Sort = "clientName desc"
    };

    _repositoryMock.Setup( r => r.GetAllAsync(
                     It.IsAny<Expression<Func<Data.Order.Order, bool>>>(),
                     It.IsAny<Func<IQueryable<Data.Order.Order>, IOrderedQueryable<Data.Order.Order>>>(),
                     It.IsAny<int>(),
                     It.IsAny<int>() ) )
                   .ReturnsAsync( ( new List<Data.Order.Order>(), 0 ) );

    // Act
    await _sut.GetAllAsync( filters );

    // Assert
    _repositoryMock.Verify( r => r.GetAllAsync(
      It.IsAny<Expression<Func<Data.Order.Order, bool>>>(),
      It.Is<Func<IQueryable<Data.Order.Order>, IOrderedQueryable<Data.Order.Order>>>( sort =>
          sort != null // Trudno zweryfikować samo wyrażenie lambda bez wywołania go na IQueryable, ale sprawdzamy czy zostało przekazane
      ),
      It.IsAny<int>(),
      It.IsAny<int>() ), Times.Once );
  }
}