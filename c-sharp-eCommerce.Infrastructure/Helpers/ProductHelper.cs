using c_sharp_eCommerce.Core.DTOs.Orders;
using c_sharp_eCommerce.Core.DTOs.Products;
using c_sharp_eCommerce.Core.Models;

namespace c_sharp_eCommerce.Infrastructure.Helpers
{
    public class ProductHelper
    {
        public static Product UpdateProductDto(Product UpdatedProductInfo, ProductUpdateDto product, string? productImage)
        {
            if (product.Name is not null) UpdatedProductInfo.Name = product.Name;
            if (product.Description is not null) UpdatedProductInfo.Description = product.Description;
            if (product.Price is not null) UpdatedProductInfo.Price = (double)product.Price;
            if (product.CategoryId is not null) UpdatedProductInfo.CategoryId = (int)product.CategoryId;
            if (product.Image is not null && productImage is not null)
            {
                UpdatedProductInfo.Image = productImage;
            }

            return UpdatedProductInfo;
        }

        public static Dictionary<int, int> CreateProductIdQuantityMap(OrderCreateDto payload)
        {
            var productQuantityMap = new Dictionary<int, int>();
            foreach (var item in payload.Items)
            {
                productQuantityMap[item.ProductId] = item.Quantity;

            }
            return productQuantityMap;
        }
    }
}
