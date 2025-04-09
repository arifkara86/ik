// HrApp.EmployeeManagement.Application/Behaviours/ValidationBehaviour.cs

using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation.Results;
// using HrApp.EmployeeManagement.Application.Exceptions; // Bu satırı kaldırıp aşağıdaki alias'ı kullanacağız
using ValidationException = HrApp.EmployeeManagement.Application.Exceptions.ValidationException; // TAKMA AD (ALIAS) KULLANIYORUZ

namespace HrApp.EmployeeManagement.Application.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    // ... (Constructor aynı) ...
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehaviour<TRequest, TResponse>> _logger;

     public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehaviour<TRequest, TResponse>> logger)
    {
        _validators = validators;
         _logger = logger;
    }


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
             var context = new ValidationContext<TRequest>(request);
            _logger.LogInformation("----- Validating command {CommandType}", typeof(TRequest).Name);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                _logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeof(TRequest).Name, request, failures);

                // Burada artık 'ValidationException' yazdığımızda, alias sayesinde
                // bizim HrApp.EmployeeManagement.Application.Exceptions.ValidationException'ı
                // kastettiğimiz anlaşılacak.
                throw new ValidationException(failures);
            }
             _logger.LogInformation("Validation successful for {CommandType}", typeof(TRequest).Name);
        }
        return await next();
    }
}