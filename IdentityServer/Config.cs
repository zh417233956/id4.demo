// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        //public static IEnumerable<IdentityResource> Ids =>
        //    new IdentityResource[]
        //    {
        //        new IdentityResources.OpenId(),
        //        //添加姓名等资源授权
        //        new IdentityResources.Profile()
        //    };

        public static List<IdentityResource> Ids =>
            new List<IdentityResource>
            {
                new IdentityResource{
                    Name="openid",
                    Enabled=true,
                    Emphasize=true,
                    Required=true,
                    DisplayName="用户授权认证信息",
                    Description="获取你的授权认证"
                },
                new IdentityResource{
                    Name="profile",
                    Enabled=true,
                    Emphasize=false,
                    Required=true,
                    DisplayName="用户个人信息",
                    Description="获取你的个人基本资料信息，如：姓名、性别、年龄等"
                },
                new IdentityResource(
                   name: "custom",
                   displayName: "custom profile",
                   claimTypes: new[] { IdentityModel.JwtClaimTypes.Name}),
                 new IdentityResource("info", "userinfo", new List<string>(){"role","name","userid","openid","unionid","nickname"}),
            };


        //public static IEnumerable<ApiResource> Apis =>
        //    new List<ApiResource>
        //{
        //    new ApiResource("api1", "My API")
        //};

        public static List<ApiResource> Apis =>
            new List<ApiResource>
            {
               //普通的通过构造函数限制 指定scope以及displayname 就行了
               //new ApiResource("api1","My API",new List<string>(){ "email"}),
               //new ApiResource("api1", "My API",new List<string>(){ IdentityModel.JwtClaimTypes.Email})

               //做一些更加严格的限制要求
               new  ApiResource(){
                    Enabled=true,
                    Name="api1",
                    DisplayName="My API",
                    Description="选择允许即同意获取My API权限",
                    Scopes={
                                new Scope()
                                {

                                    Emphasize=false,
                                    Required=false,
                                    Name="api1",
                                    DisplayName="My API",
                                    Description="选择允许即同意获取My API权限",
                                    UserClaims=new List<string>() { "email" }
                                }
                          }
               }
            };

        public static IEnumerable<Client> Clients =>
             new List<Client>
        {
            ClientCredentialsClient,
            ResourceOwnerPasswordClient,
            JsClient,
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
            },
            new Client
            {
                 ClientId = "mvc1",
                ClientName = "MVC Client",
                //AllowedGrantTypes = GrantTypes.Hybrid,
                AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret1".Sha256())
                },

                RedirectUris           = { "http://localhost:5002/signin-oidc" },
                PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                AllowedScopes =
                {
                    //IdentityServerConstants.StandardScopes.OpenId,
                    //IdentityServerConstants.StandardScopes.Profile,
                    "openid",
                    "profile",
                    "info"
                }
            }
        };

        private static Client ClientCredentialsClient => new Client
        {
            // 客户端模式
            /*
             * 客户端请求type:client_credentials
             * http://localhost:5000/connect/token 
             * post
             * data:
             * {
                    grant_type:client_credentials
                    client_id:client
                    client_secret:secret
                }
                response:
                {
                    "access_token": "access_token",
                    "expires_in": 3600,
                    "token_type": "Bearer",
                    "scope": "api1"
                }
             * 客户端鉴权
             * 第一次从http://localhost:5000获取加密秘钥，然后进行验签
            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "http://localhost:5000";
                options.RequireHttpsMetadata = false;
                options.Audience = "api1";
            }); 
             */
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
        };
        private static Client ResourceOwnerPasswordClient => new Client
        {
            //资源所有者密码授权模式
            /*
            * 客户端请求type:client_credentials
            * http://localhost:5000/connect/token 
            * post
            * data:
            * {
                   grant_type:password
                   client_id:client1
                   client_secret:secret
                   username:bob
                   password:bob
               }
               response:
               {
                   "access_token": "access_token",
                   "expires_in": 3600,
                   "token_type": "Bearer",
                   "scope": "api1"
               }
             */
            ClientId = "client1",
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

            ClientSecrets =
                           {
                               new Secret("secret".Sha256())
                           },
            AllowedScopes = { "api1" }
        };

        
        private static Client JsClient
        {
            //OpenID Connect 简化模式（implicit）

            get
            {
                const string host = "http://localhost:5002";
                return new Client
                {
                    
                    ClientId = "clientjs",
                    ClientName = "JS Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    RedirectUris =
                    {
                        $"{host}/client/oidc/login-callback.html",
                        $"{host}/client/oidc/refresh-token.html"
                    },
                    PostLogoutRedirectUris = { $"{host}/client/index.html" },

                    AllowedCorsOrigins = { host },
                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "info",
                        "api1"
                    },
                    AccessTokenLifetime = 3600,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                };
            }
        }
    }
}
