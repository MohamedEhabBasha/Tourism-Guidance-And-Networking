using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using System.Text;
using Tourism_Guidance_And_Networking.Core.Helper;
using Tourism_Guidance_And_Networking.Core.Interfaces;
using Tourism_Guidance_And_Networking.Core.Models;
using Tourism_Guidance_And_Networking.DataAccess;
using Tourism_Guidance_And_Networking.DataAccess.Data;
using Tourism_Guidance_And_Networking.DataAccess.DbInitializer;
using Tourism_Guidance_And_Networking.Web.Services;
using Tourism_Guidance_And_Networking.Web.Services.Hubs;
namespace Tourism_Guidance_And_Networking.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyHeader()
                                  .AllowAnyMethod());
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Tourism Guidance And Networking",
                    Description = "",
                    Contact = new OpenApiContact()
                    {
                        Name = "",
                        Email = "",
                        Url = new Uri("https://mydomain.com")
                    }
                });

                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter the JWT Key"
                });

                o.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    {
                       new OpenApiSecurityScheme()
                       {
                          Reference = new OpenApiReference()
                          {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer"
                          },
                          Name = "Bearer",
                          In = ParameterLocation.Header
                       },
                       new List<string>()
                    }
                });
            });


            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("ServerConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAuthService, AuthService>();
           // builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddMvc();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
                        ClockSkew = TimeSpan.Zero // do not give more time than expiration
                    };
                });

            // Hubs
            builder.Services.AddSignalR();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // if (app.Environment.IsDevelopment())
            // {
            app.UseSwagger();
            app.UseSwaggerUI();
            //}
            app.UseCors("AllowAnyOrigin");

            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
            //SeedDatabase();


            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            // Hubs
            app.MapHub<ChatHub>("/services/hubs/chathub");

            app.Run();



            //void SeedDatabase()
            //{
            //    using (var scope = app.Services.CreateScope())
            //    {
            //        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            //        dbInitializer.Initialize();
            //    }
            //}            
        }
    }
}