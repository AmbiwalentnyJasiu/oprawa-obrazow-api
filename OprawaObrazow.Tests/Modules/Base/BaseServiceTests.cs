using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Moq;
using OprawaObrazow.Data.Base;
using OprawaObrazow.Modules.Base;
using Xunit;

namespace OprawaObrazow.Tests.Modules.Base;

public class BaseServiceTests
{
    private readonly Mock<IBaseRepository<TestEntity>> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly TestBaseService _sut;

    public BaseServiceTests()
    {
        _repositoryMock = new Mock<IBaseRepository<TestEntity>>();
        _mapperMock = new Mock<IMapper>();
        _sut = new TestBaseService(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldCallRepositoryAdd_WhenValidationSucceeds()
    {
        // Arrange
        var dto = new TestEditDto();
        var entity = new TestEntity();
        _mapperMock.Setup(m => m.Map<TestEntity>(dto)).Returns(entity);

        // Act
        await _sut.AddAsync(dto);

        // Assert
        _repositoryMock.Verify(r => r.AddAsync(entity), Times.Once);
    }

    [Fact]
    public async Task AddAsync_ShouldThrowArgumentException_WhenValidationFails()
    {
        // Arrange
        var dto = new TestEditDto { ShouldFailValidation = true };

        // Act
        var act = () => _sut.AddAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Validation failed: *");
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<TestEntity>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldCallRepositoryUpdate_WhenValidationSucceeds()
    {
        // Arrange
        var dto = new TestEditDto { Id = 1 };
        var entity = new TestEntity { Id = 1 };
        _mapperMock.Setup(m => m.Map<TestEntity>(dto)).Returns(entity);

        // Act
        await _sut.UpdateAsync(dto);

        // Assert
        _repositoryMock.Verify(r => r.UpdateAsync(entity), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallRepositoryDelete_WhenEntityExists()
    {
        // Arrange
        var id = 1;
        var entity = new TestEntity { Id = id };
        _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);

        // Act
        await _sut.DeleteAsync(id);

        // Assert
        _repositoryMock.Verify(r => r.DeleteAsync(entity), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotCallRepositoryDelete_WhenEntityDoesNotExist()
    {
        // Arrange
        var id = 1;
        _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TestEntity)null!);

        // Act
        await _sut.DeleteAsync(id);

        // Assert
        _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<TestEntity>()), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMappedDto_WhenEntityExists()
    {
        // Arrange
        var id = 1;
        var entity = new TestEntity { Id = id };
        var dto = new TestViewDto { Id = id };
        _repositoryMock.Setup(r => r.GetByIdNoTrackingAsync(id)).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<TestViewDto>(entity)).Returns(dto);

        // Act
        var result = await _sut.GetByIdAsync(id);

        // Assert
        result.Should().Be(dto);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenEntityDoesNotExist()
    {
        // Arrange
        var id = 1;
        _repositoryMock.Setup(r => r.GetByIdNoTrackingAsync(id)).ReturnsAsync((TestEntity)null!);

        // Act
        var result = await _sut.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedResponse()
    {
        // Arrange
        var filters = new TestFiltersDto { Page = 1, PageSize = 10 };
        var entities = new List<TestEntity> { new TestEntity { Id = 1 } };
        var dtos = new List<TestListDto> { new TestListDto { Id = 1 } };
        
        _repositoryMock.Setup(r => r.GetAllAsync(
            It.IsAny<Expression<Func<TestEntity, bool>>>(),
            It.IsAny<Func<IQueryable<TestEntity>, IOrderedQueryable<TestEntity>>>(),
            0, 10))
            .ReturnsAsync((entities, 1));
        
        _mapperMock.Setup(m => m.Map<TestListDto>(entities[0])).Returns(dtos[0]);

        // Act
        var result = await _sut.GetAllAsync(filters);

        // Assert
        result.Count.Should().Be(1);
        result.Items.Should().ContainSingle().Which.Id.Should().Be(1);
    }

    // Helper classes for testing
    public class TestEntity : BaseEntity { }
    public class TestEditDto { public int? Id { get; set; } public bool ShouldFailValidation { get; set; } }
    public class TestViewDto { public int Id { get; set; } }
    public class TestListDto { public int Id { get; set; } }
    public class TestFiltersDto : BaseFiltersDto { }

    public class TestBaseService(IBaseRepository<TestEntity> repo, IMapper mapper) 
        : BaseService<TestEntity, TestEditDto, TestViewDto, TestListDto, TestFiltersDto>(repo, mapper)
    {
        protected override Task<(bool isValid, Dictionary<string, List<string>>? errors)> ValidateInputEntity(TestEditDto editModel)
        {
            if (editModel.ShouldFailValidation)
            {
                return Task.FromResult<(bool, Dictionary<string, List<string>>?)>(
                    (false, new Dictionary<string, List<string>> { { "Error", new List<string> { "Validation error" } } }));
            }
            return base.ValidateInputEntity(editModel);
        }
    }
}
