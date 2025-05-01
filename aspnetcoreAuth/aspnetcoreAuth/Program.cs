using System.Text;
using aspnetcoreAuth;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "授权(数据将在请求头中进行传递)直接在下面框中输入{Authorization}(注意三者其中之一) ",
        Name = "Authorization",//参数名称
        In = ParameterLocation.Header,//存放Authorization信息的位置(请求头中)
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference()
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            }, Array.Empty<string>()
        }
    });
    options.OperationFilter<RolesAsScopesOperationFilter>();
   options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("123456789123456789")),    // 加密解密Token的密钥 
        // 是否验证发布者 
        ValidateIssuer = true,
        // 发布者名称 
        ValidIssuer = "1",
        // 是否验证订阅者 
        // 订阅者名称 
        ValidateAudience = true,
        ValidAudience = "2",
        // 是否验证令牌有效期 
        ValidateLifetime = true,
        // 每次颁发令牌，令牌有效时间 
        ClockSkew = TimeSpan.FromMinutes(10),
        RoleClaimType = "r"
    };
});;
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}