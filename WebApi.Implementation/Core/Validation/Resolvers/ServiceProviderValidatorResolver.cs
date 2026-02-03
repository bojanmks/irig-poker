using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.Core.Validation;

namespace WebApi.Implementation.Core.Validation.Resolvers;

public class ServiceProviderValidatorResolver : IValidatorResolver
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceProviderValidatorResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public AbstractValidator<T> Resolve<T>()
    {
        return _serviceProvider.GetService<AbstractValidator<T>>();
    }
}
