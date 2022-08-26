using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using PrimeFuncPack;

namespace GGroupp.Infra.ArangoDb;

public static class ArangoDbGraphApiDependency
{
    public static Dependency<IDbGraphApi> UseArangoDbGraphApi(this Dependency<HttpMessageHandler, ArangoDbApiOption> dependency)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.Fold<IDbGraphApi>(ArangoDbGraphApi.Create);
    }

    public static Dependency<IDbGraphApi> UseArangoDbGraphApi(
        this Dependency<HttpMessageHandler> dependency, Func<IServiceProvider, ArangoDbApiOption> optionResolver)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _ = optionResolver ?? throw new ArgumentNullException(nameof(optionResolver));

        return dependency.With(optionResolver).Fold<IDbGraphApi>(ArangoDbGraphApi.Create);
    }

    public static Dependency<IDbGraphApi> UseArangoDbGraphApi(this Dependency<HttpMessageHandler> dependency, string sectioName = "ArangoDb")
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.With(ResolveOption).Fold<IDbGraphApi>(ArangoDbGraphApi.Create);

        ArangoDbApiOption ResolveOption(IServiceProvider serviceProvider)
            =>
            serviceProvider.GetServiceOrThrow<IConfiguration>().GetSection(sectioName).GetArangoDbApiOption();
    }

    private static ArangoDbApiOption GetArangoDbApiOption(this IConfigurationSection section)
        =>
        new(
            baseAddress: new(section["BaseAddressUrl"] ?? string.Empty),
            databaseId: section["DatabaseId"] ?? string.Empty,
            jwt: section["Jwt"] ?? string.Empty);
}