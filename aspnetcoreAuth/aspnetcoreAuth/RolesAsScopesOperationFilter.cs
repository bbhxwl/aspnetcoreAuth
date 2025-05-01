using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace aspnetcoreAuth;

public class RolesAsScopesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation op, OperationFilterContext ctx)
    {
        var attr = ctx.MethodInfo
            .GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .FirstOrDefault(a => !string.IsNullOrEmpty(a.Roles));
        if (attr == null) return;

        var roles = attr.Roles.Split(',').Select(r => r.Trim()).ToList();
        op.Security = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                [ new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id   = "oauth2" } } ] = roles
            }
        };
    }
}
