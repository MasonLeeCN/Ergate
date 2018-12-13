using AutoMapper;
using Ergate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static void AddService(this IServiceCollection services, ServiceInfo info)
        {
            //读取配置文件
            var configBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var config = configBuilder.Build();

            //跨域配置
            services.AddCors(opt =>
            {
                opt.AddPolicy("AllowDomain", builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials();
                });
            });

            //jwt授权
            if (config.GetSection("JWt:Key").Value != null)
            {
                services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = "Jwt";
                    opt.DefaultChallengeScheme = "Jwt";
                }).AddJwtBearer("Jwt", opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters();
                    opt.TokenValidationParameters.ValidateIssuer = false;
                    opt.TokenValidationParameters.ValidateAudience = false;
                    opt.TokenValidationParameters.ValidateIssuerSigningKey = true;
                    opt.TokenValidationParameters.ValidateLifetime = true;
                    opt.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(5);
                    var key = config.GetSection("JWt:Key").Value;
                    opt.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                });
            }

            //Cap配置

            if (config.GetSection("RabbitMQ:HostName").Value != null)
            {
                services.AddCap(x =>
                {
                    x.UseRabbitMQ(opt =>
                    {
                        var hostName = config.GetSection("RabbitMQ:HostName").Value;
                        var exchangeName = config.GetSection("RabbitMQ:ExchangeName").Value;
                        var port = config.GetSection("RabbitMQ:Port").Value;
                        var userName = config.GetSection("RabbitMQ:UserName").Value;
                        var password = config.GetSection("RabbitMQ:Password").Value;
                        var virtualHost = config.GetSection("RabbitMQ:VirtualHost").Value;

                        if (!string.IsNullOrEmpty(hostName) && hostName != "")
                        {
                            opt.HostName = hostName;
                        }

                        if (!string.IsNullOrEmpty(exchangeName) && exchangeName != "")
                        {
                            opt.ExchangeName = exchangeName;
                        }

                        if (!string.IsNullOrEmpty(port) && port != "")
                        {
                            opt.Port = int.Parse(port);
                        }

                        if (!string.IsNullOrEmpty(userName) && userName != "")
                        {
                            opt.UserName = userName;
                        }

                        if (!string.IsNullOrEmpty(password) && password != "")
                        {
                            opt.Password = password;
                        }

                        if (!string.IsNullOrEmpty(virtualHost) && virtualHost != "")
                        {
                            opt.VirtualHost = virtualHost;
                        }
                    });
                    x.UseMySql(opt =>
                    {
                        opt.ConnectionString = config.GetSection("ConnectionString:Database").Value;
                    });
                });
            }
            
            //Swagger配置
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc(info.ServiceName, new Info
                {
                    Title = info.Title,
                    Version = $"v{info.Version.MajorVersion}.{info.Version.MinorVersion}"
                });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, info.XmlName);
                option.IncludeXmlComments(xmlPath);
            });

            //版本控制   (貌似不支持Swagger)
            services.AddApiVersioning(opts =>
            {
                opts.ReportApiVersions = true;
                opts.AssumeDefaultVersionWhenUnspecified = true;
                opts.DefaultApiVersion = new ApiVersion(1, 0);
                opts.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
            services.AddAutoMapper();
            services.AddScoped<IEventPublisher, EventPublisher>();
        }

        public static void UseService(this IApplicationBuilder app, IHostingEnvironment env, ServiceInfo info)
        {
            //读取配置文件
            var configBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var config = configBuilder.Build();

            if (!env.IsProduction())
            {
                app.UseSwagger(opt => { opt.RouteTemplate = "{documentName}/swagger.json"; });
                app.UseSwaggerUI(opt => { opt.SwaggerEndpoint($"/{info.ServiceName}/swagger.json", info.Title); });
            }

            app.UseAuthentication();
            app.UseCors("AllowDomain");
            //Cap配置

            if (config.GetSection("RabbitMQ").Value != null)
            {
                app.UseCap();
            }
        }
    }
}
