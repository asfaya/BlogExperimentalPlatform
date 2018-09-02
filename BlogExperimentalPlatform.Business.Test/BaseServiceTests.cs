namespace BlogExperimentalPlatform.Business.Test
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Repositories;
    using BlogExperimentalPlatform.Business.ServiceImplementations;
    using BlogExperimentalPlatform.Utils;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Xunit;
    using FluentAssertions;

    public class BaseServiceTests
    {
        #region Members
        private readonly Mock<IBaseRepository<Entity>> baseRepositoryMock;
        #endregion

        #region Constructor
        public BaseServiceTests()
        {
            baseRepositoryMock = new Mock<IBaseRepository<Entity>>();
        }
        #endregion

        #region Tests

        #region GetAsync
        [Fact]
        public async Task GetAsync_WhenExists_ReturnsEntity()
        {
            // Arrange
            var entity = new BaseEntityImplementation()
            {
                Id = 1,
                Deleted = false
            };

            baseRepositoryMock
                .Setup(m => m.GetSingleAsync(It.IsAny<int>()))
                .ReturnsAsync(entity);

            var service = GetService();

            // Act
            var result = await service.GetAsync(1);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(entity);
            //Assert.Equal(entity.Id, result.Id);
            //Assert.Equal(entity.Deleted, result.Deleted);

            baseRepositoryMock.Verify(m => m.GetSingleAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_WhenNotExists_ReturnsNull()
        {
            // Arrange
            Entity entity = null;

            baseRepositoryMock
                .Setup(m => m.GetSingleAsync(It.IsAny<int>()))
                .ReturnsAsync(entity);

            var service = GetService();

            // Act
            var result = await service.GetAsync(1);

            // Assert
            Assert.Null(result);

            baseRepositoryMock.Verify(m => m.GetSingleAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_WithIncludeWhenExists_ReturnsEntity()
        {
            // Arrange
            var entity = new BaseEntityImplementation()
            {
                Id = 1,
                RelatedEntity = new BaseEntityImplementation()
                {
                    Id = 2,
                    RelatedEntity = null,
                    Deleted = false
                },
                Deleted = false
            };

            baseRepositoryMock
                .Setup(m => m.GetSingleAsync(It.IsAny<int>(), It.IsAny<Expression<Func<Entity, object>>>()))
                .ReturnsAsync(entity);

            var service = GetService();

            // Act
            var result = await service.GetAsync(1, e => ((BaseEntityImplementation)e).RelatedEntity);

            // Assert
            Assert.NotNull(result);
            result.Should()
                .BeEquivalentTo(entity, opts => opts.IncludingProperties().IncludingNestedObjects());

            baseRepositoryMock.Verify(m => m.GetSingleAsync(It.IsAny<int>(), It.IsAny<Expression<Func<Entity, object>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_WithIncludeWhenNotExists_ReturnsNull()
        {
            // Arrange
            Entity entity = null;

            baseRepositoryMock
                .Setup(m => m.GetSingleAsync(It.IsAny<int>(), It.IsAny<Expression<Func<Entity, object>>>()))
                .ReturnsAsync(entity);

            var service = GetService();

            // Act
            var result = await service.GetAsync(1, e => ((BaseEntityImplementation)e).RelatedEntity);

            // Assert
            Assert.Null(result);

            baseRepositoryMock.Verify(m => m.GetSingleAsync(It.IsAny<int>(), It.IsAny<Expression<Func<Entity, object>>>()), Times.Once);
        }
        #endregion

        #region GetAllAsync
        [Fact]
        public async Task GetAllAsync_WhenExists_ReturnsCollection()
        {
            // Arrange
            var entity = new BaseEntityImplementation()
            {
                Id = 1,
                Deleted = false
            };
            var entities = new List<Entity>() { entity };

            baseRepositoryMock
                .Setup(m => m.GetAllAsync())
                .ReturnsAsync(entities);

            var service = GetService();

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entities, result);

            baseRepositoryMock.Verify(m => m.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenNotExist_ReturnsEmptyCollection()
        {
            // Arrange
            ICollection<Entity> baseCollection = new List<Entity>();

            baseRepositoryMock
                .Setup(m => m.GetAllAsync())
                .ReturnsAsync(baseCollection);

            var service = GetService();

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(baseCollection, result);

            baseRepositoryMock.Verify(m => m.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WithIncludeWhenExists_ReturnsCollection()
        {
            // Arrange
            var entity = new BaseEntityImplementation()
            {
                Id = 1,
                RelatedEntity = new BaseEntityImplementation()
                {
                    Id = 2,
                    RelatedEntity = null, 
                    Deleted = false
                },
                Deleted = false
            };
            var entities = new List<Entity>() { entity };

            baseRepositoryMock
                .Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Entity, object>>>()))
                .ReturnsAsync(entities);

            var service = GetService();

            // Act
            var result = await service.GetAllAsync(e => ((BaseEntityImplementation)e).RelatedEntity);

            // Assert
            Assert.NotNull(result);
            result.Should()
                .BeEquivalentTo(entities, options => options.IncludingProperties().IncludingNestedObjects());
            baseRepositoryMock.Verify(m => m.GetAllAsync(It.IsAny<Expression<Func<Entity, object>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WithIncludeWhenNotExists_ReturnsEmptyCollection()
        {
            // Arrange
            ICollection<Entity> baseCollection = new List<Entity>();

            baseRepositoryMock
                .Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Entity, object>>>()))
                .ReturnsAsync(baseCollection);

            var service = GetService();

            // Act
            var result = await service.GetAllAsync(e => ((BaseEntityImplementation)e).RelatedEntity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(baseCollection, result);

            baseRepositoryMock.Verify(m => m.GetAllAsync(It.IsAny<Expression<Func<Entity, object>>>()), Times.Once);
        }
        #endregion

        #region GetPaginatedAsync
        [Fact]
        public async Task GetPaginatedAsync_WhenExists_ReturnsEntityPage()
        {
            // Arrange
            var entity = new BaseEntityImplementation()
            {
                Id = 1,
                Deleted = false
            };
            var entities = new List<Entity>() { entity };

            EntityPage<Entity> page = new EntityPage<Entity>();
            page.TotalEntities = entities.Count;
            page.TotalPages = 1;
            page.Entities = entities;

            baseRepositoryMock
                .Setup(m => m.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(entities);
            baseRepositoryMock
                .Setup(m => m.CountAsync())
                .ReturnsAsync(entities.Count);

            var service = GetService();

            // Act
            var result = await service.GetPaginatedAsync(1, 10);

            // Assert
            Assert.NotNull(result);
            result.Should()
                .BeEquivalentTo(page, opts => opts.IncludingProperties().IncludingNestedObjects());

            baseRepositoryMock.Verify(m => m.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            baseRepositoryMock.Verify(m => m.CountAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPaginatedAsync_WhenNotExists_ReturnsEmptyEntityPage()
        {
            // Arrange
            var entities = new List<Entity>();

            EntityPage<Entity> page = new EntityPage<Entity>();
            page.TotalEntities = entities.Count;
            page.TotalPages = 0;
            page.Entities = entities;

            baseRepositoryMock
                .Setup(m => m.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(entities);
            baseRepositoryMock
                .Setup(m => m.CountAsync())
                .ReturnsAsync(entities.Count);

            var service = GetService();

            // Act
            var result = await service.GetPaginatedAsync(1, 10);

            // Assert
            Assert.NotNull(result);
            result.Should()
                .BeEquivalentTo(page, opts => opts.IncludingProperties().IncludingNestedObjects());

            baseRepositoryMock.Verify(m => m.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            baseRepositoryMock.Verify(m => m.CountAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPaginatedAsync_WithIncludesWhenExists_ReturnsEntityPage()
        {
            // Arrange
            var entity = new BaseEntityImplementation()
            {
                Id = 1,
                RelatedEntity = new BaseEntityImplementation
                {
                    Id = 2,
                    RelatedEntity = null,
                    Deleted = false
                },
                Deleted = false
            };
            var entities = new List<Entity>() { entity };

            EntityPage<Entity> page = new EntityPage<Entity>();
            page.TotalEntities = entities.Count;
            page.TotalPages = 1;
            page.Entities = entities;

            baseRepositoryMock
                .Setup(m => m.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Entity, object>>>()))
                .ReturnsAsync(entities);
            baseRepositoryMock
                .Setup(m => m.CountAsync())
                .ReturnsAsync(entities.Count);

            var service = GetService();

            // Act
            var result = await service.GetPaginatedAsync(1, 10, e => ((BaseEntityImplementation)e).RelatedEntity);

            // Assert
            Assert.NotNull(result);
            result.Should()
                .BeEquivalentTo(page, opts => opts.IncludingProperties().IncludingNestedObjects());

            baseRepositoryMock.Verify(m => m.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Entity, object>>>()), Times.Once);
            baseRepositoryMock.Verify(m => m.CountAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPaginatedAsync_WithIncludesWhenNotExists_ReturnsEmptyEntityPage()
        {
            // Arrange
            var entities = new List<Entity>();

            EntityPage<Entity> page = new EntityPage<Entity>();
            page.TotalEntities = entities.Count;
            page.TotalPages = 0;
            page.Entities = entities;

            baseRepositoryMock
                .Setup(m => m.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Entity, object>>>()))
                .ReturnsAsync(entities);
            baseRepositoryMock
                .Setup(m => m.CountAsync())
                .ReturnsAsync(entities.Count);

            var service = GetService();

            // Act
            var result = await service.GetPaginatedAsync(1, 10, e => ((BaseEntityImplementation)e).RelatedEntity);

            // Assert
            Assert.NotNull(result);
            result.Should()
                .BeEquivalentTo(page, opts => opts.IncludingProperties().IncludingNestedObjects());

            baseRepositoryMock.Verify(m => m.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Entity, object>>>()), Times.Once);
            baseRepositoryMock.Verify(m => m.CountAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPaginatedAsync_WithPredicateIncludesWhenExists_ReturnsEntityPage()
        {
            // Arrange
            var entity = new BaseEntityImplementation()
            {
                Id = 1,
                RelatedEntity = new BaseEntityImplementation
                {
                    Id = 2,
                    RelatedEntity = null,
                    Deleted = false
                },
                Deleted = false
            };
            var entities = new List<Entity>() { entity };

            EntityPage<Entity> page = new EntityPage<Entity>();
            page.TotalEntities = entities.Count;
            page.TotalPages = 1;
            page.Entities = entities;

            baseRepositoryMock
                .Setup(m => m.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Entity, bool>>>(),  It.IsAny<Expression<Func<Entity, object>>>()))
                .ReturnsAsync(entities);
            baseRepositoryMock
                .Setup(m => m.CountAsync(It.IsAny<Expression<Func<Entity, bool>>>()))
                .ReturnsAsync(entities.Count);

            var service = GetService();

            // Act
            var result = await service.GetPaginatedAsync(1, 10, e => e.Id == 1, e => ((BaseEntityImplementation)e).RelatedEntity);

            // Assert
            Assert.NotNull(result);
            result.Should()
                .BeEquivalentTo(page, opts => opts.IncludingProperties().IncludingNestedObjects());

            baseRepositoryMock.Verify(m => m.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Entity, bool>>>(), It.IsAny<Expression<Func<Entity, object>>>()), Times.Once);
            baseRepositoryMock.Verify(m => m.CountAsync(It.IsAny<Expression<Func<Entity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetPaginatedAsync_WithPredicateIncludesNotExists_ReturnsEmptyEntityPage()
        {
            // Arrange
            var entities = new List<Entity>();

            EntityPage<Entity> page = new EntityPage<Entity>();
            page.TotalEntities = 0;
            page.TotalPages = 0;
            page.Entities = entities;

            baseRepositoryMock
                .Setup(m => m.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Entity, bool>>>(), It.IsAny<Expression<Func<Entity, object>>>()))
                .ReturnsAsync(entities);
            baseRepositoryMock
                .Setup(m => m.CountAsync(It.IsAny<Expression<Func<Entity, bool>>>()))
                .ReturnsAsync(entities.Count);

            var service = GetService();

            // Act
            var result = await service.GetPaginatedAsync(1, 10, e => e.Id == 1, e => ((BaseEntityImplementation)e).RelatedEntity);

            // Assert
            Assert.NotNull(result);
            result.Should()
                .BeEquivalentTo(page, opts => opts.IncludingProperties().IncludingNestedObjects());

            baseRepositoryMock.Verify(m => m.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Entity, bool>>>(), It.IsAny<Expression<Func<Entity, object>>>()), Times.Once);
            baseRepositoryMock.Verify(m => m.CountAsync(It.IsAny<Expression<Func<Entity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetPaginatedAsync_WithPredicateIncludesOrderWhenExists_ReturnsEntityPage()
        {
            // Arrange
            var entity = new BaseEntityImplementation()
            {
                Id = 1,
                RelatedEntity = new BaseEntityImplementation
                {
                    Id = 2,
                    RelatedEntity = null,
                    Deleted = false
                },
                Deleted = false
            };
            var entities = new List<Entity>() { entity };

            EntityPage<Entity> page = new EntityPage<Entity>();
            page.TotalEntities = entities.Count;
            page.TotalPages = 1;
            page.Entities = entities;

            baseRepositoryMock
                .Setup(m => m.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Entity, bool>>>(), It.IsAny<Expression<Func<Entity, object>>>(), It.IsAny<bool>(),  It.IsAny<Expression<Func<Entity, object>>>()))
                .ReturnsAsync(entities);
            baseRepositoryMock
                .Setup(m => m.CountAsync(It.IsAny<Expression<Func<Entity, bool>>>()))
                .ReturnsAsync(entities.Count);

            var service = GetService();

            // Act
            var result = await service.GetPaginatedAsync(1, 10, e => e.Id == 1, e => e.Id, true, e => ((BaseEntityImplementation)e).RelatedEntity);

            // Assert
            Assert.NotNull(result);
            result.Should()
                .BeEquivalentTo(page, opts => opts.IncludingProperties().IncludingNestedObjects());

            baseRepositoryMock.Verify(m => m.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Entity, bool>>>(), It.IsAny<Expression<Func<Entity, object>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Entity, object>>>()), Times.Once);
            baseRepositoryMock.Verify(m => m.CountAsync(It.IsAny<Expression<Func<Entity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetPaginatedAsync_WithPredicateIncludesOrderNotExists_ReturnsEmptyEntityPage()
        {
            // Arrange
            EntityPage<Entity> page = new EntityPage<Entity>();
            page.TotalEntities = 0;
            page.TotalPages = 0;
            page.Entities = null;

            baseRepositoryMock
                .Setup(m => m.CountAsync(It.IsAny<Expression<Func<Entity, bool>>>()))
                .ReturnsAsync(0);

            var service = GetService();

            // Act
            var result = await service.GetPaginatedAsync(1, 10, e => e.Id == 1, e => e.Id, true, e => ((BaseEntityImplementation)e).RelatedEntity);

            // Assert
            Assert.NotNull(result);
            result.Should()
                .BeEquivalentTo(page, opts => opts.IncludingProperties().IncludingNestedObjects());

            baseRepositoryMock.Verify(m => m.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Entity, bool>>>(), It.IsAny<Expression<Func<Entity, object>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Entity, object>>>()), Times.Never);
            baseRepositoryMock.Verify(m => m.CountAsync(It.IsAny<Expression<Func<Entity, bool>>>()), Times.Once);
        }
        #endregion

        #region GetFilteredAsync
        [Fact]
        public async Task GetFilteredAsync_WhenExists_ReturnsCollection()
        {
            // Arrange
            var entity = new BaseEntityImplementation()
            {
                Id = 1,
                Deleted = false
            };
            var entities = new List<Entity>() { entity };

            baseRepositoryMock
                .Setup(m => m.GetFilteredAsync(It.IsAny<Expression<Func<Entity, bool>>>()))
                .ReturnsAsync(entities);

            var service = GetService();

            // Act
            var result = await service.GetFilteredAsync(e => e.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entities, result);

            baseRepositoryMock.Verify(m => m.GetFilteredAsync(It.IsAny<Expression<Func<Entity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetFilteredAsync_WhenNotExist_ReturnsEmptyCollection()
        {
            // Arrange
            ICollection<Entity> baseCollection = new List<Entity>();

            baseRepositoryMock
                .Setup(m => m.GetFilteredAsync(It.IsAny<Expression<Func<Entity, bool>>>()))
                .ReturnsAsync(baseCollection);

            var service = GetService();

            // Act
            var result = await service.GetFilteredAsync(e => e.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(baseCollection, result);

            baseRepositoryMock.Verify(m => m.GetFilteredAsync(It.IsAny<Expression<Func<Entity, bool>>>()), Times.Once);
        }
        #endregion

        #region AddOrUpdateAsync
        [Fact]
        public async Task AddOrUpdateAsync_WhenExisting_ReturnsEntity()
        {
            // Arrange
            var entity = new BaseEntityImplementation()
            {
                Id = 1,
                Deleted = false
            };

            var resultEntity = new BaseEntityImplementation()
            {
                Id = 1,
                Deleted = false
            };

            baseRepositoryMock
                .Setup(m => m.AddOrUpdateAsync(It.IsAny<Entity>()))
                .ReturnsAsync(resultEntity);

            var service = GetService();

            // Act
            var result = await service.AddOrUpdateAsync(entity);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(resultEntity);

            baseRepositoryMock.Verify(m => m.AddOrUpdateAsync(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public async Task AddOrUpdateAsync_WhenNew_ReturnsEntityWithId()
        {
            // Arrange
            var entity = new BaseEntityImplementation()
            {
                Id = 0,
                Deleted = false
            };

            var resultEntity = new BaseEntityImplementation()
            {
                Id = 1,
                Deleted = false
            };

            baseRepositoryMock
                .Setup(m => m.AddOrUpdateAsync(It.IsAny<Entity>()))
                .ReturnsAsync(resultEntity);

            var service = GetService();

            // Act
            var result = await service.AddOrUpdateAsync(entity);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(entity.Id, result.Id);
            Assert.Equal(entity.Deleted, result.Deleted);

            baseRepositoryMock.Verify(m => m.AddOrUpdateAsync(It.IsAny<Entity>()), Times.Once);
        }
        #endregion

        #endregion

        #region Private Helper Methods
        private BaseService<Entity> GetService()
        {
            var service = new BaseServiceImplementation(baseRepositoryMock.Object);

            return service;
        }
        #endregion
    }
}
