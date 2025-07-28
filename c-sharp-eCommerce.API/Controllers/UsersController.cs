using AutoMapper;
using c_sharp_eCommerce.Core.DTOs.Users;
using c_sharp_eCommerce.Core.Models;
using c_sharp_eCommerce.Core.DTOs.ApiResponse;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using c_sharp_eCommerce.Core.IRepositories;
using c_sharp_eCommerce.Core.IServices;
using c_sharp_eCommerce.Core.Exceptions;
using FluentValidation;

namespace c_sharp_eCommerce.API.Controllers
{
	[Route("api/users")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUsersRepository _usersRepository;
		private readonly UserManager<User> _userManager;
		private readonly IEmailService _emailService;
		private readonly IMapper _mapper;
		private readonly IValidator<RegisterationRequestDto> _registerationRequestValidator;
		private readonly IValidator<LoginRequestDto> _loginRequestValidator;
		private readonly IValidator<SendEmailDto> _sendEmailDtoValidator;
		private readonly IValidator<ResetPasswordDto> _resetPasswordRequestValidator;

		public UsersController(
			IUsersRepository usersRepository, IMapper mapper,
			UserManager<User> userManager, IEmailService emailService,
			IValidator<RegisterationRequestDto> registerationRequestValidator, IValidator<LoginRequestDto> loginRequestValidator
			, IValidator<SendEmailDto> sendEmailDtoValidator, IValidator<ResetPasswordDto> resetPasswordRequestValidator)
		{
			_usersRepository = usersRepository;
			_mapper = mapper;
			_userManager = userManager;
			_emailService = emailService;
			_registerationRequestValidator = registerationRequestValidator;
			_loginRequestValidator = loginRequestValidator;
			_sendEmailDtoValidator = sendEmailDtoValidator;
			_resetPasswordRequestValidator = resetPasswordRequestValidator;
		}
		[HttpPost("register")]
		public async Task<ActionResult> Register([FromBody] RegisterationRequestDto dto)
		{
			_registerationRequestValidator.ValidateAndThrow(dto);
			bool isUniqueEmail = _usersRepository.IsUniqueUser(dto.Email);
			if (!isUniqueEmail)
			{
				var message = $"User with email: {dto.Email} already exists!";
				var errResponse = new ApiResponse(HttpStatusCode.Conflict, message);
				return Conflict(errResponse);
			}

			var createdUser = await _usersRepository.Register(dto);
			if (createdUser == null)
			{
				var message = "an error has occurred during user creation";
				var errResponse = new ApiResponse(HttpStatusCode.InternalServerError, message);
				return StatusCode(StatusCodes.Status500InternalServerError, errResponse);
			}

			var loginReq = new LoginRequestDto()
			{
				Email = dto.Email,
				Password = dto.Password,
			};

			var token = (await _usersRepository.Login(loginReq)).Token;
			var data = new Dictionary<string, object>
			{
				{ "user", createdUser },
				{ "token", token }
			};

			var response = new ApiResponse(HttpStatusCode.Created, data);
			return StatusCode(201, response);
		}

		[HttpPost("login")]
		public async Task<ActionResult> Login([FromBody] LoginRequestDto dto)
		{
			_loginRequestValidator.ValidateAndThrow(dto);
			try
			{
				var loginResponse = await _usersRepository.Login(dto);
				var userDto = _mapper.Map<UserDto>(loginResponse.User);
				var data = new Dictionary<string, object>
				{
					{ "user", userDto },
					{ "token", loginResponse.Token }
				};

				var response = new ApiResponse(HttpStatusCode.OK, data);
				return Ok(response);
			}
			catch (UnauthorizedException ex)
			{
				return Unauthorized(new ApiResponse(HttpStatusCode.Unauthorized, ex.Message));
			}

		}

		[HttpPost("SendEmail")]
		public async Task<ActionResult> SendEmailToUser([FromBody] SendEmailDto dto)
		{
			_sendEmailDtoValidator.ValidateAndThrow(dto);
			var user = await _userManager.FindByEmailAsync(dto.Email);
			if (user == null)
			{
				var errResponse = new ApiResponse(HttpStatusCode.BadRequest, $"User with email: {dto.Email} does not exist");
				return BadRequest(errResponse);
			}

			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
			var forgotPasswordLink = Url.Action("ResetPassword", "Users", new { token, email = user.Email });
			var subject = "Reset Your Password";
			var message = $"Please click on the link to reset your password: {forgotPasswordLink}";

			bool isSent = await _emailService.SendEmailAsync(subject, user.Email!, message);
			if (!isSent)
			{
				var errResponse = new ApiResponse(HttpStatusCode.InternalServerError, "An unexpected error has occurred please try again later");
				return StatusCode(StatusCodes.Status500InternalServerError, errResponse);
			}

			var response = new ApiResponse(HttpStatusCode.OK, "Password link has been sent to your email.");
			return Ok(response);
		}

		[HttpPost("ResetPassword")]
		public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
		{
			_resetPasswordRequestValidator.ValidateAndThrow(dto);
			var user = await _userManager.FindByEmailAsync(dto.Email);
			if (user == null)
			{
				var errResponse = new ApiResponse(HttpStatusCode.BadRequest, $"User with the provided email: {dto.Email} does not exist");
				return BadRequest(errResponse);
			}
			var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
			if (!result.Succeeded)
			{
				var errResponse = new ApiResponse(HttpStatusCode.InternalServerError, false, "Something went wring during reseting password");
				return StatusCode(StatusCodes.Status500InternalServerError, errResponse);
			}

			var successRes = new ApiResponse(HttpStatusCode.Accepted, "Password has been reset successfully!");
			return Accepted(successRes);
		}
	}
}
