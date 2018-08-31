namespace BlogExperimentalPlatform.Web.DTOValidators
{
    using BlogExperimentalPlatform.Web.DTOs;
    using FluentValidation;

    public class BlogEntryDTOValidator : AbstractValidator<BlogEntryDTO>
    {
        public BlogEntryDTOValidator()
            : base()
        {
            RuleSet("BlogEntryAddOrUpdate", () =>
            {
                RuleFor(b => b.Title).NotEmpty().NotNull().WithMessage("Blog entry title is mandatory.");
                RuleFor(b => b.Content).NotEmpty().NotNull().WithMessage("Blog entry content is mandatory");
                RuleFor(b => b.Status).NotNull().WithMessage("Blog entry status is mandatory");
                RuleFor(b => b.Blog).NotNull().WithMessage("Blog entry must be created for a particular blog");
            });
        }
    }
}
