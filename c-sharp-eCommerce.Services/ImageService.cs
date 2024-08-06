using c_shap_eCommerce.Core.IServices;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace c_sharp_eCommerce.Services
{
    public class ImageService : IImageService
    {
        public string ProductsFolderPath { get; } = "c-sharp-ecommerce/products";
        private string cloudName = "doxhxgz2g";
        private IConfiguration configuration;
        private readonly Cloudinary cloudinary;
        public ImageService(IConfiguration configuration)
        {
            this.configuration = configuration;
            var apiKey = configuration.GetSection("CloudinarySettings")["ApiKey"];
            var apiSecret = configuration.GetSection("CloudinarySettings")["ApiSecret"];
            
            var account = new Account(cloudName, apiKey, apiSecret);
            cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;
        }
        public async Task<ImageUploadResult> UploadImageAsync(IFormFile file, string folderPath)
        {
            try
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.Name, file.OpenReadStream()),
                    Folder = folderPath,
                };

                var uploadResult = await cloudinary.UploadAsync(uploadParams);
                return uploadResult;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ImageUploadResult();
            }

        }
        public async Task<ImageUploadResult> UpdateImageAsync(IFormFile file, string publicId, string folderPath)
        {
			var uploadResult = new ImageUploadResult();
			try
            {
                using(var stream = file.OpenReadStream())
                { 
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Name, stream),
                        Folder = folderPath,
                        PublicId= publicId,
	                    Overwrite = true,
                    };

                    uploadResult = await cloudinary.UploadAsync(uploadParams);
                    return uploadResult;
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return uploadResult;
			}
            
        }

        public async Task<DeletionResult> DeleteImageAsync(string publicId)
        {
            try
            {
                var deleteParams = new DeletionParams(publicId);
                var destoryResult = await cloudinary.DestroyAsync(deleteParams);
                return destoryResult;
            }
            catch(Exception ex)
            {
				Console.WriteLine(ex.Message);
                return new DeletionResult
                {
                    Result = "error",
                    Error = new Error
                    {
                        Message = ex.Message,
                    }
                };
			}
            
        }
        // publicId for delete not like for update, for update wouldnt require u folder path, for delete it does.
        public string GetImagePublicId(string imageUrl, string folderPath)
        {
            var splitByDot = imageUrl.Split("/");
            var imageNameWithExt = splitByDot[splitByDot.Length - 1];
            var publicId = imageNameWithExt.Split(".")[0];
            return publicId;
        }
    }
}
