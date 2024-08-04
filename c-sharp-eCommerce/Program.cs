using c_shap_eCommerce.Core.IRepositories;
using c_sharp_eCommerce.Infrastructure.Data;
using c_sharp_eCommerce.Infrastructure.Repositories;
using c_sharp_eCommerce.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using c_sharp_eCommerce.Infrastructure.Helpers;
using c_shap_eCommerce.Core.DTOs.ApiResponseHandlers;
using c_shap_eCommerce.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using c_shap_eCommerce.Core.IServices;
using c_sharp_eCommerce.Services;

namespace c_sharp_eCommerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConfiguration"))
            );
            // Add services to the container.

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

            builder.Services.AddResponseCaching();
            builder.Services.AddScoped(typeof(IProductsRepository), typeof(ProductsRepository));
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddScoped(typeof(IUsersRepository), typeof(UsersRepository));
			builder.Services.AddScoped(typeof(ITokenService), typeof(TokenService));
			builder.Services.AddScoped(typeof(IEmailService), typeof(EmailService));

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

            var key = builder.Configuration.GetValue<string>("ApiSettings:JWTSecretKey");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                };
            });


            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (ActionContext) =>
                {
                    var errorsMessages = ValidationHelper.GetValidationErrors(ActionContext);
                    var response = new ApiValidationResponse(System.Net.HttpStatusCode.BadRequest,errorsMessages);

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
            
            app.UseHttpsRedirection();

            app.UseAuthorization();
            
            app.MapControllers();

            app.Run();
        }
    }
}
