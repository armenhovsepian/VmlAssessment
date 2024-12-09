using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Vml.Application.Validators;
using Vml.Core.Dtos;
using Vml.Core.Exceptions;
using Vml.Core.Interfaces;
using Vml.Infrastructure.Data;
using Vml.Infrastructure.Data.Repositories;


namespace Vml.Api.Configurtions
{
    public static class Configures
    {
        public static WebApplicationBuilder ConfigureProblemDetails(this WebApplicationBuilder builder)
        {
            // Configure problem details
            builder.Services.AddProblemDetails(options =>
            {
                // Only include exception details in a development environment. There's really no need
                // to set this as it's the default behavior. It's just included here for completeness :)
                options.IncludeExceptionDetails = (ctx, ex) => builder.Environment.IsDevelopment();

                // This will map DataJobException to the 400 BadRequest status code and return custom problem details.
                options.Map<DataJobException>(ex => new ProblemDetails
                {
                    Title = "Could not initiate DataJob.",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = ex.Message,
                });

                // This will map NotImplementedException to the 501 Not Implemented status code.
                //options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);

                // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
                // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
                options.Map<Exception>(ex => new ProblemDetails
                {
                    Title = "Contact with Administrator",
                    Status = StatusCodes.Status500InternalServerError
                });
            });

            return builder;
        }


        public static WebApplicationBuilder ConfigureInMemoryDatabase(this WebApplicationBuilder builder)
        {
            // Add framework services.
            builder.Services.AddDbContext<DataProcesserContext>(opt => opt.UseInMemoryDatabase("VML interview"));
            builder.Services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            return builder;
        }


        public static WebApplicationBuilder ConfigureSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            });

            return builder;
        }

        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IDataProcesserRepository, DataProcesserRepository>();
            builder.Services.AddScoped<Core.Interfaces.V1.IDataProcesserService, Application.Services.V1.DataProcesserService>();

            builder.Services.AddScoped<IValidator<DataJobDTO>, DataJobDtoValidator>();

            return builder;
        }
    }
}
