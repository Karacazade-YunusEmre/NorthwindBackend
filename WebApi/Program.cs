using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.DependencyResolvers.Autofac;
using Core.Entities.Concrete.Authentication;
using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DependencyInjection Default Style
// builder.Services.AddScoped<IProductRepository, EfProductRepository>();
// builder.Services.AddScoped<ICategoryRepository, EfCategoryRepository>();
// builder.Services.AddScoped<IProductService, ProductManager>();
// builder.Services.AddScoped<ICategoryService, CategoryManager>();

// DependencyInjection Autofac Style
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new AutofacBusinessModule());
    });

// **********************************************************************

// Database Connection
// var connectionString = builder.Configuration.GetConnectionString("Connection");
// builder.Services.AddDbContext<NorthwindContext>(options =>
//     options.UseSqlite(connectionString));

// Cors bir güvenlik önlemidir
// Bu yapıyla dışarıdan yani 3000 portundan istek gelirse
// cevap ver demiş olduk
builder.Services.AddCors(options => options.AddPolicy(
    name: "AllowOrigin",
    policyBuilder => policyBuilder.WithOrigins("http://localhost:3000"))
);

// Rol bazlı attribute'lar çalışması için gerekli
builder.Services.AddIdentity<User, IdentityRole>(
        options => options.User.RequireUniqueEmail = true
    )
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddEntityFrameworkStores<NorthwindContext>().AddDefaultTokenProviders();

// SecretKey Configuration dosyasında tanımladığımız Secret değeridir
// Bu değer ile Jwt token oluşturulduğu için oldukça önemlidir
var secretStr = builder.Configuration["Application:Secret"];
var secret = Encoding.UTF8.GetBytes(secretStr!);

// JwtBearer için authentication tanımlamaları
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Audience = builder.Configuration["Application:Audience"];
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(secret),
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = true,
        // ClockSkew = TimeSpan.Zero
    };
});


// SwaggerUI ile Authorization işlemleri için gerekli
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
            { Title = "JwtTokenWithIdentity", Version = "v1", Description = "JwtTokenWithIdentity test app" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "Enter 'Bearer' [space] and then your valid token in the text input below." +
            "\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
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
                }
            },
            Array.Empty<string>()
        }
    });
});

// **********************************************************************


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JwtTokenWithIdentity v1"));
}

// **************************************************************

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.UseCors(corsPolicyBuilder => corsPolicyBuilder
    .SetIsOriginAllowed(_ => true)
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .WithOrigins("http://localhost:3000").AllowAnyHeader()
);


// **************************************************************

app.UseHttpsRedirection();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();