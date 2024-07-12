using AutoMapper;
using c_shap_eCommerce.Core.DTOs;
using c_shap_eCommerce.Core.IRepositories;
using c_shap_eCommerce.Core.Models;
using c_sharp_eCommerce.Infrastructure.Data;
using c_sharp_eCommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        public async Task<ActionResult<ApiResponse>> GetAllProducts()
        {
            var products = await unitOfWork.productRepository.GetAll();
            bool isEmpty = products.Any();
            if (isEmpty)
            {
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.IsSucess = true;
                var mappedProducts = mapper.Map<IEnumerable<Product> ,IEnumerable <ProductDTO>>(products);
                response.Result = mappedProducts;
                return response;
            }
          
            response.ErrorMessages = "no product was found";
            response.IsSucess = false;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            return response; 
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> GetProductById(int Id)
        {
            var product = await unitOfWork.productRepository.GetById(Id);
            return Ok(product);
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
            return Ok(product);
        }

        [HttpPut]
        public async Task<ActionResult> Update(Product product)
        {
            await unitOfWork.productRepository.Update(product);
            // change it to var updatedProduct = unitOfWork.productRepository.Update(product);
            await unitOfWork.saveAsync();
            return Ok(product);
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

            return Ok(204);
        }

        [HttpGet("categories/{CategoryId}")]
        public async Task<ActionResult<ApiResponse>> GetProductsByCategoryId(int CategoryId)
        {
            var products = await unitOfWork.productRepository.GetProductsByCategoryId(CategoryId);
            bool isEmpty = !products.Any();
            if (isEmpty)
            {
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.IsSucess = false;
                response.ErrorMessages = "no products with this category";
            }

            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.IsSucess = true;
            var mappedProducts = mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
            response.Result = mappedProducts;

            return Ok(response);
        }


    }
}
