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
    using BlogExperimentalPlatform.Web.DTOs;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : BaseController
    {
        #region Members
        private readonly IBlogService blogService;
        private readonly IMapper mapper;
        #endregion

        #region Constructor
        public BlogsController(IBlogService blogService, IMapper mapper, IUserService userService)
            : base(userService)
        {
            this.blogService = blogService ?? throw new ArgumentNullException("IBlogService DI failed");
            this.mapper = mapper ?? throw new ArgumentNullException("AutoMapper DI failed");
        }
        #endregion

        #region Methods
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ICollection<BlogDTO>))]
        public async Task<IActionResult> Get()
        {
            var blogs = await blogService.GetAllAsync(b => b.Owner);
            var blogsDTO = mapper.Map<ICollection<BlogDTO>>(blogs);
            return Ok(blogsDTO);
        }

        // GET api/blogs/5
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BlogDTO))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var blog = await blogService.GetAsync(id, b => b.Owner);
            if (blog == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<BlogDTO>(blog));
        }

        // POST api/blogs/
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BlogDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> Post([CustomizeValidator(RuleSet = "BlogAddOrUpdate")]BlogDTO blogDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var blog = mapper.Map<Blog>(blogDTO);
                blog.OwnerId = LoggedInUserId;
                blog = await blogService.AddOrUpdateAsync(blog);
                blogDTO = mapper.Map<BlogDTO>(blog);
            }
            catch
            {
                throw new BlogSystemException("There's been an error while trying to add the blog entry");
            }

            return Ok(blogDTO);
        }

        // PUT api/blogs/5
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BlogDTO))]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> Put(int id, [CustomizeValidator(RuleSet = "BlogAddOrUpdate")]BlogDTO blogDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != blogDTO.Id)
                return BadRequest("Parameters mismatch");

            try
            {
                var blog = await blogService.GetAsync(id);

                if (blog.OwnerId != LoggedInUserId)
                    return StatusCode((int)HttpStatusCode.Forbidden, "The user is not the owner of the blog.");

                mapper.Map<BlogDTO, Blog>(blogDTO, blog);
                blog = await blogService.AddOrUpdateAsync(blog);
                blogDTO = mapper.Map<BlogDTO>(blog);
            }
            catch
            {
                throw new BlogSystemException("There's been an error while trying to add the blog entry");
            }

            return Ok(blogDTO);
        }

        // DELETE api/blogs/5
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var blog = await blogService.GetAsync(id);

                if (blog.OwnerId != LoggedInUserId)
                    return StatusCode((int)HttpStatusCode.Forbidden, "The user is not the owner of the blog.");

                await blogService.DeleteAsync(id);
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