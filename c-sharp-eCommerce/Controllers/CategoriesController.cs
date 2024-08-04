using AutoMapper;
using c_shap_eCommerce.Core.DTOs.Categories;
using c_shap_eCommerce.Core.IRepositories;
using c_shap_eCommerce.Core.Models;
using c_sharp_eCommerce.Infrastructure.Helpers;
using c_sharp_eCommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace c_sharp_eCommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork<Category> unitOfWork;
        private readonly IMapper mapper;

        public CategoriesController(IUnitOfWork<Category> UnitOfWork, IMapper mapper)
        {
            this.unitOfWork = UnitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCategories([FromQuery] int Page = PaginationHelper.DefaultPage,[FromQuery] int Limit = PaginationHelper.DefaultLimit)
        {
			var validatedPage = PaginationHelper.ValidatePage(Page);
			var validatedLimit = PaginationHelper.ValidateLimit(Limit);
			var categories = await unitOfWork.categoryRepository.GetAll(validatedPage, validatedLimit);
            bool isEmpty = !categories.Any();
			var result = new Dictionary<string, object>
			{
				{ "page", validatedPage },
				{ "limit", validatedLimit }
			};

			if (isEmpty){
                result.Add("categories", new object[] {});
				result.Add("count", 0);

			}else{
                var mappedCategories = mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(categories);
                result.Add("categories", mappedCategories);
				result.Add("count", categories.Count());
			}
            
            var response = new ApiResponse(HttpStatusCode.OK, result);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Category category)
        {
            await unitOfWork.categoryRepository.Create(category);
            if (category is null)
            {
                return BadRequest();
            }

            await unitOfWork.saveAsync();
            return Ok(category);
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> Update([FromBody] Category category, int Id)
        {
            await unitOfWork.categoryRepository.Update(Id, (UpdatedCategory) =>
            {
                UpdatedCategory.Name = category.Name;
                UpdatedCategory.Description = category.Description;
            });
            if (category is null)
            {
                return BadRequest();
            }
            await unitOfWork.saveAsync();
            return Ok(category);
        }
    }
}
