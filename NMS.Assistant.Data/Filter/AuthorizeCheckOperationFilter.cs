using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using NMS.Assistant.Domain.Constants;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NMS.Assistant.Data.Filter
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            bool isAuthorized = context.MethodInfo.GetAttributes<AuthorizeAttribute>().Any();
            bool isBasicAuth = context.ApiDescription.RelativePath.Equals("auth", StringComparison.InvariantCultureIgnoreCase);

            if (isAuthorized || isBasicAuth)
            {
                if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();

                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
            }

            List<OpenApiSecurityRequirement> security = new List<OpenApiSecurityRequirement>();
            if (isBasicAuth) security.Add(BasicAuthRequirement());
            if (isAuthorized) security.Add(JwtAuthRequirement());
            operation.Security = security;
        }
        
        private OpenApiSecurityRequirement JwtAuthRequirement() =>
            new OpenApiSecurityRequirement
            {
                { new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = ApiAuthScheme.JwtBearer
                    },
                    Scheme = "oauth2",
                    Name = ApiAuthScheme.JwtBearer,
                    In = ParameterLocation.Header,

                }, new List<string>() }
            };
        
        private OpenApiSecurityRequirement BasicAuthRequirement() =>
            new OpenApiSecurityRequirement
            {
                { new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = ApiAuthScheme.Basic
                    },
                    Scheme = ApiAuthScheme.Basic,
                    Name = ApiAuthScheme.Basic,
                    In = ParameterLocation.Header,

                }, new List<string>() }
            };
    }
}
