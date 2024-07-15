using AutoMapper;
using c_shap_eCommerce.Core.DTOs.Categories;
using c_shap_eCommerce.Core.IRepositories;
using c_shap_eCommerce.Core.Models;
using c_sharp_eCommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace c_sharp_eCommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork<Category> unitOfWork;
        private ApiResponse response;
        private IMapper mapper;

        public CategoriesController(IUnitOfWork<Category> UnitOfWork, IMapper mapper)
        {
            this.unitOfWork = UnitOfWork;
            this.mapper = mapper;
            response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCategories(int Page, int Limit)
        {
            var Categories = await unitOfWork.categoryRepository.GetAll(Page, Limit, null);
            bool isEmpty = !Categories.Any();
            var result = new Dictionary<string, object>();
            result.Add("page", Page);
            result.Add("limit", Limit);

            if (isEmpty)
            {
                result.Add("Categories", new object[] {});
            }
            else
            {
                var mappedCategories = mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(Categories);
                result.Add("Categories", mappedCategories);
            }
            response.Result = result;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.IsSucess = true;
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Category category)
        {
            await unitOfWork.categoryRepository.Create(category);
            if (category is null)
            {
                return BadRequest();
            }
            await unitOfWork.saveAsync();
            return Ok(category);
        }

        [HttpPut]
        public async Task<ActionResult> Update(Category category, int Id)
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
