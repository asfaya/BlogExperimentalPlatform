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
                RuleFor(b => b.Title).NotEmpty().WithMessage("Blog entry title is mandatory.");
                RuleFor(b => b.Content).NotEmpty().WithMessage("Blog entry content is mandatory");
                RuleFor(b => b.Status).NotEmpty().WithMessage("Blog entry status is mandatory");
                RuleFor(b => b.Blog).NotEmpty().WithMessage("Blog entry must be created for a particular blog");
            });
        }
    }
}
