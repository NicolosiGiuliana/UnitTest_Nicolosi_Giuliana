using Application.Interfaces.IValidator;
using FluentValidation;

namespace Application.Validators
{
    public class ValidatorHandler<TRequest> : IValidatorHandler<TRequest>
    {
        private readonly IValidator<TRequest> _validator;

        public ValidatorHandler(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public async Task Validate(TRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
