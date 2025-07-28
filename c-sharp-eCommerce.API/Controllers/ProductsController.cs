using AutoMapper;
using c_sharp_eCommerce.Core.DTOs.Products;
using c_sharp_eCommerce.Core.IRepositories;
using c_sharp_eCommerce.Core.IServices;
using c_sharp_eCommerce.Core.Models;
using c_sharp_eCommerce.Core.DTOs.ApiResponse;
using c_sharp_eCommerce.Infrastructure.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace c_sharp_eCommerce.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork<Product> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IValidator<ProductCreateDto> _productCreateValidator;
        private readonly IValidator<ProductUpdateDto> _productUpdateValidator;

        public ProductsController(IUnitOfWork<Product> unitOfWork, IMapper mapper, IImageService imageService, IValidator<ProductCreateDto> productCreateValidator, IValidator<ProductUpdateDto> productUpdateValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
            _productCreateValidator = productCreateValidator;
            _productUpdateValidator = productUpdateValidator;

        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllProducts(
            [FromQuery] int page = PaginationHelper.DefaultPage,
            [FromQuery] int limit = PaginationHelper.DefaultLimit)
        {
            var (validatedPage, validatedLimit) = PaginationHelper.ValidatePageAndLimit(page, limit);
            var (products, count) = await _unitOfWork.ProductRepository.GetAll(validatedPage, validatedLimit, new string[] { "Category" });

            var mappedProducts = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);

            var response = new ApiResponse(
                HttpStatusCode.OK,
                new { page = validatedPage, limit = validatedLimit, count, products = mappedProducts }
            );

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProductById(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductById(id);
            bool isEmpty = (product == null);
            if (isEmpty)
            {
                string message = $"no products with {id} id";
                var NotFoundResult = new ApiResponse(HttpStatusCode.NotFound, message);

                return NotFound(NotFoundResult);
            }

            var mappedProduct = _mapper.Map<Product, ProductDto>(product!);
            var result = new Dictionary<string, object>() { { "product", mappedProduct } };
            var response = new ApiResponse(HttpStatusCode.OK, result);

            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create([FromForm] ProductCreateDto dto)
        {
            _productCreateValidator.ValidateAndThrow(dto);
            var category = await _unitOfWork.CategoryRepository.GetById(dto.CategoryId!.Value);
            if (category is null)
            {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, $"category with id: '{dto.CategoryId}' wasn't found"));
            }

            var updatedResult = await _imageService.UploadImageAsync(dto.Image, _imageService.ProductsFolderPath);
            var url = updatedResult.Url.ToString();

            var product = _mapper.Map<Product>(dto);
            product.Image = url;

            await _unitOfWork.ProductRepository.Create(product);
            await _unitOfWork.SaveAsync();

            var mappedProduct = _mapper.Map<Product, ProductDto>(product);
            var response = new ApiResponse(HttpStatusCode.Created, mappedProduct);

            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,SuperAdmin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> Update([FromForm] ProductUpdateDto dto, int id)
        {
            _productUpdateValidator.ValidateAndThrow(dto);
            var product = await _unitOfWork.ProductRepository.GetById(id);
            if (product is null)
            {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, $"product with id: '{id}' was not found"));
            }

            string? newProdImage = null;
            if (dto.Image != null)
            {
                var oldImageUrl = product.Image;
                var oldImagePublicId = _imageService.GetImagePublicId(oldImageUrl, _imageService.ProductsFolderPath);
                var uploadResult = await _imageService.UpdateImageAsync(dto.Image, _imageService.ProductsFolderPath, oldImagePublicId);
                newProdImage = uploadResult.Url.ToString();
            }

            var updatedProduct = ProductHelper.UpdateProductDto(product, dto, newProdImage);
            var returnedProduct = _mapper.Map<Product, ProductDto>(updatedProduct);

            await _unitOfWork.SaveAsync();
            var successResponse = new ApiResponse(HttpStatusCode.Accepted, returnedProduct);

            return Accepted(successResponse);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool isDeleted = await _unitOfWork.ProductRepository.Delete(id);
            if (!isDeleted)
            {
                var errorMessage = "Something went wrong please try again later!";
                var badResponse = new ApiResponse(HttpStatusCode.BadRequest, errorMessage);

                return BadRequest(badResponse);
            }

            var product = await _unitOfWork.ProductRepository.GetById(id);
            var productImagePublicId = _imageService.GetImagePublicId(product!.Image, _imageService.ProductsFolderPath);
            var deletionResult = await _imageService.DeleteImageAsync(productImagePublicId);

            if (deletionResult.Result == "error")
            {
                var errorMessage = "Something went wrong please try again later!";
                var badResponse = new ApiResponse(HttpStatusCode.InternalServerError, errorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, badResponse);
            }

            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpGet("categories/{CategoryId}")]
        public async Task<ActionResult<ApiResponse>> GetProductsByCategoryId([FromRoute] int CategoryId,
            [FromQuery] int page = PaginationHelper.DefaultPage, [FromQuery] int limit = PaginationHelper.DefaultLimit)
        {
            var (validatedPage, validatedLimit) = PaginationHelper.ValidatePageAndLimit(page, limit);
            var products = await _unitOfWork.ProductRepository.GetProductsByCategoryId(CategoryId, validatedPage, validatedLimit);
            bool isEmpty = !products.Any();
            if (isEmpty)
            {
                var emptyResult = new object[] { };
                var emptyResponse = new ApiResponse(HttpStatusCode.OK, data: emptyResult);
                return Ok(emptyResponse);
            }

            var mappedProducts = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponseDto>>(products);
            var count = mappedProducts.Count();
            var response = new ApiResponse(HttpStatusCode.OK, new { page = validatedPage, limit = validatedLimit, count, products = mappedProducts });

            return Ok(response);
        }
    }
}
