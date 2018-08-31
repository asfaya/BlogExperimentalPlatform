namespace BlogExperimentalPlatform.Web.DTOValidators
{
    using BlogExperimentalPlatform.Web.DTOs;
    using FluentValidation;

    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
            : base()
        {
            RuleSet("UserAddOrUpdate", () =>
            {
                RuleFor(u => u.FullName).NotEmpty().NotNull().WithMessage("User full name is mandatory.");
                RuleFor(u => u.UserName).NotEmpty().NotNull().WithMessage("Username is mandatory.");
            });
        }
    }
}
