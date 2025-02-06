
namespace Application.Interfaces.IValidator
{
    public interface IValidatorHandler<in TRequest>
    {
        Task Validate(TRequest request);
    }
}
