using c_shap_eCommerce.Core.DTOs.Products;
using c_shap_eCommerce.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_eCommerce.Infrastructure.Helpers
{
    public class ProductHelper
    {
        public static Product UpdateProductDto(ref Product UpdatedProductInfo, ProductUpdateDto product)
        {
            if (product.Name is not null) UpdatedProductInfo.Name = product.Name;
            if (product.Description is not null) UpdatedProductInfo.Description = product.Description;
            if (product.Price is not null) UpdatedProductInfo.Price = (double)product.Price;
            if (product.CategoryId is not null) UpdatedProductInfo.CategoryId = (int)product.CategoryId;
            if (product.Image is not null) UpdatedProductInfo.Image = product.Image;

            return UpdatedProductInfo;
        }
        public static bool IsInvalidUpdateDto(ProductUpdateDto product)
        {
            if (product.Name is null && product.Description is null && product.Price is null && product.CategoryId is null && product.Image is null)
            {
                return true;
            }

            return false;
        }
        
    }
}
