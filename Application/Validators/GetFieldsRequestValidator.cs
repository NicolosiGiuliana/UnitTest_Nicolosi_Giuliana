using Application.DTOS.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class GetFieldsRequestValidator : AbstractValidator<GetFieldsRequest>
    {
        public GetFieldsRequestValidator()
        {
            RuleFor(x => x.Offset)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Offset.HasValue)
                .WithMessage("The 'offset' value must be a non-negative integer.");

            RuleFor(x => x.Size)
                 .GreaterThanOrEqualTo(0)
                 .When(x => x.Size.HasValue)
                 .WithMessage("The 'size' value must be a non-negative integer.");

        }
    }
}
