namespace BlogExperimentalPlatform.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using AutoMapper;
    using BlogExperimentalPlatform.Business.Services;
    using BlogExperimentalPlatform.Web.Classes;
    using BlogExperimentalPlatform.Web.DTOs;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class BlogEntriesController : ControllerBase
    {
        #region Members
        private readonly IBlogEntryService blogEntryService;
        private readonly IMapper mapper;

        private int page = 1;
        private int pageSize = 10;
        #endregion

        #region Constructor
        public BlogEntriesController(IBlogEntryService blogEntryService, IMapper mapper)
        {
            this.blogEntryService = blogEntryService ?? throw new ArgumentNullException("IBlogEntryService DI failed");
            this.mapper = mapper ?? throw new ArgumentNullException("AutoMapper DI failed");
        }
        #endregion

        #region Methods

        // GET api/GetAllByBlogId/5
        [HttpGet("GetAllByBlogId/{blogId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ICollection<BlogEntryDTO>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllByBlogId(int blogId)
        {
            var pagination = Request.Headers["Pagination"];
            if (!string.IsNullOrEmpty(pagination))
            {
                string[] vals = pagination.ToString().Split(',');
                int.TryParse(vals[0], out page);
                int.TryParse(vals[1], out pageSize);
            }

            var entitiesPage = await blogEntryService.GetPaginatedAsync(page, pageSize, be => be.Blog.Id == blogId, be => be.LastUpdated, true, be => be.Blog.Owner);

            Response.AddPagination(page, pageSize, entitiesPage.TotalEntities, entitiesPage.TotalPages);

            return Ok(mapper.Map<ICollection<BlogEntryDTO>>(entitiesPage.Entities));
        }

        // GET api/blogEntries/5
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BlogEntryDTO))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var blogEntry = await blogEntryService.GetAsync(id, b => b.Blog, b => b.Blog);
            if (blogEntry == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<BlogEntryDTO>(blogEntry));
        }
        #endregion
    }
}