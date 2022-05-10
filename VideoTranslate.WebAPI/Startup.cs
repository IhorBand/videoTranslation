using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using VideoTranslate.DataAccess.Repositories;
using VideoTranslate.Service.MQServices;
using VideoTranslate.Service.Providers;
using VideoTranslate.Service.Services;
using VideoTranslate.Service.Validators;
using VideoTranslate.Shared.Abstractions.Providers;
using VideoTranslate.Shared.Abstractions.Repositories;
using VideoTranslate.Shared.Abstractions.Services;
using VideoTranslate.Shared.Abstractions.Services.MQServices;
using VideoTranslate.Shared.Abstractions.Validators;
using VideoTranslate.Shared.DTO.Configuration;
using VideoTranslate.WebAPI.Infrastructure.MappingProfiles;
using VideoTranslate.WebAPI.Middleware;
using VideoTranslate.WebApiClient.DTO;

namespace VideoTranslate.WebAPI
{
    public class Startup
    {
        private readonly IResourceProvider resourceProvider;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.Configuration = configuration;
            this.Env = env;
            this.resourceProvider = new ResourceProvider();
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            this.SetupDependencyInjection(services);

            services.AddCors(options =>
            {
                options.AddPolicy("OpenPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            /*
            this.SetupTracing(services);
            */

            var connectionStrings =
                this.Configuration.GetSection("ConnectionStrings").Get<ConnectionStringConfiguration>();

            services.AddHealthChecks()
                .AddSqlServer(connectionStrings.Main);
            /*
            this.SetupJWTServices(services);
            */

            services
                .AddControllers((op) =>
                {
                    op.Filters.Add(new ProducesResponseTypeAttribute(typeof(List<ErrorDetail>), (int)HttpStatusCode.BadRequest));
                    op.Filters.Add(new ProducesResponseTypeAttribute(typeof(ExceptionDetail), (int)HttpStatusCode.InternalServerError));
                })
                .AddNewtonsoftJson((op) =>
                {
                    op.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VideoTranslate.WebApi", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorizationheader using the Bearer scheme. \n\n
                        Enter 'Bearer[space]'and then your token in the text  input below.\n\n
                        Example:  'Bearer tes2543t432to243ke324n'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            services.AddSwaggerGenNewtonsoftSupport();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<ExceptionMiddleware> logger)
        {
            app.UseRequestResponseLogging();

            /*
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VideoTranslate v1"));
            }
            */

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VideoTranslate v1"));

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = this.WriteResponseAsync
            });

            app.UseMiddleware<ExceptionMiddleware>(this.resourceProvider, logger);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("OpenPolicy");

            // app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void SetupDependencyInjection(IServiceCollection services)
        {
            var connectionStrings = this.Configuration.GetSection("ConnectionStrings").Get<ConnectionStringConfiguration>();
            services.AddSingleton(connectionStrings);
            var rabbitMQConfiguration = this.Configuration.GetSection("RabbitMQ").Get<RabbitMQConfiguration>();
            services.AddSingleton(rabbitMQConfiguration);
            var folderPathConfiguration = this.Configuration.GetSection("FolderPaths").Get<FolderPathConfiguration>();
            services.AddSingleton(folderPathConfiguration);

            // AutoMapper Configuration
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSingleton<IPasswordValidator, PasswordValidator>();
            services.AddSingleton<IFFmpegQueueService, FFmpegQueueService>();

            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IFileServerRepository, FileServerRepository>();
            services.AddScoped<IVideoFileRepository, VideoFileRepository>();
            services.AddScoped<IVideoInfoRepository, VideoInfoRepository>();

            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IVideoFileService, VideoFileService>();
            services.AddScoped<IVideoInfoService, VideoInfoService>();
        }

        private Task WriteResponseAsync(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            var defaultLogLevel = this.Configuration["Logging:LogLevel:Default"];
            var connectionString = this.Configuration["ConnectionStrings:Main"];
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            if (!string.IsNullOrEmpty(connectionStringBuilder.Password))
            {
                connectionStringBuilder.Password = $"{connectionStringBuilder.Password[0]}***{connectionStringBuilder.Password[connectionStringBuilder.Password.Length - 1]}";
            }

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("configuration", new JObject(
                    new JProperty("defaultLogLevel", defaultLogLevel),
                    new JProperty("hostEnvironment", this.Env.EnvironmentName))),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                   new JProperty(pair.Key, new JObject(
                       new JProperty("status", pair.Value.Status.ToString()),
                       new JProperty("description", pair.Value.Description),
                       new JProperty("data", new JObject(pair.Value.Data.Select(p =>
                           new JProperty(p.Key, p.Value))))))))));

            return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
        }

