using AutoMapper;
using c_shap_eCommerce.Core.DTOs.Categories;
using c_shap_eCommerce.Core.DTOs.Users;
using c_shap_eCommerce.Core.IRepositories;
using c_shap_eCommerce.Core.Models;
using c_sharp_eCommerce.Infrastructure.Helpers;
using c_sharp_eCommerce.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace c_sharp_eCommerce.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork<Category> unitOfWork;
        private readonly IMapper mapper;
		private readonly IValidator<CategoryCreateDto> categoryCreateValidator;
		private readonly IValidator<CategoryUpdateDto> categoryUpdateValidator;

		public CategoriesController(IUnitOfWork<Category> UnitOfWork, IMapper mapper,
            IValidator<CategoryCreateDto> categoryCreateValidator,IValidator<CategoryUpdateDto> categoryUpdateValidator)
        {
            this.unitOfWork = UnitOfWork;
            this.mapper = mapper;
			this.categoryUpdateValidator = categoryUpdateValidator;
            this.categoryCreateValidator = categoryCreateValidator;
		}

        [HttpGet]
        public async Task<ActionResult> GetAllCategories([FromQuery] int page = PaginationHelper.DefaultPage
            ,[FromQuery] int limit = PaginationHelper.DefaultLimit)
        {
            var (validatedPage,validatedLimit) = PaginationHelper.ValidatePageAndLimit(page, limit);
			var (categories, count) = await unitOfWork.categoryRepository.GetAll(validatedPage, validatedLimit);
			
            var mappedCategories = mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(categories);

            var data = new { page= validatedPage, limit= validatedLimit, count, categories = mappedCategories};
            var response = new ApiResponse(HttpStatusCode.OK, data);
            return Ok(response);
        }

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,SuperAdmin")]
		[HttpPost]
        public async Task<ActionResult> Create([FromBody] CategoryCreateDto category)
        {
            categoryCreateValidator.ValidateAndThrow(category);
            var mappedCategory = mapper.Map<Category>(category);
            
            await unitOfWork.categoryRepository.Create(mappedCategory);
            await unitOfWork.saveAsync();

            var responseCategory = mapper.Map<CategoryDto>(mappedCategory);
            var message = "success";
            var response = new ApiResponse(HttpStatusCode.Created, message, new { category= responseCategory });

            return StatusCode(StatusCodes.Status201Created, response);
        }

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,SuperAdmin")]
		[HttpPut("{Id}")]
        public async Task<ActionResult> Update([FromBody] CategoryUpdateDto category, int Id)
        {
			categoryUpdateValidator.ValidateAndThrow(category);
			await unitOfWork.categoryRepository.Update(Id, (UpdatedCategory) =>
            {
                UpdatedCategory.Name = category.Name!;
                UpdatedCategory.Description = category.Description!;
            });

            await unitOfWork.saveAsync();
			var mappedCategory = mapper.Map<CategoryDto>(category);
            mappedCategory.Id = Id;
			var message = "success";
			var response = new ApiResponse(HttpStatusCode.Created, message, new { category = mappedCategory });

			return Accepted(response);
        }

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,SuperAdmin")]
		[HttpDelete("{Id}")]
		public async Task<ActionResult> Delete(int Id)
		{
            bool IsFoundAndDeleted = await unitOfWork.categoryRepository.Delete(Id);
            if (!IsFoundAndDeleted)
            {
                var errMsg = $"category with Id: {Id} was not found";
                var errResponse = new ApiResponse(HttpStatusCode.BadRequest, errMsg);
                return BadRequest(errResponse);
            }
			await unitOfWork.saveAsync();

			return NoContent();
		}
	}
}
