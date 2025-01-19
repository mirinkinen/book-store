using FluentValidation;

namespace Cataloging.Requests.Authors.Application.UpdateAuthor;

public class AuthorPutValidator : AbstractValidator<AuthorPutDtoV1>
{
    public AuthorPutValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithErrorCode("firstname-is-empty")
            .MaximumLength(32).WithErrorCode("firstname-is-too-long");

        RuleFor(x => x.LastName)
            .NotEmpty().WithErrorCode("lastname-is-empty")
            .MaximumLength(32).WithErrorCode("lastname-is-too-long");

        RuleFor(x => x.Birthday)
            .NotEmpty().WithErrorCode("birthday-is-empty");
    }
}