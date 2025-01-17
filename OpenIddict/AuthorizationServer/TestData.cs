﻿using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

public class TestData : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public TestData(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<DbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        if (await manager.FindByClientIdAsync("postman", cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "postman",
                ClientSecret = "postman-secret",
                RedirectUris = { new Uri("https://oauth.pstmn.io/v1/callback") }, // for AuthorizationCodeFlow
                DisplayName = "Postman",
                Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization, // for AuthorizationCodeFlow
                        OpenIddictConstants.Permissions.Endpoints.Token,

                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode, // for AuthorizationCodeFlow
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,

                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken, // for Refresh Token

                        OpenIddictConstants.Permissions.Prefixes.Scope + "api",

                        OpenIddictConstants.Permissions.ResponseTypes.Code // for AuthorizationCodeFlow
                    }
            }, cancellationToken);
        } 
        if (await manager.FindByClientIdAsync("oidcdebugger", cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "oidcdebugger",
                ClientSecret = "oidcdebugger-secret",
                RedirectUris = { new Uri("https://oidcdebugger.com/debug") }, // for AuthorizationCodeFlow
                DisplayName = "OidcDebugger",
                Permissions =
            {
                OpenIddictConstants.Permissions.Endpoints.Authorization, // for AuthorizationCodeFlow
                OpenIddictConstants.Permissions.Endpoints.Token,

                OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode, // for AuthorizationCodeFlow
                OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,

                OpenIddictConstants.Permissions.GrantTypes.RefreshToken, // for Refresh Token

                OpenIddictConstants.Permissions.Prefixes.Scope + "api",

                OpenIddictConstants.Permissions.ResponseTypes.Code // for AuthorizationCodeFlow
            }
            }, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
