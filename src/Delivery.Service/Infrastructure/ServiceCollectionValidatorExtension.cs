using Delivery.UseCases.Utils.Result;
using Delivery.UseCases.Utils.Validation;
using MediatR;
using System.Reflection;
using FluentValidation;

namespace Delivery.Service.Infrastructure;

/// <summary>
/// Extensions for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionValidatorExtension
{
    /// <summary>
    /// Adds validation to the command processing pipelines.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="assemblies">Assemblies with commands or validators</param>
    /// <returns> Service collection </returns>
    public static IServiceCollection AddValidationPipelines(
        this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddValidatorsFromAssemblies(assemblies);
        var types = assemblies.SelectMany(assembly => assembly.GetTypes());
        AddValidationPipelines(services, types);
        return services;
    }

    /// <summary>
    /// Add validation pipelines.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="commandTypes">Command types</param>
    private static void AddValidationPipelines(IServiceCollection services, IEnumerable<Type> commandTypes)
    {
        foreach (var commandType in commandTypes)
        {
            if (commandType is not { IsClass: true, IsAbstract: false, ContainsGenericParameters: false }) continue;

            var targetInterface = commandType
                .GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && 
                                     i.GetGenericTypeDefinition() == typeof(IValidatableCommand<>));

            if (targetInterface == null) continue;

            var valueType = targetInterface.GenericTypeArguments[0];
            AddScoped(services, commandType, valueType);
        }
    }

    /// <summary>
    /// Adds validation as scoped service.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="commandType">Command type</param>
    /// <param name="valueType">Value type</param>
    private static void AddScoped(IServiceCollection services, Type commandType, Type valueType)
    {
        var pipe = typeof(IPipelineBehavior<,>).MakeGenericType(commandType, typeof(Result<>).MakeGenericType(valueType));
        services.AddScoped(pipe, typeof(ValidationBehavior<,>).MakeGenericType(commandType, valueType));
    }
}