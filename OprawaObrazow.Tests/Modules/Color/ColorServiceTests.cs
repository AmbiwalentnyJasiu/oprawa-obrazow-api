using AutoMapper;
using FluentAssertions;
using Moq;
using OprawaObrazow.Data;
using OprawaObrazow.Modules.Base;
using OprawaObrazow.Modules.Color;
using OprawaObrazow.Modules.Color.Dto;
using Xunit;

namespace OprawaObrazow.Tests.Modules.Color;

public class ColorServiceTests
{
    private readonly Mock<IBaseRepository<OprawaObrazow.Data.Color.Color>> _repositoryMock;
    private readonly IMapper _mapper;
    private readonly ColorService _sut;

    public ColorServiceTests()
    {
        _repositoryMock = new Mock<IBaseRepository<OprawaObrazow.Data.Color.Color>>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _sut = new ColorService(_repositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_WhenColorNameAlreadyExists()
    {
        // Arrange
        var dto = new ColorEditDto { Name = "Czerwony" };
        _repositoryMock.Setup(r => r.GetAllAsync(
                It.IsAny<System.Linq.Expressions.Expression<System.Func<OprawaObrazow.Data.Color.Color, bool>>>(),
                It.IsAny<System.Func<System.Linq.IQueryable<OprawaObrazow.Data.Color.Color>, System.Linq.IOrderedQueryable<OprawaObrazow.Data.Color.Color>>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<System.Linq.Expressions.Expression<System.Func<OprawaObrazow.Data.Color.Color, object>>[]>()))
            .ReturnsAsync((new List<OprawaObrazow.Data.Color.Color> { new() { Name = "Czerwony" } }, 1));

        // Act
        var act = () => _sut.AddAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Kolor o tej nazwie już istnieje.*");
    }

    [Fact]
    public async Task GetAllAsync_ShouldApplySearchFilter()
    {
        // Arrange
        var filters = new ColorFiltersDto { Search = "nieb" };
        _repositoryMock.Setup(r => r.GetAllAsync(
                It.IsAny<System.Linq.Expressions.Expression<System.Func<OprawaObrazow.Data.Color.Color, bool>>>(),
                It.IsAny<System.Func<System.Linq.IQueryable<OprawaObrazow.Data.Color.Color>, System.Linq.IOrderedQueryable<OprawaObrazow.Data.Color.Color>>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<System.Linq.Expressions.Expression<System.Func<OprawaObrazow.Data.Color.Color, object>>[]>()))
            .ReturnsAsync((new List<OprawaObrazow.Data.Color.Color>(), 0));

        // Act
        await _sut.GetAllAsync(filters);

        // Assert
        _repositoryMock.Verify(r => r.GetAllAsync(
            It.Is<System.Linq.Expressions.Expression<System.Func<OprawaObrazow.Data.Color.Color, bool>>>(exp =>
                exp.ToString().Contains("Name.Contains(\"nieb\")")
            ),
            It.IsAny<System.Func<System.Linq.IQueryable<OprawaObrazow.Data.Color.Color>, System.Linq.IOrderedQueryable<OprawaObrazow.Data.Color.Color>>>(),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<System.Linq.Expressions.Expression<System.Func<OprawaObrazow.Data.Color.Color, object>>[]>()), Times.Once);
    }
}
