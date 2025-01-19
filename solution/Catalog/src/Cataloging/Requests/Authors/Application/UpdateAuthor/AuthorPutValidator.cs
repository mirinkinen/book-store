using FluentValidation;

namespace Cataloging.Requests.Authors.Application.UpdateAuthor;

public class AuthorPutValidator : AbstractValidator<AuthorPutDtoV1>
{
    public AuthorPutValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.Birthday)
            .NotEmpty();
    }
}