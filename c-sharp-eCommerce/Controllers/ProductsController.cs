using AutoMapper;
using c_shap_eCommerce.Core.DTOs.ApiResponseHandlers;
using c_shap_eCommerce.Core.DTOs.Products;
using c_shap_eCommerce.Core.IRepositories;
using c_shap_eCommerce.Core.IServices;
using c_shap_eCommerce.Core.Models;
using c_sharp_eCommerce.Infrastructure.Data;
using c_sharp_eCommerce.Infrastructure.Helpers;
using c_sharp_eCommerce.Infrastructure.Repositories;
using c_sharp_eCommerce.Services;
using c_sharp_eCommerce.Services.Validations;
using CloudinaryDotNet.Actions;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace c_sharp_eCommerce.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork<Product> unitOfWork;
        private readonly IMapper mapper;
        private readonly IImageService imageService;
        private readonly IValidator<ProductCreateDto> productCreateValidator;
        private readonly IValidator<ProductUpdateDto> productUpdateValidator;

		public ProductsController(IUnitOfWork<Product> UnitOfWork, IMapper mapper, IImageService imageService, IValidator<ProductCreateDto> productCreateValidator, IValidator<ProductUpdateDto> productUpdateValidator)
        {
            this.unitOfWork = UnitOfWork;
            this.mapper = mapper;
            this.imageService = imageService;
            this.productCreateValidator = productCreateValidator;
            this.productUpdateValidator = productUpdateValidator;

		}

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllProducts([FromQuery] int page = PaginationHelper.DefaultPage, [FromQuery] int limit = PaginationHelper.DefaultLimit)
        {
			var (validatedPage, validatedLimit) = PaginationHelper.ValidatePageAndLimit(page, limit);
			var (products, count) = await unitOfWork.productRepository.GetAll(validatedPage, validatedLimit, new string[] { "Category" });
            
            var mappedProducts = mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            
            var response = new ApiResponse(
                HttpStatusCode.OK,
                new {page= validatedPage,limit = validatedLimit, count,products = mappedProducts}
            );

			return Ok(response); 
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> GetProductById(int Id)
        {
            var product = await unitOfWork.productRepository.GetProductById(Id);
            bool isEmpty = (product == null);
            if (isEmpty)
            {
                string message = $"no products with {Id} id";
                var NotFoundResult = new ApiResponse(HttpStatusCode.NotFound, message);

                return NotFound(NotFoundResult);
            }
            
            var mappedProduct = mapper.Map<Product, ProductDto>(product!);
            var result = new Dictionary<string, object>() { { "product", mappedProduct } };
            var response = new ApiResponse(HttpStatusCode.OK, result);

            return Ok(response);
        }

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,SuperAdmin")]
		[HttpPost]
        public async Task<ActionResult<ApiResponse>> Create([FromForm] ProductCreateDto productCreateDto)
        {
            productCreateValidator.ValidateAndThrow(productCreateDto);
            var updatedResult = await imageService.UploadImageAsync(productCreateDto.Image, imageService.ProductsFolderPath);
            var url = updatedResult.Url.ToString();

            var product = mapper.Map<Product>(productCreateDto);
            product.Image = url;

            await unitOfWork.productRepository.Create(product);
            await unitOfWork.saveAsync();

            var mappedProduct = mapper.Map<Product, ProductDto>(product);
            var response = new ApiResponse(HttpStatusCode.Created, mappedProduct);

            return Ok(response);
        }

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,SuperAdmin")]
		[HttpPut("{Id}")]
		public async Task<ActionResult<ApiResponse>> Update([FromForm] ProductUpdateDto product, int Id)
        {;
            productUpdateValidator.ValidateAndThrow(product);
            ProductDto returnedProduct = new();
            string? newProdImage = null;
            if(product.Image != null)
            {
                var productBeforeChange = await unitOfWork.productRepository.GetById(Id);
                var oldImageUrl = productBeforeChange.Image;
                var oldImagePublicId = imageService.GetImagePublicId(oldImageUrl, imageService.ProductsFolderPath);
                var uploadResult = await imageService.UpdateImageAsync(product.Image, imageService.ProductsFolderPath, oldImagePublicId);
                newProdImage = uploadResult.Url.ToString();
            }

            try
            {
                await unitOfWork.productRepository.Update(Id, (ProductAfterChange) =>
                {
                   var UpdatedProduct = ProductHelper.UpdateProductDto(ref ProductAfterChange, product, newProdImage);
                   returnedProduct = mapper.Map<Product, ProductDto>(UpdatedProduct);
                });
            }
            catch(ArgumentException argEx)
            {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, argEx.Message));
            }
			
            await unitOfWork.saveAsync();
            var successResponse = new ApiResponse(HttpStatusCode.Accepted, returnedProduct);

            return Accepted(successResponse);
        }

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,SuperAdmin")]
		[HttpDelete("{Id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool isDeleted = await unitOfWork.productRepository.Delete(id);
            if (!isDeleted)
            {
                var errorMessage = "Something went wrong please try again later!";
                var badResponse = new ApiResponse(HttpStatusCode.BadRequest, errorMessage);

                return BadRequest(badResponse);
            }
            var product = await unitOfWork.productRepository.GetById(id);
            var productImagePublicId = imageService.GetImagePublicId(product.Image, imageService.ProductsFolderPath);
            var deletionResult = await imageService.DeleteImageAsync(productImagePublicId);

            if (deletionResult.Result == "error")
            {
				var errorMessage = "Something went wrong please try again later!";
				var badResponse = new ApiResponse(HttpStatusCode.InternalServerError, errorMessage);
				return StatusCode(StatusCodes.Status500InternalServerError,badResponse);
			}
            
			await unitOfWork.saveAsync();

            return NoContent();
        }

		[HttpGet("categories/{CategoryId}")]
        public async Task<ActionResult<ApiResponse>> GetProductsByCategoryId([FromRoute] int CategoryId,
            [FromQuery] int page = PaginationHelper.DefaultPage, [FromQuery] int limit = PaginationHelper.DefaultLimit)
        {
            var (validatedPage, validatedLimit) = PaginationHelper.ValidatePageAndLimit(page, limit);
			var products = await unitOfWork.productRepository.GetProductsByCategoryId(CategoryId, validatedPage, validatedLimit);
            bool isEmpty = !products.Any();
            if (isEmpty)
            {
                var emptyResult = new object[] {};
                var emptyResponse = new ApiResponse(HttpStatusCode.OK,data: emptyResult);
				return Ok(emptyResponse);
			}

            var mappedProducts = mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponseDto>>(products);
            var count = mappedProducts.Count();
            var response = new ApiResponse(HttpStatusCode.OK, new {page=validatedPage, limit=validatedLimit, count, products=mappedProducts});

            return Ok(response);
        }
	}
}
