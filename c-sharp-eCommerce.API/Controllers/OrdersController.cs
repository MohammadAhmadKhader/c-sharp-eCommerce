using AutoMapper;
using c_sharp_eCommerce.Core.DTOs.OrderDetails;
using c_sharp_eCommerce.Core.DTOs.Orders;
using c_sharp_eCommerce.Core.IRepositories;
using c_sharp_eCommerce.Core.IServices;
using c_sharp_eCommerce.Core.Models;
using c_sharp_eCommerce.Core.DTOs.ApiResponse;
using c_sharp_eCommerce.Infrastructure.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace c_sharp_eCommerce.API.Controllers
{
	[Route("api/orders")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
		private readonly IUnitOfWork<Order> _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IValidator<OrderCreateDto> _orderCreateValidator;
		private readonly ITokenService _tokenService;

		public OrdersController(IUnitOfWork<Order> unitOfWork, IMapper mapper, IValidator<OrderCreateDto> orderCreateValidator, ITokenService tokenService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_orderCreateValidator = orderCreateValidator;
			_tokenService = tokenService;
		}
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetAllOrders(
			[FromQuery] int page = PaginationHelper.DefaultPage,
			[FromQuery] int limit = PaginationHelper.DefaultLimit)
		{
			var validatedPage = PaginationHelper.ValidatePage(page);
			var validatedLimit = PaginationHelper.ValidateLimit(limit);
			var (orders, count) = await _unitOfWork.OrderRepository.GetAll(validatedPage, validatedLimit);

			var data = new { page = validatedPage, limit = validatedLimit, count, orders = orders };
			var response = new ApiResponse(HttpStatusCode.OK, data);
			return Ok(response);
		}

		[HttpGet("{id}")]
		[Authorize]
		public async Task<IActionResult> GetOrderById(int id)
		{
			var order = await _unitOfWork.OrderRepository.GetById(id);
			if (order == null)
			{
				var errResponse = new ApiResponse(HttpStatusCode.BadRequest, $"order with Id {id} was not found");
				return BadRequest(errResponse);
			}

			var data = new Dictionary<string, object>
            {
                { "order", order }
            };
			var response = new ApiResponse(HttpStatusCode.OK, data);
			return Ok(response);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
		{
			_orderCreateValidator.ValidateAndThrow(dto);

			using var transaction = await _unitOfWork.StartTransactionAsync();
			try
			{
				var productIds = dto.Items.Select(item => item.ProductId).ToList();
				var productQuantityMap = ProductHelper.CreateProductIdQuantityMap(dto);
				var products = await _unitOfWork.ProductRepository.GetProductsByIds(productIds);

				foreach (var product in products)
				{
					if (product.Quantity < productQuantityMap[product.Id])
					{
						var message = $"product with id {product.Id} has only available: {product.Quantity} and you have requested: {productQuantityMap[product.Id]}";
						var errResponse = new ApiResponse(HttpStatusCode.BadRequest, message);
						return BadRequest(errResponse);
					}
				}

				var userId = _tokenService.GetTokenData().UserId;
				var order = new Order
				{
					UserId = Guid.Parse(userId),
					Status = "pending",
				};
				await _unitOfWork.OrderRepository.Create(order);
				await _unitOfWork.SaveAsync();

				foreach (var item in dto.Items)
				{
					var product = products.FirstOrDefault(prod => prod.Id == item.ProductId);
					if (product == null)
					{
						var message = $"Something went wrong during checking project with id {item.ProductId}";
						var errResponse = new ApiResponse(HttpStatusCode.BadRequest, message);

						await transaction.RollbackAsync();
						return StatusCode(StatusCodes.Status500InternalServerError, errResponse);
					}
					var orderItem = new OrderDetailsDto
					{
						ProductId = item.ProductId,
						Quantity = item.Quantity,
						Price = product.Price,
						OrderId = order.Id,
					};

					var mappedOrderDetails = _mapper.Map<OrderDetailsDto, OrderDetails>(orderItem);
					order.OrderDetails.Add(mappedOrderDetails);
					product.Quantity = product.Quantity - productQuantityMap[product.Id];
				}

				await _unitOfWork.SaveAsync();

				var mappedOrder = _mapper.Map<Order, OrderDto>(order);
				var response = new ApiResponse(HttpStatusCode.OK, new { order = mappedOrder });

				await transaction.CommitAsync();
				return Ok(response);

			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				var message = "An error has occurred while creating the order";
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
				var errResponse = new ApiResponse(HttpStatusCode.InternalServerError, message);
				return StatusCode(StatusCodes.Status500InternalServerError, errResponse);
			}

		}

		[HttpPatch("cancel/{Id}")]
		[Authorize]
		public async Task<ActionResult> CancelOrder(int Id)
		{
			var order = await _unitOfWork.OrderRepository.GetById(Id);
			if (order is null)
			{
				var message = "Order was not found.";
				var errResponse = new ApiResponse(HttpStatusCode.BadRequest, message);

				return BadRequest(errResponse);
			}

			var tokenUserId = _tokenService.GetTokenData().UserId;
			if (order.UserId.ToString() != tokenUserId)
			{
				return Unauthorized();
			}

			var errMessage = await _unitOfWork.OrderRepository.CancelOrderById(Id);
			await _unitOfWork.SaveAsync();
			if (errMessage != null)
			{
				var errResponse = new ApiResponse(HttpStatusCode.BadRequest, errMessage);
				return BadRequest(errResponse);
			}

			var response = new ApiResponse(HttpStatusCode.Accepted, true, "success");

			return Accepted(response);
		}
	}
}
