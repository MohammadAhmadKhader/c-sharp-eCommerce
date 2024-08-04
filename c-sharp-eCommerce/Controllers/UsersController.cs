using AutoMapper;
using c_shap_eCommerce.Core.DTOs.Users;
using c_shap_eCommerce.Core.Models;
using c_sharp_eCommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using c_shap_eCommerce.Core.IRepositories;
using c_shap_eCommerce.Core.IServices;

namespace c_sharp_eCommerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUsersRepository usersRepository;
		private readonly UserManager<User> userManager;
		private readonly IEmailService emailService;
		private readonly IMapper mapper;
		public UsersController(IUsersRepository usersRepository, IMapper mapper, UserManager<User> userManager, IEmailService emailService)
		{
			this.usersRepository = usersRepository;
			this.mapper = mapper;
			this.userManager = userManager;
			this.emailService = emailService;
		}
		[HttpPost("register")]
		public async Task<ActionResult> Register([FromBody] RegisterationRequestDto registerationRequest)
		{
			bool isUniqueEmail = usersRepository.IsUniqueUser(registerationRequest.Email);
			if (!isUniqueEmail)
			{
				var message = $"User with email: {registerationRequest.Email} already exists!";
				var errResponse = new ApiResponse(HttpStatusCode.Conflict, message);
				return Conflict(errResponse);
			}

			var createdUser = await usersRepository.Register(registerationRequest);
			if (createdUser == null)
			{
				var message = "an error has occurred during user creation";
				var errResponse = new ApiResponse(HttpStatusCode.InternalServerError, message);
				return StatusCode(StatusCodes.Status500InternalServerError, errResponse);
			}

			var loginReq = new LoginRequestDto()
			{
				Email = registerationRequest.Email,
				Password = registerationRequest.Password,
			};

			var token = (await usersRepository.Login(loginReq)).Token;
			var data = new Dictionary<string, object>();
			data.Add("user", createdUser);
			data.Add("token", token);

			var response = new ApiResponse(HttpStatusCode.Created, data);
			return StatusCode(201, response);
		}

		[HttpPost("login")]
		public async Task<ActionResult> Login([FromBody] LoginRequestDto loginRequest)
		{
			var loginResponse = await usersRepository.Login(loginRequest);
			if(loginResponse.User == null)
			{
				var message = "Email or password is wrong";
				var errResponse = new ApiResponse(HttpStatusCode.Unauthorized, message);
				return Unauthorized(errResponse);
			}
			var userDto = mapper.Map<UserDto>(loginResponse.User);
			var data = new Dictionary<string, object>();
			data.Add("user", userDto);
			data.Add("token", loginResponse.Token);

			var response = new ApiResponse(HttpStatusCode.OK, data);
			return Ok(response);
		}

		[HttpPost("SendEmail")]
		public async Task<ActionResult> SendEmailToUser(string email)
		{
			var user = await userManager.FindByEmailAsync(email);
			if(user == null)
			{
				var errResponse = new ApiResponse(HttpStatusCode.BadRequest, $"User with email: {email} does not exist");
				return BadRequest(errResponse);
			}

			var token = await userManager.GeneratePasswordResetTokenAsync(user);
			var forgotPasswordLink = Url.Action("ResetPassword", "Users", new { token = token, email = user.Email});
			var subject = "Reset Your Password";
			var message = $"Please click on the link to reset your password: {forgotPasswordLink}";
			
			bool isSent = await emailService.SendEmailAsync(subject, user.Email, message);
			if (!isSent)
			{
				var errResponse = new ApiResponse(HttpStatusCode.InternalServerError, "An unexpected error has occurred please try again later");
				return StatusCode(StatusCodes.Status500InternalServerError,errResponse);
			}
	
			var response = new ApiResponse(HttpStatusCode.OK, "Password link has been sent to your email.");
			return Ok(response);
		}

		[HttpPost("ResetPassword")]
		public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordRequest)
		{
			var user = await userManager.FindByEmailAsync(resetPasswordRequest.Email);
			if(user == null)
			{
				var errResponse = new ApiResponse(HttpStatusCode.BadRequest, $"User with the provided email: {resetPasswordRequest.Email} does not exist");
				return BadRequest(errResponse);
			}
			var result = await userManager.ResetPasswordAsync(user,resetPasswordRequest.Token, resetPasswordRequest.NewPassword);
			if (!result.Succeeded)
			{
				return StatusCode(StatusCodes.Status500InternalServerError,"Something went wring during reseting password");
			}

			var successRes = new ApiResponse(HttpStatusCode.Accepted, "Password has been reset successfully!");
			return Accepted(successRes);
		}
	}
}
