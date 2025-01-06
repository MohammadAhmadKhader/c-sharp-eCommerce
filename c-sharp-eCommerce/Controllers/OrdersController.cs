using AutoMapper;
using c_shap_eCommerce.Core.DTOs.ApiResponseHandlers;
using c_shap_eCommerce.Core.DTOs.OrderDetails;
using c_shap_eCommerce.Core.DTOs.Orders;
using c_shap_eCommerce.Core.IRepositories;
using c_shap_eCommerce.Core.IServices;
using c_shap_eCommerce.Core.Models;
using c_sharp_eCommerce.Infrastructure.Helpers;
using FluentValidation;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Crud;
using MySqlX.XDevAPI.Common;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using Order = c_shap_eCommerce.Core.Models.Order;

namespace c_sharp_eCommerce.Controllers
{
	[Route("api/orders")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
		private readonly IUnitOfWork<Order> unitOfWork;
		private readonly IMapper mapper;
		private readonly IValidator<OrderCreateDto> orderCreateValidator;
		private readonly ITokenService tokenService;

		public OrdersController(IUnitOfWork<Order> unitOfWork, IMapper mapper, IValidator<OrderCreateDto> orderCreateValidator, ITokenService tokenService)
		{
			this.unitOfWork = unitOfWork;
			this.mapper = mapper;
			this.orderCreateValidator = orderCreateValidator;
			this.tokenService = tokenService;
		}
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetAllOrders([FromQuery] int Page = PaginationHelper.DefaultPage, [FromQuery] int Limit = PaginationHelper.DefaultLimit)
		{
            var validatedPage = PaginationHelper.ValidatePage(Page);
			var validatedLimit = PaginationHelper.ValidateLimit(Limit);
			var (orders, count) = await unitOfWork.orderRepository.GetAll(validatedPage, validatedLimit);

            var data = new { page= validatedPage, limit= validatedLimit, count, orders = orders };
			var response = new ApiResponse(HttpStatusCode.OK, data);
			return Ok(response);
		}

		[HttpGet("{Id}")]
		[Authorize]
		public async Task<IActionResult> GetOrderById(int Id)
		{
			var order = await unitOfWork.orderRepository.GetById(Id);
			if(order == null)
			{
				var errResponse = new ApiResponse(HttpStatusCode.BadRequest, $"order with Id {Id} was not found");
				return BadRequest(errResponse);
			}

			var data = new Dictionary<string, object>();
			data.Add("order", order);
			var response = new ApiResponse(HttpStatusCode.OK, data);
			return Ok(response);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Create([FromBody] OrderCreateDto payload)
		{
			orderCreateValidator.ValidateAndThrow(payload);
			
			using var transaction = await unitOfWork.startTransactionAsync();
			try
			{	
				var productIds = payload.Items.Select(item => item.ProductId).ToList();
				var productQuantityMap = ProductHelper.CreateProductIdQuantityMap(payload);
				var products = await unitOfWork.productRepository.GetProductsByIds(productIds);

				foreach (var product in products)
				{
					if (product.Quantity < productQuantityMap[product.Id])
					{
						var message = $"product with id {product.Id} has only available: {product.Quantity} and you have requested: {productQuantityMap[product.Id]}";
						var errResponse = new ApiResponse(HttpStatusCode.BadRequest, message);
						return BadRequest(errResponse);
					}
				}

				var userId = tokenService.GetTokenData().UserId;
				var order = new Order
				{
					UserId =  Guid.Parse(userId),
					Status = "pending",
				};
				await unitOfWork.orderRepository.Create(order);
				await unitOfWork.saveAsync();

				foreach (var item in payload.Items)
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

					var mappedOrderDetails = mapper.Map<OrderDetailsDto, OrderDetails>(orderItem);
					order.OrderDetails.Add(mappedOrderDetails);
					product.Quantity = product.Quantity - productQuantityMap[product.Id];
				}

				await unitOfWork.saveAsync();

				var mappedOrder = mapper.Map<Order, OrderDto>(order);
				var response = new ApiResponse(HttpStatusCode.OK, new { order = mappedOrder});

				await transaction.CommitAsync();
				return Ok(response);

			}catch(Exception ex)
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
			var order = await unitOfWork.orderRepository.GetById(Id);
			if(order is null)
			{
                var message = "Order was not found.";
                var errResponse = new ApiResponse(HttpStatusCode.BadRequest, message);

                return BadRequest(errResponse);
            }

			var tokenUserId = tokenService.GetTokenData().UserId;
			if(order.UserId.ToString() != tokenUserId)
			{
                return Unauthorized();
            }

			var errMessage = await unitOfWork.orderRepository.CancelOrderById(Id);
			await unitOfWork.saveAsync();
			if (errMessage != null)
			{
				var errResponse = new ApiResponse(HttpStatusCode.BadRequest, errMessage);
				return BadRequest(errResponse);
			}

			var response = new ApiResponse(HttpStatusCode.Accepted,true, "success");

			return Accepted(response);
		}
	}
}
