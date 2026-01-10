using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Moq;
using OprawaObrazow.Data;
using OprawaObrazow.Modules.Base;
using OprawaObrazow.Modules.Supplier;
using OprawaObrazow.Modules.Supplier.Dto;

namespace OprawaObrazow.Tests.Modules.Supplier;

public class SupplierServiceTests
{
  private readonly IMapper _mapper;
  private readonly Mock<IBaseRepository<Data.Supplier.Supplier>> _repositoryMock;
  private readonly SupplierService _sut;

  public SupplierServiceTests()
  {
    _repositoryMock = new Mock<IBaseRepository<Data.Supplier.Supplier>>();
    var config = new MapperConfiguration( cfg => cfg.AddProfile<MappingProfile>() );
    _mapper = config.CreateMapper();
    _sut = new SupplierService( _repositoryMock.Object, _mapper );
  }

  [Fact]
  public async Task GetAllAsync_ShouldApplySearchFilter()
  {
    // Arrange
    var filters = new SupplierFiltersDto { Search = "ABC" };
    _repositoryMock.Setup( r => r.GetAllAsync(
                     It.IsAny<Expression<Func<Data.Supplier.Supplier, bool>>>(),
                     It.IsAny<Func<IQueryable<Data.Supplier.Supplier>, IOrderedQueryable<Data.Supplier.Supplier>>>(),
                     It.IsAny<int?>(),
                     It.IsAny<int?>(),
                     It.IsAny<Expression<Func<Data.Supplier.Supplier, object>>[]>() ) )
                   .ReturnsAsync( ( new List<Data.Supplier.Supplier>(), 0 ) );

    // Act
    await _sut.GetAllAsync( filters );

    // Assert
    _repositoryMock.Verify( r => r.GetAllAsync(
      It.Is<Expression<Func<Data.Supplier.Supplier, bool>>>( exp =>
        exp.ToString().Contains( "Name.Contains(\"ABC\")" )
      ),
      It.IsAny<Func<IQueryable<Data.Supplier.Supplier>, IOrderedQueryable<Data.Supplier.Supplier>>>(),
      It.IsAny<int?>(),
      It.IsAny<int?>(),
      It.IsAny<Expression<Func<Data.Supplier.Supplier, object>>[]>() ), Times.Once );
  }

  [Fact]
  public async Task AddAsync_ShouldThrowException_WhenSupplierNameAlreadyExists()
  {
    // Arrange
    var dto = new SupplierEditDto { Name = "Existing" };
    _repositoryMock.Setup( r => r.GetAllAsync(
                     It.IsAny<Expression<Func<Data.Supplier.Supplier, bool>>>(),
                     It.IsAny<Func<IQueryable<Data.Supplier.Supplier>, IOrderedQueryable<Data.Supplier.Supplier>>>(),
                     It.IsAny<int?>(),
                     It.IsAny<int?>(),
                     It.IsAny<Expression<Func<Data.Supplier.Supplier, object>>[]>() ) )
                   .ReturnsAsync( ( new List<Data.Supplier.Supplier> { new() { Name = "Existing" } }, 1 ) );

    // Act
    var act = () => _sut.AddAsync( dto );

    // Assert
    await act.Should().ThrowAsync<ArgumentException>()
             .WithMessage( "*Dostawca o tej nazwie już istnieje*" );
  }
}