using AutoMapper;
using c_shap_eCommerce.Core.DTOs.Users;
using c_shap_eCommerce.Core.IRepositories;
using c_shap_eCommerce.Core.IServices;
using c_shap_eCommerce.Core.Models;
using c_sharp_eCommerce.Infrastructure.Data;
using c_sharp_eCommerce.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_eCommerce.Infrastructure.Repositories
{
	public class UsersRepository : IUsersRepository
	{
		private readonly AppDbContext dbContext;
		private readonly UserManager<User> userManager;
		private readonly SignInManager<User> signInManager;
		private readonly ITokenService tokenService;
		private readonly IMapper mapper;
		public UsersRepository(AppDbContext dbContext, UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, ITokenService tokenService)
		{
			this.dbContext = dbContext;
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.mapper = mapper;
			this.tokenService = tokenService;
		}
		public bool IsUniqueUser(string Email)
		{
			var result = dbContext.Users.FirstOrDefault(x => x.NormalizedEmail == Email.ToUpper());
			return result == null;
		}

		public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
		{
			var user = await userManager.FindByEmailAsync(loginRequest.Email);
			var checkPassword = await signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
			if (!checkPassword.Succeeded)
			{
				return new LoginResponseDto()
				{
					User = null,
					Token = "0,"
				};
			}
			var roles = await userManager.GetRolesAsync(user);
			return new LoginResponseDto()
			{
				User = mapper.Map<UserDto>(user),
				Token = await tokenService.CreateTokenAsync(user),
				Role = string.Join(", ", roles),
			};
		}

		public async Task<UserDto> Register(RegisterationRequestDto registerationRequest)
		{
			using (var transaction = await dbContext.Database.BeginTransactionAsync())
			{
				try
				{
					var user = new User
					{
						Email = registerationRequest.Email,
						UserName = registerationRequest.Email.Split("@")[0],
						FirstName = registerationRequest.FirstName,
						LastName = registerationRequest.LastName,
					};

					var result = await userManager.CreateAsync(user, registerationRequest.Password);
					if (!result.Succeeded)
					{
						var errors = UserHelper.CaptureManagerError(result.Errors);
						await transaction.RollbackAsync();
						throw new Exception($"User Registeration has failed, errors: {errors}");
					}
				
					var userRoleResult = await userManager.AddToRoleAsync(user, "User");
					if (!userRoleResult.Succeeded)
					{
						await transaction.RollbackAsync();
						var errors = UserHelper.CaptureManagerError(userRoleResult.Errors);
						throw new Exception($"Error during creating user role, errors: {errors}");
					}

					await transaction.CommitAsync();
					var returnedUser = dbContext.Users.FirstOrDefault(user => user.Email == registerationRequest.Email);
					var returnedUserDto = mapper.Map<User, UserDto>(returnedUser!);
					return returnedUserDto;
				}
				catch
				{
					throw;
				}
			};
		}
	}
}
