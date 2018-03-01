using System.IO;
using System.Threading.Tasks;

namespace BusinessLogic.services
{
    public interface IImageHandlerService
    {
        bool DeleteImage(string webRootPath, string ImageUrl);
        Task<string> UploadImage(byte[] file, string fileName, string webRootPath);
        string GenerateImageFileName(params string[] arg);
    }
}