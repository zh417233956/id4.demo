// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
                                    UserClaims=new List<string>() { "role", "userid" }
                                }
                          }
               }
            };

        public static IEnumerable<Client> Clients =>
             new List<Client>
        {
            ClientCredentialsClient,
            ResourceOwnerPasswordClient,
            AuthorizationCodeClient,
            ImplicitClient,
            HybridClient,
            JsClient
        };

        /*
         * 
         * （1）客户端模式（AllowedGrantTypes = GrantTypes.ClientCredentials）
         * 这是一种最简单的授权方式，应用于服务于服务之间的通信，token通常代表的是客户端的请求，而不是用户。
         * 用这种授权类型，会向token endpoint发送token请求，并获得代表客户机的access token。客户端通常必须使用token endpoint的Client ID和secret进行身份验证。
         * 适用场景：用于和用户无关，服务与服务之间直接交互访问资源
         * 
         * （2）密码模式（ClientAllowedGrantTypes = GrantTypes.ResourceOwnerPassword）
         * 该方式发送用户名和密码到token endpoint，向资源服务器请求令牌。这是一种“非交互式”授权方法。
         * 官网上称，为了解决一些历史遗留的应用场景，所以保留了这种授权方式，但不建议使用。
         * 适用场景：用于当前的APP是专门为服务端设计的情况。
         * 
         * （3）混合模式和客户端模式（ClientAllowedGrantTypes =GrantTypes.HybridAndClientCredentials）
            ClientCredentials授权方式在第一种应用场景已经介绍了，这里主要介绍Hybrid授权方式。Hybrid是由Implicit和Authorization code结合起来的一种授权方式。其中Implicit用于身份认证，ID token被传输到浏览器并在浏览器进行验证；而Authorization code使用反向通道检索token和刷新token。
            推荐使用Hybrid模式。
            适用场景：用于MVC框架，服务器端 Web 应用程序和原生桌面/移动应用程序。
         * 
         * （4）简化模式（ClientAllowedGrantTypes =GrantTypes.Implicit）
            Implicit要么仅用于服务端和JavaScript应用程序端进行身份认证，要么用于身份身份验证和access token的传输。
            在Implicit中，所有token都通过浏览器传输的。
            适用场景：JavaScript应用程序。
         * 
         */

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

        private static Client ImplicitClient
        {
            get
            {
                const string home = "http://localhost:5003";
                const string OidcLoginCallback = "/signin-oidc";
                const string OidcFrontChannelLogoutCallback = "/oidc/front-channel-logout-callback";

                return new Client
                {
                    ClientId = "implicit",
                    ClientName = "Implicit Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    RequireConsent = false,

                    RedirectUris = { home + OidcLoginCallback },
                    PostLogoutRedirectUris = { home },

                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "info"
                    },

                    FrontChannelLogoutUri = home + OidcFrontChannelLogoutCallback,
                    FrontChannelLogoutSessionRequired = true
                };
            }
        }

        private static Client AuthorizationCodeClient
        {
            get
            {
                return new Client
                {
                    ClientId = "mvc",
                    ClientName = "Oidc Authorization Code Client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = true,//是否弹出授权
                    RequirePkce = true,

                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5001/signin-oidc", "http://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5001/signout-callback-oidc", "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },
                    FrontChannelLogoutUri = "http://localhost:5002/signout-oidc",
                    FrontChannelLogoutSessionRequired = true
                };
            }
        }

        private static Client HybridClient
        {
            get
            {
                const string home = "http://localhost:5002";
                const string OidcLoginCallback = "/signin-oidc";
                const string OidcSignoutCallback = "/signout-callback-oidc";
                const string OidcFrontChannelLogoutCallback = "/oidc/front-channel-logout-callback";
                return new Client
                {
                    ClientId = "mvc1",
                    ClientName = "Hybrid Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },

                    RequireConsent = false,

                    RedirectUris = { home + OidcLoginCallback },
                    PostLogoutRedirectUris = { home + OidcSignoutCallback },

                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "info"
                    },

                    FrontChannelLogoutUri = home + OidcFrontChannelLogoutCallback,
                    FrontChannelLogoutSessionRequired = true
                };
            }
        }

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


        public static IEnumerable<Client> CusClients
        {
            get
            {
                var result = new List<Client>{
                    ClientCredentialsClient,
                    ResourceOwnerPasswordClient,
                    AuthorizationCodeClient,
                    ImplicitClient,
                    //HybridClient,
                    //JsClient
                };
                var config = Startup.Configuration;
                var Total = int.Parse(config["Id4:Total"]);

                for (int i = 0; i < Total; i++)
                {
                    string configKeyPre = $"Id4:Clients:{i}:";
                    string home = config[configKeyPre + "Host"];
                    var OidcLoginCallback = config[configKeyPre + "OidcLoginCallback"].Split(',').ToList();                   
                    var OidcSignoutCallback = config[configKeyPre + "OidcSignoutCallback"].Split(',').ToList();

                    for (int j = 0; j < OidcLoginCallback.Count; j++)
                    {
                        OidcLoginCallback[i] = home + OidcLoginCallback[i];
                    }
                    for (int j = 0; j < OidcSignoutCallback.Count; j++)
                    {
                        OidcSignoutCallback[i] = home + OidcSignoutCallback[i];
                    }

                    var OidcFrontChannelLogoutCallback = config[configKeyPre + "OidcFrontChannelLogoutCallback"];

                    var client = new Client
                    {
                        ClientId = config[configKeyPre + "ClientId"],
                        ClientName = config[configKeyPre + "ClientName"],
                        AllowedGrantTypes = config[configKeyPre + "GrantTypes"].Split(',').ToList(),
                        ClientSecrets = new List<Secret>
                        {
                            new Secret(config[configKeyPre + "Secret"].Sha256())
                        },

                        RequireConsent = Convert.ToBoolean(config[configKeyPre + "RequireConsent"]),

                        RedirectUris = OidcLoginCallback,
                        PostLogoutRedirectUris = OidcSignoutCallback,

                        AllowedScopes = config[configKeyPre + "Scopes"].Split(',').ToList()

                    };
                    if (config[configKeyPre + "FrontChannelLogoutSessionRequired"] == "True")
                    {
                        client.FrontChannelLogoutUri = home + OidcFrontChannelLogoutCallback;
                        client.FrontChannelLogoutSessionRequired = true;
                    }

                    result.Add(client);
                }

                return result;
            }
        }
    }
}
