namespace BlogExperimentalPlatform.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AutoMapper;
    using BlogExperimentalPlatform.Business.Services;
    using BlogExperimentalPlatform.Web.DTOs;
    using BlogExperimentalPlatform.Web.Security;
    using BlogExperimentalPlatform.Web.Settings;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : BaseController
    {
        #region Members
        private readonly IOptions<SecuritySettings> securitySettings;
        private readonly IMapper mapper;
        #endregion

        #region Constructor
        public LoginController(
            IOptions<SecuritySettings> securitySettings,
            IMapper mapper,
            IUserService userService)
            : base(userService)
        {
            this.securitySettings = securitySettings ?? throw new ArgumentNullException("Security Settings DI failed");
            this.mapper = mapper ?? throw new ArgumentNullException("AutoMapper DI failed");
        }
        #endregion

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ICollection<UserDTO>))]
        public async Task<IActionResult> Get()
        {
            var users = await UserService.GetAllAsync();
            var usersDTO = mapper.Map<ICollection<UserDTO>>(users);
            return Ok(usersDTO);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Authenticate([CustomizeValidator(RuleSet = "Login")]LoginDTO login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await UserService.AuthenticateAsync(login.UserName, login.Password);

            if (user == null)
                return Unauthorized();

            var userDTO = mapper.Map<UserDTO>(user);
            userDTO.Token = BuildAuthToken(user.Id, user.UserName);

            return Ok(userDTO);
        }

        private string BuildAuthToken(int id, string subject)
        {
            var token = new JwtTokenBuilder()
                                            .AddSecurityKey(JwtSecurityKey.Create(securitySettings.Value.Secret))
                                            .AddSubject(subject)
                                            .AddClaim(ClaimTypes.Sid, id.ToString())
                                            .AddIssuer(securitySettings.Value.Issuer)
                                            .AddAudience(securitySettings.Value.Audience)
                                            .AddExpiry(securitySettings.Value.TokenTimeOut)
                                            .Build();

            return token.Value;
        }
    }
}