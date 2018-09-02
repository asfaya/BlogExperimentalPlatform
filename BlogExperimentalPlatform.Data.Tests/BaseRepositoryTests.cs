namespace BlogExperimentalPlatform.Data.Test
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Data.Repositories;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class BaseRepositoryTests
    {
        #region Tests

        #region GetAllASync
        [Fact]
        public async Task GetAllAsync_WhenExists_ReturnsCollection()
        {
            // Arrange
            var entities = GetDataForTests();

            var context = GetContext();
            foreach (var entity in entities)
                context.Entities.Add(entity);
            context.SaveChanges();

            var repository = GetRepository(context);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(entities, opts => opts.Excluding(e => e.RelatedEntity));
        }

        [Fact]
        public async Task GetAllAsync_WhenNotExists_ReturnsEmpty()
        {
            // Arrange
            var entities = new List<Entity>();

            var context = GetContext();
            var repository = GetRepository(context);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(entities);
        }

        [Fact]
        public async Task GetAllAsync_WithIncludeWhenExists_ReturnsCollection()
        {
            // Arrange
            var entities = GetDataForTests();

            var context = GetContext();
            foreach (var entity in entities)
                context.Entities.Add(entity);
            context.SaveChanges();

            var repository = GetRepository(context);

            // Act
            var result = await repository.GetAllAsync(e => e.RelatedEntity);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(entities, opts => opts.IncludingProperties().IncludingNestedObjects());
        }

        [Fact]
        public async Task GetAllAsync_WithIncludeWhenNotExists_ReturnsEmpty()
        {
            // Arrange
            var entities = new List<Entity>();

            var context = GetContext();
            var repository = GetRepository(context);

            // Act
            var result = await repository.GetAllAsync(e => e.RelatedEntity);

            // Assert
            result.Should().BeEquivalentTo(entities);
        }
        #endregion

        #region GetPaginatedAsync 
        [Fact]
        public async Task GetPaginatedAsync_WhenExistsMoreThanOnePageSize_ReturnsCollection()
        {
            // Arrange
            var entities = GetDataForTests();
            int page = 1;
            int pageSize = 2;
            var resultEntities = entities.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var context = GetContext();
            foreach(var entity in entities)
                context.Entities.Add(entity);
            context.SaveChanges();

            var repository = GetRepository(context);

            // Act
            var result = await repository.GetPaginatedAsync(page, pageSize);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(resultEntities, opts => opts.Excluding(p => p.RelatedEntity));
        }

        [Fact]
        public async Task GetPaginatedAsync_WhenExistsLessThanOnePageSize_ReturnsCollection()
        {
            // Arrange
            var entities = GetDataForTests();
            int page = 1;
            int pageSize = 10;
            var resultEntities = entities.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var context = GetContext();
            foreach (var entity in entities)
                context.Entities.Add(entity);
            context.SaveChanges();

            var repository = GetRepository(context);

            // Act
            var result = await repository.GetPaginatedAsync(page, pageSize);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(resultEntities, opts => opts.Excluding(p => p.RelatedEntity));
        }

        [Fact]
        public async Task GetPaginatedAsync_WhenNotExists_ReturnsEmpty()
        {
            // Arrange
            var entities = new List<Entity>();

            var context = GetContext();
            var repository = GetRepository(context);

            // Act
            var result = await repository.GetPaginatedAsync(1, 10);

            // Assert
            result.Should().BeEquivalentTo(entities);
        }

        [Fact]
        public async Task GetPaginatedAsync_WithIncludeWhenExistsMoreThanOnePageSize_ReturnsCollection()
        {
            // Arrange
            var entities = GetDataForTests();
            int page = 1;
            int pageSize = 2;
            var resultEntities = entities.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var context = GetContext();
            foreach (var entity in entities)
                context.Entities.Add(entity);
            context.SaveChanges();

            var repository = GetRepository(context);

            // Act
            var result = await repository.GetPaginatedAsync(page, pageSize, e => e.RelatedEntity);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(resultEntities, opts => opts.IncludingProperties().IncludingNestedObjects());
        }

        [Fact]
        public async Task GetPaginatedAsync_WithIncludeWhenExistsLessThanOnePageSize_ReturnsCollection()
        {
            // Arrange
            var entities = GetDataForTests();
            int page = 1;
            int pageSize = 10;
            var resultEntities = entities.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var context = GetContext();
            foreach (var entity in entities)
                context.Entities.Add(entity);
            context.SaveChanges();

            var repository = GetRepository(context);

            // Act
            var result = await repository.GetPaginatedAsync(page, pageSize, e => e.RelatedEntity);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(entities, opts => opts.IncludingProperties().IncludingNestedObjects());
        }

        [Fact]
        public async Task GetPaginatedAsync_WithIncludeWhenNotExists_ReturnsEmpty()
        {
            // Arrange
            var entities = new List<Entity>();

            var context = GetContext();
            var repository = GetRepository(context);

            // Act
            var result = await repository.GetPaginatedAsync(1, 10, e => e.RelatedEntity);

            // Assert
            result.Should().BeEquivalentTo(entities);
        }

        [Fact]
        public async Task GetPaginatedAsync_WithPredicateIncludesWhenExistsMoreThanOnePageSize_ReturnsCollection()
        {
            // Arrange
            var entities = GetDataForTests();
            int page = 1;
            int pageSize = 2;
            var resultEntities = entities.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var context = GetContext();
            foreach (var entity in entities)
                context.Entities.Add(entity);
            context.SaveChanges();

            var repository = GetRepository(context);

            // Act
            var result = await repository.GetPaginatedAsync(page, pageSize, e => e.Id < 10, e => e.RelatedEntity);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(resultEntities, opts => opts.IncludingProperties().IncludingNestedObjects());
        }

        [Fact]
        public async Task GetPaginatedAsync_WithPredicateIncludesWhenExistsLessThanOnePageSize_ReturnsCollection()
        {
            // Arrange
            var entities = GetDataForTests();
            int page = 1;
            int pageSize = 10;
            var resultEntities = entities.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var context = GetContext();
            foreach (var entity in entities)
                context.Entities.Add(entity);
            context.SaveChanges();

            var repository = GetRepository(context);

            // Act
            var result = await repository.GetPaginatedAsync(page, pageSize, e => e.Id < 10, e => e.RelatedEntity);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(entities, opts => opts.IncludingProperties().IncludingNestedObjects());
        }

        [Fact]
        public async Task GetPaginatedAsync_WithPredicateIncludesNotExists_ReturnsEmpty()
        {
            // Arrange
            var entities = new List<Entity>();

            var context = GetContext();
            var repository = GetRepository(context);

            // Act
            var result = await repository.GetPaginatedAsync(1, 10, e => e.Id == 1, e => e.RelatedEntity);

            // Assert
            result.Should().BeEquivalentTo(entities);
        }

        [Fact]
        public async Task GetPaginatedAsync_WithPredicateIncludesWhenExistsMoreThanOnePageSizeOrderAsc_ReturnsCollection()
        {
            // Arrange
            var entities = GetDataForTests();
            int page = 1;
            int pageSize = 10;
            var resultEntities = entities.OrderBy(e => e.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var context = GetContext();
            foreach (var entity in entities)
                context.Entities.Add(entity);
            context.SaveChanges();

            var repository = GetRepository(context);

            // Act
            var result = await repository.GetPaginatedAsync(page, pageSize, e => e.Id < 10, e => e.Id, false, e => e.RelatedEntity);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(resultEntities, opts => opts.IncludingProperties().IncludingNestedObjects());
        }

        [Fact]
        public async Task GetPaginatedAsync_WithPredicateIncludesWhenExistsMoreThanOnePageSizeOrderDesc_ReturnsCollection()
        {
            // Arrange
            var entities = GetDataForTests();
            int page = 1;
            int pageSize = 10;
            var resultEntities = entities.OrderByDescending(e => e.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var context = GetContext();
            foreach (var entity in entities)
                context.Entities.Add(entity);
            context.SaveChanges();

            var repository = GetRepository(context);

            // Act
            var result = await repository.GetPaginatedAsync(page, pageSize, e => e.Id < 10, e => e.Id, true, e => e.RelatedEntity);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(resultEntities, opts => opts.IncludingProperties().IncludingNestedObjects());
        }

        [Fact]
        public async Task GetPaginatedAsync_WithPredicateIncludesWhenExistsLessThanOnePageSizeOrderAsc_ReturnsCollection()
        {
            // Arrange
            var entities = GetDataForTests();
            int page = 1;
            int pageSize = 2;
            var resultEntities = entities.OrderBy(e => e.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var context = GetContext();
            foreach (var entity in entities)
                context.Entities.Add(entity);
            context.SaveChanges();

            var repository = GetRepository(context);

            // Act
            var result = await repository.GetPaginatedAsync(page, pageSize, e => e.Id < 10, e => e.Id, false, e => e.RelatedEntity);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(resultEntities, opts => opts.IncludingProperties().IncludingNestedObjects());
        }

        [Fact]
        public async Task GetPaginatedAsync_WithPredicateIncludesWhenExistsLessThanOnePageSizeOrderDesc_ReturnsCollection()
        {
            // Arrange
            var entities = GetDataForTests();
            int page = 1;
            int pageSize = 2;
            var resultEntities = entities.OrderByDescending(e => e.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var context = GetContext();
            foreach (var entity in entities)
                context.Entities.Add(entity);
            context.SaveChanges();

            var repository = GetRepository(context);

            // Act
            var result = await repository.GetPaginatedAsync(page, pageSize, e => e.Id < 10, e => e.Id, true, e => e.RelatedEntity);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(resultEntities, opts => opts.IncludingProperties().IncludingNestedObjects());
        }

        [Fact]
        public async Task GetPaginatedAsync_WithPredicateIncludesOrderNotExists_ReturnsEmpty()
        {
            // Arrange
            var entities = new List<Entity>();

            var context = GetContext();
            var repository = GetRepository(context);

            // Act
            var result = await repository.GetPaginatedAsync(1, 10, e => e.Id == 1, e => e.Id, false, e => e.RelatedEntity);

            // Assert
            result.Should().BeEquivalentTo(entities);
        }
        #endregion

        #region GetFilteredAsync
        [Fact]
        public async Task GetFilteredAsync_WhenExists_ReturnsCollection()
        {
            // Arrange
            var entities = GetDataForTests();

            var context = GetContext();
            foreach (var entity in entities)
                context.Entities.Add(entity);
            context.SaveChanges();

            var repository = GetRepository(context);

            // Act
            var result = await repository.GetFilteredAsync(e => e.Id < 10);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(entities, opts => opts.Excluding(e => e.RelatedEntity));
        }

        [Fact]
        public async Task GetFilteredAsync_WhenNotExists_ReturnsEmpty()
        {
            // Arrange
            var entities = new List<Entity>();

            var context = GetContext();
            var repository = GetRepository(context);

            // Act
            var result = await repository.GetFilteredAsync(e => e.Id < 10);

            // Assert
            result.Should().BeEquivalentTo(entities);
        }

        [Fact]
        public async Task GetFilteredAsync_WithIncludeWhenExists_ReturnsCollection()
        {
            // Arrange
            var entities = GetDataForTests();

            var context = GetContext();
            foreach (var entity in entities)
                context.Entities.Add(entity);
            context.SaveChanges();

            var repository = GetRepository(context);

            // Act
            var result = await repository.GetFilteredAsync(e => e.Id < 10, e => e.RelatedEntity);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(entities, opts => opts.IncludingProperties().IncludingNestedObjects());
        }

        [Fact]
        public async Task GetFilteredAsync_WithIncludeWhenNotExists_ReturnsEmpty()
        {
            // Arrange
            var entities = new List<Entity>();

            var context = GetContext();
            var repository = GetRepository(context);

            // Act
            var result = await repository.GetFilteredAsync(e => e.Id < 10, e => e.RelatedEntity);

            // Assert
            result.Should().BeEquivalentTo(entities);
        }
        #endregion

        #region CountAsync
        [Fact]
        public async Task CountAsync_WhenExists_ReturnsNumber()
        {
            // Arrange
            var entities = GetDataForTests();

            var context = GetContext();
            foreach (var entity in entities)
                context.Entities.Add(entity);
            context.SaveChanges();

            var repository = GetRepository(context);

            // Act
            var result = await repository.CountAsync();

            // Assert
            Assert.Equal(entities.Count(), result);
        }

        [Fact]
        public async Task CountAsync_WhenNotExists_ReturnsZero()
        {
            // Arrange
            var entities = new List<Entity>();

            var context = GetContext();
            var repository = GetRepository(context);

            // Act
            var result = await repository.CountAsync();

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task CountAsync_WithPredicateWhenExists_ReturnsNumber()
        {
            // Arrange
            var entities = GetDataForTests();
            var resultEntitiesCount = entities.Where(e => e.Id < 3).Count();

            var context = GetContext();
            foreach (var entity in entities)
                context.Entities.Add(entity);
            context.SaveChanges();

            var repository = GetRepository(context);

            // Act
            var result = await repository.CountAsync(e => e.Id < 3);

            // Assert
            Assert.Equal(resultEntitiesCount, result);
        }

        [Fact]
        public async Task CountAsync_WithPredicateWhenNotExists_ReturnsZero()
        {
            // Arrange
            var entities = new List<Entity>();

            var context = GetContext();
            var repository = GetRepository(context);

            // Act
            var result = await repository.CountAsync(e => e.Id < 3);

            // Assert
            Assert.Equal(0, result);
        }
        #endregion

        #region GetSingleAsync
        [Fact]
        public async Task GetSingleAsync_WhenExists_ReturnsEntity()
        {
            // Arrange
            var entities = GetDataForTests();
            var resultEntity = entities.Where(e => e.Id == 1).First();

            var context = GetContext();
            foreach (var entity in entities)
                context.Entities.Add(entity);
            context.SaveChanges();

            var repository = GetRepository(context);

            // Act
            var result = await repository.GetSingleAsync(1);

            // Assert
            result.Should().BeEquivalentTo(resultEntity, opts => opts.Excluding(e => e.RelatedEntity));
        }

        [Fact]
        public async Task GetSingleAsync_WhenNotExists_ReturnsNull()
        {
            // Arrange
            var entities = new List<Entity>();

            var context = GetContext();
            var repository = GetRepository(context);

            // Act
            var result = await repository.GetSingleAsync(10);

            // Assert
            Assert.Null(result);
        }
        #endregion

        #region AddOrUpdateAsync
        [Fact]
        public async Task AddOrUpdateAsync_WhenExisting_ReturnsEntity()
        {
            // Arrange
            var entities = GetDataForTests();
            var context = GetContext();
            foreach (var entity in entities)
                context.Entities.Add(entity);
            context.SaveChanges();

            var entityToUpdate = entities.First();

            var resultEntity = new BaseEntityImplementation()
            {
                Id = 1,
                RelatedEntityId = 1,
                Deleted = false
            };

            var repository = GetRepository(context);

            // Act
            var result = await repository.AddOrUpdateAsync(entityToUpdate);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(resultEntity, opts => opts.Excluding(e => e.RelatedEntity));
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

            var context = GetContext();
            var repository = GetRepository(context);
            
            // Act
            var result = await repository.AddOrUpdateAsync(entity);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(0, result.Id);
            Assert.Equal(entity.Deleted, result.Deleted);
        }
        #endregion

        #endregion

        #region Private Helpers
        private BlogDbContextWithEntityImplementation GetContext()
        {
            var options = new DbContextOptionsBuilder<BlogDbContextWithEntityImplementation>()
                                .UseInMemoryDatabase(databaseName: "BlogPOC")
                                .Options;
            var context = new BlogDbContextWithEntityImplementation(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }

        private BaseRepository<BaseEntityImplementation> GetRepository(BlogDbContextWithEntityImplementation context)
        {
            return new BaseRepositoryImplementation(context);
        }

        private ICollection<BaseEntityImplementation> GetDataForTests()
        {
            var entity1 = new BaseEntityImplementation()
            {
                Id = 1,
                RelatedEntityId = 1,
                RelatedEntity = new BaseRelatedEntityImplementation()
                {
                    Id = 1,
                    Deleted = false
                },
                Deleted = false
            };
            var entity2 = new BaseEntityImplementation()
            {
                Id = 2,
                RelatedEntityId = 2,
                RelatedEntity = new BaseRelatedEntityImplementation()
                {
                    Id = 2,
                    Deleted = false
                },
                Deleted = false
            };
            var entity3 = new BaseEntityImplementation()
            {
                Id = 3,
                RelatedEntityId = 3,
                RelatedEntity = new BaseRelatedEntityImplementation()
                {
                    Id = 3,
                    Deleted = false
                },
                Deleted = false
            };
            
            var entities = new List<BaseEntityImplementation>() {
                entity1,
                entity2,
                entity3
            };

            return entities;
        }

        #endregion

    }
}