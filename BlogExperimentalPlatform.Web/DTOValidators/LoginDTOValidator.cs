﻿namespace BlogExperimentalPlatform.Web.DTOValidators
{
    using BlogExperimentalPlatform.Web.DTOs;
    using FluentValidation;

    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
             : base()
        {
            RuleSet("Login", () =>
            {
                RuleFor(l => l.UserName).NotEmpty().WithMessage("User name is mandatory.");
                RuleFor(l => l.Password).NotEmpty().WithMessage("Password is mandatory");
            });
        }
    }
}
