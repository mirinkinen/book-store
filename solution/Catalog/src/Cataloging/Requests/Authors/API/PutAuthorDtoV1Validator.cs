using Cataloging.Requests.Authors.API.Models;
using FluentValidation;

namespace Cataloging.Requests.Authors.API;

public class PutAuthorDtoV1Validator : AbstractValidator<PutAuthorDtoV1>
{
    public PutAuthorDtoV1Validator()
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