using c_sharp_eCommerce.Core.IRepositories;
using c_sharp_eCommerce.Infrastructure.Data;
using c_sharp_eCommerce.Infrastructure.Repositories;
using c_sharp_eCommerce.API.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using c_sharp_eCommerce.Infrastructure.Helpers;
using c_sharp_eCommerce.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using c_sharp_eCommerce.Core.IServices;
using c_sharp_eCommerce.Services;
using CloudinaryDotNet;
using c_sharp_eCommerce.Infrastructure.Middlewares;
using c_sharp_eCommerce.API.Services.Validations;
using c_sharp_eCommerce.Core.DTOs.ApiResponse;
using c_sharp_eCommerce.Services.Extensions;
using System.Net;
using c_sharp_eCommerce.Services.Options;

namespace c_sharp_eCommerce.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConfiguration")));

            builder.Services.AddControllers(options => options.CacheProfiles.Add("defaultCache",
                new CacheProfile
                {
                    Duration = 30,
                    Location = ResponseCacheLocation.Any,
                })
            );

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAppOptions(builder.Configuration);

            builder.Services.AddResponseCaching();
            builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddSingleton<Cloudinary>();
            builder.Services.AddValidators();


            builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(30);
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var apiSettings = builder.Configuration.GetSection(nameof(ApiSettings)).Get<ApiSettings>();
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(apiSettings.JWTSecretKey)),
                };
            });

            builder.Services.AddHttpContextAccessor();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (ActionContext) =>
                {
                    var errorsMessages = ValidationHelper.GetValidationErrors(ActionContext);
                    var response = new ApiValidationResponse(HttpStatusCode.BadRequest, errorsMessages);

                    return new BadRequestObjectResult(response);
                };
            });

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<GlobalExceptionsMiddleware>();
            //app.UseMiddleware<PaginationMiddleware>(); // this middleware has been set to Obselete, pagination handling was moved to controllers

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
