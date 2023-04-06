using Microsoft.AspNetCore.Hosting;

namespace EFCoreExam.Hepler.Image
{
    public static class UpLoadImages
    {
        public static readonly IWebHostEnvironment _webHostEnvironment;
        public static async Task<List<string>> UploadAlbumImagesAsync(List<IFormFile> albumImages)
        {
            var uploadedImages = new List<string>();

            foreach (var albumImage in albumImages)
            {
                if (albumImage.ContentType != "image/jpeg" && albumImage.ContentType != "image/png")
                {
                    throw new ArgumentException("Only JPG and PNG images are supported.");
                }

                var fileName = Path.GetFileNameWithoutExtension(albumImage.FileName);
                var fileExtension = Path.GetExtension(albumImage.FileName);
                fileName = fileName + "_" + DateTime.Now.Ticks + fileExtension;
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await albumImage.CopyToAsync(fileStream);
                }
                uploadedImages.Add(fileName);
            }

            return uploadedImages;
        }
    }
}