        /*
        private void SetupTracing(IServiceCollection services)
        {
            var exporter = this.Configuration.GetValue<string>("UseExporter").ToLowerInvariant();
            switch (exporter.ToLower())
            {
                case "jaeger":
                    services.AddOpenTelemetryTracing((builder) => builder
                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(this.Configuration.GetValue<string>("Jaeger:ServiceName")))
                        .AddSource(UserService.ActivitySourceName)
                        .AddSource(UserRepository.ActivitySourceName)
                        .AddAspNetCoreInstrumentation((options) => options.Enrich = (activity, eventName, rawObject) =>
                        {
                            if (eventName.Equals("OnStartActivity"))
                            {
                                if (rawObject is HttpRequest httpRequest)
                                {
                                    activity.SetTag("requestProtocol", httpRequest.Protocol);
                                }
                            }
                            else if (eventName.Equals("OnStopActivity"))
                            {
                                if (rawObject is HttpResponse httpResponse)
                                {
                                    activity.SetTag("responseLength", httpResponse.ContentLength);
                                }
                            }
                        })
                        .AddHttpClientInstrumentation()
                        .AddGrpcClientInstrumentation()
                        .AddSqlClientInstrumentation(options =>
                        {
                            options.EnableConnectionLevelAttributes = true;
                            options.SetTextCommandContent = true;
                        })
                        .AddJaegerExporter(jaegerOptions =>
                        {
                            jaegerOptions.AgentHost = this.Configuration.GetValue<string>("Jaeger:Host");
                            jaegerOptions.AgentPort = this.Configuration.GetValue<int>("Jaeger:Port");
                        }));
                    break;
                case "azure":
                    services.AddOpenTelemetryTracing((builder) => builder
                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(this.Configuration.GetValue<string>("Azure:ServiceName")))
                        .AddSource(UserService.ActivitySourceName)
                        .AddSource(UserRepository.ActivitySourceName)
                        .AddAspNetCoreInstrumentation((options) => options.Enrich = (activity, eventName, rawObject) =>
                        {
                            if (eventName.Equals("OnStartActivity"))
                            {
                                if (rawObject is HttpRequest httpRequest)
                                {
                                    activity.SetTag("requestProtocol", httpRequest.Protocol);
                                }
                            }
                            else if (eventName.Equals("OnStopActivity"))
                            {
                                if (rawObject is HttpResponse httpResponse)
                                {
                                    activity.SetTag("responseLength", httpResponse.ContentLength);
                                }
                            }
                        })
                        .AddHttpClientInstrumentation()
                        .AddGrpcClientInstrumentation()
                        .AddSqlClientInstrumentation(options =>
                        {
                            options.EnableConnectionLevelAttributes = true;
                            options.SetTextCommandContent = true;
                        })
                        .AddProcessor(new BatchExportProcessor<Activity>(new AzureMonitorTraceExporter(new AzureMonitorExporterOptions
                        {
                            ConnectionString = this.Configuration.GetValue<string>("Azure:ConnectionString")
                        }))));
                    break;
                case "otlp":
                    services.AddOpenTelemetryTracing((builder) => builder
                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(this.Configuration.GetValue<string>("Otlp:ServiceName")))
                        .AddSource(UserService.ActivitySourceName)
                        .AddSource(UserRepository.ActivitySourceName)
                        .AddAspNetCoreInstrumentation((options) => options.Enrich
                        = (activity, eventName, rawObject) =>
                        {
                            if (eventName.Equals("OnStartActivity"))
                            {
                                if (rawObject is HttpRequest httpRequest)
                                {
                                    activity.SetTag("requestProtocol", httpRequest.Protocol);
                                }
                            }
                            else if (eventName.Equals("OnStopActivity"))
                            {
                                if (rawObject is HttpResponse httpResponse)
                                {
                                    activity.SetTag("responseLength", httpResponse.ContentLength);
                                }
                            }
                        })
                        .AddHttpClientInstrumentation()
                        .AddGrpcClientInstrumentation()
                        .AddSqlClientInstrumentation(options =>
                        {
                            options.EnableConnectionLevelAttributes = true;
                            options.SetTextCommandContent = true;
                        })
                        .AddOtlpExporter(otlpOptions =>
                        {
                            otlpOptions.Endpoint = this.Configuration.GetValue<string>("Otlp:Endpoint");
                        }));
                    break;
                default:
                    services.AddOpenTelemetryTracing((builder) => builder
                        .AddSource(UserService.ActivitySourceName)
                        .AddSource(UserRepository.ActivitySourceName)
                        .AddAspNetCoreInstrumentation((options) => options.Enrich
                        = (activity, eventName, rawObject) =>
                        {
                            if (eventName.Equals("OnStartActivity"))
                            {
                                if (rawObject is HttpRequest httpRequest)
                                {
                                    activity.SetTag("requestProtocol", httpRequest.Protocol);
                                }
                            }
                            else if (eventName.Equals("OnStopActivity"))
                            {
                                if (rawObject is HttpResponse httpResponse)
                                {
                                    activity.SetTag("responseLength", httpResponse.ContentLength);
                                }
                            }
                        })
                        .AddHttpClientInstrumentation()
                        .AddGrpcClientInstrumentation()
                        .AddSqlClientInstrumentation(options =>
                        {
                            options.EnableConnectionLevelAttributes = true;
                            options.SetTextCommandContent = true;
                        })
                        .AddConsoleExporter());
                    break;
            }
        }
        */
        /*
        private void SetupJWTServices(IServiceCollection services)
        {
            var jwtSigningKeyProvider = new JwtSigningKeyProvider();
            services.AddSingleton<IJwtSigningKeyProvider>(jwtSigningKeyProvider);

            var jwtTokenConfig = this.Configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();
            services.AddSingleton(jwtTokenConfig);

            services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(
                options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtTokenConfig.Issuer,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = jwtSigningKeyProvider.GetSymmetricSecurityKey(jwtTokenConfig.Secret),
                        ValidAudience = jwtTokenConfig.Audience,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1)
                    };
                });
        }
        */
    }
}
