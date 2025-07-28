using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace c_sharp_eCommerce.Core.IServices
{
	public interface IImageService
	{
		string ProductsFolderPath { get; }
		Task<ImageUploadResult> UploadImageAsync(IFormFile file, string folderPath);
		Task<ImageUploadResult> UpdateImageAsync(IFormFile file, string folderPath, string publicId);
		Task<DeletionResult> DeleteImageAsync(string publicId);
		string GetImagePublicId(string imageUrl, string folderPath);
	}
}
