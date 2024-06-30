using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace c_sharp_eCommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetAllProducts()
        {
            var product = new
            {
                Id = 2,
                Name = "Product name",
                Description = "Description",
                Image = "Some Url"
            };
            return Ok(product);
        }
    }
}
