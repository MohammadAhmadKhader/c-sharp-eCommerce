using AutoMapper;
using c_shap_eCommerce.Core.DTOs.Products;
using c_shap_eCommerce.Core.IRepositories;
using c_shap_eCommerce.Core.Models;
using c_sharp_eCommerce.Infrastructure.Data;
using c_sharp_eCommerce.Infrastructure.Helpers;
using c_sharp_eCommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace c_sharp_eCommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork<Product> unitOfWork;
        private ApiResponse response;
        private IMapper mapper;

        public ProductsController(IUnitOfWork<Product> UnitOfWork, IMapper mapper)
        {
            this.unitOfWork = UnitOfWork;
            this.mapper = mapper;
            response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllProducts(int Page, int Limit)
        {
            var products = await unitOfWork.productRepository.GetAll(Page, Limit, new string[] { "Category" });
            bool isEmpty = !products.Any();
            
            if(isEmpty) {
                response.Result = new object[] { };

            }else{
                var mappedProducts = mapper.Map<IEnumerable<Product> ,IEnumerable <ProductDTO>>(products);
                response.Result = mappedProducts;
            }
            
            response.IsSucess = true;
            response.StatusCode = HttpStatusCode.OK;
            return response; 
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> GetProductById(int Id)
        {
            var product = await unitOfWork.productRepository.GetById(Id);
            bool isEmpty = (product == null);
            if (isEmpty)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSucess = false;
                response.ErrorMessages = "no products with this category";
                return BadRequest(response);
            }

            response.StatusCode = HttpStatusCode.OK;
            response.IsSucess = true;
            var mappedProducts = mapper.Map<Product, ProductDTO>(product);
            response.Result = mappedProducts;
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Product product)
        {
            await unitOfWork.productRepository.Create(product);
            // must be var product = unitOfWork.productRepository.Create(product);
            if (product is null)
            {
                return BadRequest();
            }
            await unitOfWork.saveAsync();

            var mappedProduct = mapper.Map<Product, ProductDTO>(product);
            return Ok(mappedProduct);
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> Update(UpdateProductDto product, int Id)
        {
            bool IsBadRequest = false;
            ProductDTO returnedProduct = new();
            await unitOfWork.productRepository.Update(Id, (ProductAfterChange) =>
            {
                if(ProductHelpers.IsInvalid(product))
                {
                    response.IsSucess = false;
                    response.ErrorMessages = "At least one field of Name, Description, Price, CategoryId, Image is required";
                    response.StatusCode = HttpStatusCode.BadRequest;
                }

                var UpdatedProduct = ProductHelpers.UpdateProductDto(ref ProductAfterChange, product);

                returnedProduct = mapper.Map<Product, ProductDTO>(UpdatedProduct);
            });
            if (IsBadRequest) { 
                return BadRequest(response);
            }
            // change it to var updatedProduct = unitOfWork.productRepository.Update(product);
            
            await unitOfWork.saveAsync();
            
            response.IsSucess = true;
            response.Result = returnedProduct;
            response.StatusCode = HttpStatusCode.Accepted;

            return Accepted(response);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            bool isDeleted = await unitOfWork.productRepository.Delete(id);
            if (!isDeleted)
            {
                return BadRequest();
            }
            await unitOfWork.saveAsync();

            return NoContent();
        }

        [HttpGet("categories/{CategoryId}")]
        public async Task<ActionResult<ApiResponse>> GetProductsByCategoryId(int CategoryId)
        {
            var products = await unitOfWork.productRepository.GetProductsByCategoryId(CategoryId);
            bool isEmpty = !products.Any();
            if (isEmpty)
            {
                response.Result = new object[] {};
            }
            else
            {
                var mappedProducts = mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
                response.Result = mappedProducts;
            }

            response.StatusCode = HttpStatusCode.OK;
            response.IsSucess = true;
            return Ok(response);
        }
    }
}
