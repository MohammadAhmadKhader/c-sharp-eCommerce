using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.IServices
{
	public interface IImageService
	{
		string ProductsFolderPath { get; }
		Task<ImageUploadResult> UploadImageAsync(IFormFile file,string folderPath);
		Task<ImageUploadResult> UpdateImageAsync(IFormFile file, string folderPath, string publicId);
		Task<DeletionResult> DeleteImageAsync(string publicId);
		string GetImagePublicId(string imageUrl, string folderPath);
	}
}
