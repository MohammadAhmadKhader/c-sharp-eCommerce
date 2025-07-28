using AutoMapper;
using c_sharp_eCommerce.Core.DTOs.ApiResponse;
using c_sharp_eCommerce.Core.DTOs.Categories;
using c_sharp_eCommerce.Core.IRepositories;
using c_sharp_eCommerce.Core.Models;
using c_sharp_eCommerce.Infrastructure.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace c_sharp_eCommerce.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork<Category> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryCreateDto> _categoryCreateValidator;
        private readonly IValidator<CategoryUpdateDto> _categoryUpdateValidator;

        public CategoriesController(IUnitOfWork<Category> unitOfWork, IMapper mapper,
            IValidator<CategoryCreateDto> categoryCreateValidator, IValidator<CategoryUpdateDto> categoryUpdateValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _categoryUpdateValidator = categoryUpdateValidator;
            _categoryCreateValidator = categoryCreateValidator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCategories(
            [FromQuery] int page = PaginationHelper.DefaultPage,
            [FromQuery] int limit = PaginationHelper.DefaultLimit)
        {
            var (validatedPage, validatedLimit) = PaginationHelper.ValidatePageAndLimit(page, limit);
            var (categories, count) = await _unitOfWork.CategoryRepository.GetAll(validatedPage, validatedLimit);

            var mappedCategories = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(categories);

            var data = new { page = validatedPage, limit = validatedLimit, count, categories = mappedCategories };
            var response = new ApiResponse(HttpStatusCode.OK, data);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CategoryCreateDto dto)
        {
            _categoryCreateValidator.ValidateAndThrow(dto);
            var mappedCategory = _mapper.Map<Category>(dto);

            await _unitOfWork.CategoryRepository.Create(mappedCategory);
            await _unitOfWork.SaveAsync();

            var responseCategory = _mapper.Map<CategoryDto>(mappedCategory);
            var message = "success";
            var response = new ApiResponse(HttpStatusCode.Created, message, new { category = responseCategory });

            return StatusCode(StatusCodes.Status201Created, response);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,SuperAdmin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromBody] CategoryUpdateDto dto, int id)
        {
            _categoryUpdateValidator.ValidateAndThrow(dto);
            await _unitOfWork.CategoryRepository.Update(id, (UpdatedCategory) =>
            {
                UpdatedCategory.Name = dto.Name!;
                UpdatedCategory.Description = dto.Description!;
            });

            await _unitOfWork.SaveAsync();
            var mappedCategory = _mapper.Map<CategoryDto>(dto);
            mappedCategory.Id = id;
            var message = "success";
            var response = new ApiResponse(HttpStatusCode.Created, message, new { category = mappedCategory });

            return Accepted(response);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool IsFoundAndDeleted = await _unitOfWork.CategoryRepository.Delete(id);
            if (!IsFoundAndDeleted)
            {
                var errMsg = $"category with Id: {id} was not found";
                var errResponse = new ApiResponse(HttpStatusCode.BadRequest, errMsg);
                return BadRequest(errResponse);
            }
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
