namespace BlogExperimentalPlatform.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        #region Members
        private readonly IUserService userService;
        #endregion

        #region Constructor
        public BaseController(IUserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException("IUserService DI failed");
        }
        #endregion

        #region Properties
        protected IUserService UserService => userService;

        protected string LoggedInUserName
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                    return User.Identity.Name;
                else
                    return null;
            }
        }

        protected int LoggedInUserId
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    var claim = User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Sid);
                    return Convert.ToInt32(claim?.Value);
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion
    }
}
