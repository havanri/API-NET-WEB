using System.Text;

namespace EFCoreExam.Hepler.Image
{
    public static class FileHelper
    {
        public static string ConvertIFormFileToString(IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8);
            return reader.ReadToEnd();
        }
    }
}
