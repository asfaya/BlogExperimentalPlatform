namespace BlogExperimentalPlatform.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using AutoMapper;
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Services;
    using BlogExperimentalPlatform.Utils;
    using BlogExperimentalPlatform.Web.Classes;
    using BlogExperimentalPlatform.Web.DTOs;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class BlogEntriesController : BaseController
    {
        #region Members
        private readonly IBlogEntryService blogEntryService;
        private readonly IBlogService blogService;
        private readonly IMapper mapper;

        private int page = 1;
        private int pageSize = 10;
        #endregion

        #region Constructor
        public BlogEntriesController(IBlogEntryService blogEntryService, IBlogService blogService, IUserService userService, IMapper mapper)
            : base(userService)
        {
            this.blogEntryService = blogEntryService ?? throw new ArgumentNullException("IBlogEntryService DI failed");
            this.blogService = blogService ?? throw new ArgumentNullException("IBlogService DI failed");
            this.mapper = mapper ?? throw new ArgumentNullException("AutoMapper DI failed");
        }
        #endregion

        #region Methods

        // GET api/blogEntries/GetAllByBlogId/5
        [HttpGet("GetAllByBlogId/{blogId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ICollection<BlogEntryDTO>))]
        public async Task<IActionResult> GetAllByBlogId(int blogId)
        {
            var pagination = Request.Headers["Pagination"];
            if (!string.IsNullOrEmpty(pagination))
            {
                string[] vals = pagination.ToString().Split(',');
                int.TryParse(vals[0], out page);
                int.TryParse(vals[1], out pageSize);
            }

            var entitiesPage = await blogEntryService.GetPaginatedAsync(page, pageSize, be => be.Blog.Id == blogId, be => be.LastUpdated, true, be => be.Blog, be => be.Blog.Owner);

            Response.AddPagination(page, pageSize, entitiesPage.TotalEntities, entitiesPage.TotalPages);

            return Ok(mapper.Map<ICollection<BlogEntryDTO>>(entitiesPage.Entities));
        }

        // GET api/blogEntries/5
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BlogEntryDTO))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var blogEntry = await blogEntryService.GetAsync(id, b => b.Blog, b => b.Blog.Owner);
            if (blogEntry == null)
                return NotFound();

            return Ok(mapper.Map<BlogEntryDTO>(blogEntry));
        }

        // POST api/blogEntries/
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BlogEntryDTO))]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> Post([CustomizeValidator(RuleSet = "BlogEntryAddOrUpdate")]BlogEntryDTO blogEntryDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var blog = await blogService.GetAsync(blogEntryDTO.Blog.Id);
                if (blog.OwnerId != LoggedInUserId)
                    return StatusCode((int)HttpStatusCode.Forbidden, "The user is not the owner of the blog.");

                var blogEntry = mapper.Map<BlogEntry>(blogEntryDTO);
                blogEntry = await blogEntryService.AddOrUpdateAsync(blogEntry);
                blogEntryDTO = mapper.Map<BlogEntryDTO>(blogEntry);
            }
            catch
            {
                throw new BlogSystemException("There's been an error while trying to add the blog entry");
            }

            return Ok(blogEntryDTO);
        }

        // PUT api/blogEntries/5
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BlogEntryDTO))]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> Put(int id, [CustomizeValidator(RuleSet = "BlogEntryAddOrUpdate")]BlogEntryDTO blogEntryDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != blogEntryDTO.Id)
                return BadRequest("Parameters mismatch");

            try
            {
                var blogEntry = await blogEntryService.GetAsync(id, be => be.Blog);
                if (blogEntry.Blog.OwnerId != LoggedInUserId)
                    return StatusCode((int)HttpStatusCode.Forbidden, "The user is not the owner of the blog.");

                mapper.Map<BlogEntryDTO, BlogEntry>(blogEntryDTO, blogEntry);
                blogEntry = await blogEntryService.AddOrUpdateAsync(blogEntry);
                blogEntryDTO = mapper.Map<BlogEntryDTO>(blogEntry);
            }
            catch
            {
                throw new BlogSystemException("There's been an error while trying to add the blog entry");
            }

            return Ok(blogEntryDTO);
        }

        // DELETE api/blogEntries/5
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var blogEntry = await blogEntryService.GetAsync(id, be => be.Blog);
                if (blogEntry.Blog.OwnerId != LoggedInUserId)
                    return StatusCode((int)HttpStatusCode.Forbidden, "The user is not the owner of the blog.");

                await blogEntryService.DeleteAsync(id);
            }
            catch
            {
                throw new BlogSystemException("There's been an error while trying to delete the blog entry");
            }

            return NoContent();
        }
        #endregion
    }
}