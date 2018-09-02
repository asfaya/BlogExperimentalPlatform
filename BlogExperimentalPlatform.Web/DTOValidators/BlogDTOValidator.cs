namespace BlogExperimentalPlatform.Web.DTOValidators
{
    using BlogExperimentalPlatform.Web.DTOs;
    using FluentValidation;

    public class BlogDTOValidator : AbstractValidator<BlogDTO>
    {
        public BlogDTOValidator()
            : base()
        {
            RuleSet("BlogAddOrUpdate", () =>
            {
                RuleFor(b => b.Name).NotEmpty().WithMessage("Blog name is mandatory.");
                RuleFor(b => b.Owner).NotEmpty().WithMessage("Blog needs to have an owner");
            });
        }
    }
}
