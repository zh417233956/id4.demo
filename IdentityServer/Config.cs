﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                //添加姓名等资源授权
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
        {
            new ApiResource("api1", "My API")
        };

        public static IEnumerable<Client> Clients =>
             new List<Client>
        {
            new Client
            {
                ClientId = "client",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                // scopes that client has access to
                AllowedScopes = { "api1" }
            },
             // interactive ASP.NET Core MVC client
            new Client
            {
                ClientId = "mvc",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,
                RequireConsent = true,//是否弹出授权
                RequirePkce = true,

                // where to redirect to after login
                RedirectUris = { "http://localhost:5001/signin-oidc","http://localhost:5002/signin-oidc" },

                // where to redirect to after logout
                PostLogoutRedirectUris = { "http://localhost:5001/signout-callback-oidc","http://localhost:5002/signout-callback-oidc" },

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },
                FrontChannelLogoutUri="http://localhost:5002/signout-oidc",
                FrontChannelLogoutSessionRequired=true
            }
        };

    }
}