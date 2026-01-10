using AutoMapper;
using FluentAssertions;
using Moq;
using OprawaObrazow.Data;
using OprawaObrazow.Modules.Base;
using OprawaObrazow.Modules.Frame;
using OprawaObrazow.Modules.Frame.Dto;
using Xunit;

namespace OprawaObrazow.Tests.Modules.Frame;

public class FrameServiceTests
{
    private readonly Mock<IBaseRepository<OprawaObrazow.Data.Frame.Frame>> _repositoryMock;
    private readonly IMapper _mapper;
    private readonly FrameService _sut;

    public FrameServiceTests()
    {
        _repositoryMock = new Mock<IBaseRepository<OprawaObrazow.Data.Frame.Frame>>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _sut = new FrameService(_repositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_WhenWidthIsNegative()
    {
        // Arrange
        var dto = new FrameEditDto { Width = -1 };

        // Act
        var act = () => _sut.AddAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Szerokość nie może być ujemna.*");
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_WhenPriceIsNegative()
    {
        // Arrange
        var dto = new FrameEditDto { Price = -10 };

        // Act
        var act = () => _sut.AddAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Cena nie może być ujemna.*");
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_WhenCodeAlreadyExists()
    {
        // Arrange
        var dto = new FrameEditDto { Code = "F123" };
        _repositoryMock.Setup(r => r.GetAllAsync(
                It.IsAny<System.Linq.Expressions.Expression<System.Func<OprawaObrazow.Data.Frame.Frame, bool>>>(),
                It.IsAny<System.Func<System.Linq.IQueryable<OprawaObrazow.Data.Frame.Frame>, System.Linq.IOrderedQueryable<OprawaObrazow.Data.Frame.Frame>>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<System.Linq.Expressions.Expression<System.Func<OprawaObrazow.Data.Frame.Frame, object>>[]>()))
            .ReturnsAsync((new List<OprawaObrazow.Data.Frame.Frame> { new() { Code = "F123" } }, 1));

        // Act
        var act = () => _sut.AddAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Rama o tym kodzie już istnieje.*");
    }

    [Fact]
    public async Task GetAllAsync_ShouldApplyFilters()
    {
        // Arrange
        var filters = new FrameFiltersDto
        {
            Search = "GOLD",
            ColorId = 1,
            SupplierId = 2,
            MinWidth = 10,
            MaxWidth = 50,
            HasDecoration = true
        };

        _repositoryMock.Setup(r => r.GetAllAsync(
                It.IsAny<System.Linq.Expressions.Expression<System.Func<OprawaObrazow.Data.Frame.Frame, bool>>>(),
                It.IsAny<System.Func<System.Linq.IQueryable<OprawaObrazow.Data.Frame.Frame>, System.Linq.IOrderedQueryable<OprawaObrazow.Data.Frame.Frame>>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<System.Linq.Expressions.Expression<System.Func<OprawaObrazow.Data.Frame.Frame, object>>[]>()))
            .ReturnsAsync((new List<OprawaObrazow.Data.Frame.Frame>(), 0));

        // Act
        await _sut.GetAllAsync(filters);

        // Assert
        _repositoryMock.Verify(r => r.GetAllAsync(
            It.Is<System.Linq.Expressions.Expression<System.Func<OprawaObrazow.Data.Frame.Frame, bool>>>(exp =>
                exp.ToString().Contains("Code.Contains(\"GOLD\")") &&
                exp.ToString().Contains("ColorId == 1") &&
                exp.ToString().Contains("SupplierId == 2") &&
                exp.ToString().Contains("Width >= 10") &&
                exp.ToString().Contains("Width <= 50") &&
                exp.ToString().Contains("HasDecoration == True")
            ),
            It.IsAny<System.Func<System.Linq.IQueryable<OprawaObrazow.Data.Frame.Frame>, System.Linq.IOrderedQueryable<OprawaObrazow.Data.Frame.Frame>>>(),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<System.Linq.Expressions.Expression<System.Func<OprawaObrazow.Data.Frame.Frame, object>>[]>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldApplySorting()
    {
        // Arrange
        var filters = new FrameFiltersDto
        {
            Sort = "width desc"
        };

        _repositoryMock.Setup(r => r.GetAllAsync(
                It.IsAny<System.Linq.Expressions.Expression<System.Func<OprawaObrazow.Data.Frame.Frame, bool>>>(),
                It.IsAny<System.Func<System.Linq.IQueryable<OprawaObrazow.Data.Frame.Frame>, System.Linq.IOrderedQueryable<OprawaObrazow.Data.Frame.Frame>>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<System.Linq.Expressions.Expression<System.Func<OprawaObrazow.Data.Frame.Frame, object>>[]>()))
            .ReturnsAsync((new List<OprawaObrazow.Data.Frame.Frame>(), 0));

        // Act
        await _sut.GetAllAsync(filters);

        // Assert
        _repositoryMock.Verify(r => r.GetAllAsync(
            It.IsAny<System.Linq.Expressions.Expression<System.Func<OprawaObrazow.Data.Frame.Frame, bool>>>(),
            It.Is<System.Func<System.Linq.IQueryable<OprawaObrazow.Data.Frame.Frame>, System.Linq.IOrderedQueryable<OprawaObrazow.Data.Frame.Frame>>>(sort =>
                sort != null
            ),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<System.Linq.Expressions.Expression<System.Func<OprawaObrazow.Data.Frame.Frame, object>>[]>()), Times.Once);
    }
}
