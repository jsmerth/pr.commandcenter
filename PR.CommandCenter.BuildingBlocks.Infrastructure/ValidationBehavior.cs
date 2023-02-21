using FluentValidation;
using MediatR;

namespace PR.CommandCenter.BuildingBlocks.Infrastructure;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
  private readonly IEnumerable<IValidator<TRequest>> _validators;

  public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
  {
    _validators = validators;
  }

  public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    if (!_validators.Any())
    {
      return next();
    }

    var failures = _validators
      .Select(v => v.Validate(request))
      .SelectMany(result => result.Errors)
      .Where(x => x != null)
      .ToList();

    return !failures.Any()
      ? next()
      : throw new ValidationException("Validation Error", failures);
  }
}