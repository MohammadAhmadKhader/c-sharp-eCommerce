using AutoMapper;
using c_shap_eCommerce.Core.DTOs.Products;
using c_shap_eCommerce.Core.IRepositories;
using c_shap_eCommerce.Core.Models;
using c_sharp_eCommerce.Infrastructure.Data;
using c_sharp_eCommerce.Infrastructure.Helpers;
using c_sharp_eCommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace c_sharp_eCommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork<Product> unitOfWork;
        private readonly IMapper mapper;

        public ProductsController(IUnitOfWork<Product> UnitOfWork, IMapper mapper)
        {
            this.unitOfWork = UnitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllProducts([FromQuery] int Page = PaginationHelper.DefaultPage, [FromQuery] int Limit = PaginationHelper.DefaultLimit)
        {
            var validatedPage = PaginationHelper.ValidatePage(Page);
            var validatedLimit = PaginationHelper.ValidateLimit(Limit);
            var products = await unitOfWork.productRepository.GetAll(validatedPage, validatedLimit, new string[] { "Category" });
            bool isEmpty = !products.Any();
			var result = new Dictionary<string, object>
			{
		    	{ "page", validatedPage },
		    	{ "limit", validatedLimit }
		    };

            var mappedProducts = mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);

			if (isEmpty) {
				result.Add("products",Array.Empty<Product>());
                result.Add("count", 0);
            }else{
                result.Add("products", mappedProducts);
                result.Add("count", products.Count());
			}
            
            var response = new ApiResponse(HttpStatusCode.OK, result);

			return Ok(response); 
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> GetProductById(int Id)
        {
            var product = await unitOfWork.productRepository.GetProductById(Id);
            bool isEmpty = (product == null);
            if (isEmpty)
            {
                string message = "no products with this category";
                var NotFoundResult = new ApiResponse(HttpStatusCode.NotFound, message);

                return NotFound(NotFoundResult);
            }
            
            var mappedProduct = mapper.Map<Product, ProductDto>(product!);
            var result = new Dictionary<string, object>() { { "product", mappedProduct } };
            var response = new ApiResponse(HttpStatusCode.OK, result);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create([FromBody] ProductCreateDto productDto)
        {
            var product = mapper.Map<Product>(productDto);

            await unitOfWork.productRepository.Create(product);
            await unitOfWork.saveAsync();

            var mappedProduct = mapper.Map<Product, ProductDto>(product);
            var response = new ApiResponse(HttpStatusCode.Created, mappedProduct);

            return Ok(response);
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult<ApiResponse>> Update([FromBody] ProductUpdateDto product, int Id)
        {
            bool IsBadRequest = false;
            ProductDto returnedProduct = new();
            await unitOfWork.productRepository.Update(Id, (ProductAfterChange) =>
            {
                if(ProductHelper.IsInvalidUpdateDto(product))
                {
                    IsBadRequest = true;
                }else{
                    var UpdatedProduct = ProductHelper.UpdateProductDto(ref ProductAfterChange, product);
                    returnedProduct = mapper.Map<Product, ProductDto>(UpdatedProduct);
                }
            });
            if (IsBadRequest) { 
                string message = "At least one field of Name, Description, Price, CategoryId, Image is required";
                var errorResponse = new ApiResponse(HttpStatusCode.BadRequest, message);
				
				return BadRequest(errorResponse);
            }
            
            await unitOfWork.saveAsync();
            var successResponse = new ApiResponse(HttpStatusCode.Accepted, returnedProduct);

            return Accepted(successResponse);
        }

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
            await unitOfWork.saveAsync();

            return NoContent();
        }

        [HttpGet("categories/{CategoryId}")]
        public async Task<ActionResult<ApiResponse>> GetProductsByCategoryId([FromRoute] int CategoryId)
        {
            var products = await unitOfWork.productRepository.GetProductsByCategoryId(CategoryId);
            bool isEmpty = !products.Any();
            if (isEmpty)
            {
                var emptyResult = new object[] {};
                var emptyResponse = new ApiResponse(HttpStatusCode.OK,data: emptyResult);
				return Ok(emptyResponse);
			}

            var mappedProducts = mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            var response = new ApiResponse(HttpStatusCode.OK, mappedProducts);

            return Ok(response);
        }
    }
}
