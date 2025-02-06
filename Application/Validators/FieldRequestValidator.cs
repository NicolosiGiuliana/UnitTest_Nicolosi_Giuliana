using Application.DTOS.Request;
using Application.Interfaces.IQuery;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class FieldRequestValidator : AbstractValidator<FieldRequest>
    {
        private readonly IFieldQuery _fieldQuery;
        private readonly IAvailabilityQuery _availabilityQuery;

        public FieldRequestValidator(IFieldQuery fieldQuery, IAvailabilityQuery availabilityQuery)
        {
            _fieldQuery = fieldQuery;
            _availabilityQuery = availabilityQuery;

            RuleFor(field => field.Name)
                .NotEmpty().WithMessage("The name is required.");


            RuleFor(field => field.Size)
                .NotEmpty().WithMessage("The size is required.")
                .Must(size => size == "5" || size == "7" || size == "11")
                .WithMessage("The size must be either 5, 7, or 11.");


            RuleFor(field => field.FieldType)
                .NotEmpty()
                .WithMessage("FieldTypeNavigator is required.")
                .InclusiveBetween(1, 4);
        }


        // Check that Availability exists
        private async Task<bool> AvailabilityExists(int availabilityID, CancellationToken cancellationToken)
        {

            return await _availabilityQuery.AvailabilityExists(availabilityID);
        }
    }
}
