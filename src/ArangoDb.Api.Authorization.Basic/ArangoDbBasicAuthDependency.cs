using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using PrimeFuncPack;

namespace GGroupp.Infra.ArangoDb;

public static class ArangoDbBasicAuthDependency
{
    public static Dependency<HttpMessageHandler> UseArangoDbBasicAuthorization(
        this Dependency<HttpMessageHandler, ArangoDbBasicAuthOption> dependency)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.Fold<HttpMessageHandler>(ArangoDbBasicAuthDelegatingHandler.Create);
    }

    public static Dependency<HttpMessageHandler> UseArangoDbBasicAuthorization(
        this Dependency<HttpMessageHandler> dependency, Func<IServiceProvider, ArangoDbBasicAuthOption> optionResolver)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _ = optionResolver ?? throw new ArgumentNullException(nameof(optionResolver));

        return dependency.With(optionResolver).Fold<HttpMessageHandler>(ArangoDbBasicAuthDelegatingHandler.Create);
    }

    public static Dependency<HttpMessageHandler> UseArangoDbBasicAuthorization(
        this Dependency<HttpMessageHandler> dependency, string optionSectionName = "ArangoDb")
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.With(ResolveOption).Fold<HttpMessageHandler>(ArangoDbBasicAuthDelegatingHandler.Create);

        ArangoDbBasicAuthOption ResolveOption(IServiceProvider serviceProvider)
            =>
            serviceProvider.GetServiceOrThrow<IConfiguration>().GetRequiredSection(optionSectionName).GetArangoDbBasicAuthOption();
    }

    private static ArangoDbBasicAuthOption GetArangoDbBasicAuthOption(this IConfigurationSection section)
        =>
        new(
            userName: section["userName"],
            password: section["password"]);
}