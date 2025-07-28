using AutoMapper;
using c_sharp_eCommerce.Core.DTOs.Users;
using c_sharp_eCommerce.Core.Exceptions;
using c_sharp_eCommerce.Core.IRepositories;
using c_sharp_eCommerce.Core.IServices;
using c_sharp_eCommerce.Core.Models;
using c_sharp_eCommerce.Infrastructure.Data;
using c_sharp_eCommerce.Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;

namespace c_sharp_eCommerce.Infrastructure.Repositories
{
	public class UsersRepository : IUsersRepository
	{
		private readonly AppDbContext _context;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly ITokenService _tokenService;
		private readonly IMapper _mapper;
		public UsersRepository(AppDbContext context, UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, ITokenService tokenService)
		{
			_context = context;
			_userManager = userManager;
			_signInManager = signInManager;
			_mapper = mapper;
			_tokenService = tokenService;
		}
		public bool IsUniqueUser(string Email)
		{
			var result = _context.Users.FirstOrDefault(x => x.NormalizedEmail == Email.ToUpper());
			return result == null;
		}

		public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
		{
			var user = await _userManager.FindByEmailAsync(loginRequest.Email);
			if (user == null)
			{
				var message = "User email or password is wrong";
				throw new UnauthorizedException(message);
			}

			var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
			if (checkPassword == null || !checkPassword.Succeeded)
			{
				var message = "User email or password is wrong";
				throw new UnauthorizedException(message);
			}

			var roles = await _userManager.GetRolesAsync(user);

			return new LoginResponseDto()
			{
				User = _mapper.Map<UserDto>(user),
				Token = await _tokenService.CreateTokenAsync(user),
				Role = string.Join(", ", roles),
			};

		}

		public async Task<UserDto> Register(RegisterationRequestDto registerationRequest)
		{
			using (var transaction = await _context.Database.BeginTransactionAsync())
			{
				var user = new User
				{
					Email = registerationRequest.Email,
					UserName = registerationRequest.Email.Split("@")[0],
					FirstName = registerationRequest.FirstName,
					LastName = registerationRequest.LastName,
				};

				var result = await _userManager.CreateAsync(user, registerationRequest.Password);
				if (!result.Succeeded)
				{
					var errors = UserHelper.CaptureManagerError(result.Errors);
					await transaction.RollbackAsync();
					throw new Exception($"User Registeration has failed, errors: {errors}");
				}
				// by default user on Registeration is set to "User"
				var userRoleResult = await _userManager.AddToRoleAsync(user, "User");
				if (!userRoleResult.Succeeded)
				{
					await transaction.RollbackAsync();
					var errors = UserHelper.CaptureManagerError(userRoleResult.Errors);
					throw new Exception($"Error during creating user role, errors: {errors}");
				}

				await transaction.CommitAsync();
				var returnedUser = _context.Users.FirstOrDefault(user => user.Email == registerationRequest.Email);
				var returnedUserDto = _mapper.Map<User, UserDto>(returnedUser!);
				return returnedUserDto;
			}
			;
		}
	}
}
