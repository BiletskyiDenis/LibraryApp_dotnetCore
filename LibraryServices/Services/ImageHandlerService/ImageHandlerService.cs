using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LibraryServices.Models;
using Microsoft.Extensions.Options;
using SkiaSharp;
namespace LibraryServices
{
    public class ImageHandlerService : IImageHandlerService
    {
        private readonly ImageServiceOptions _options;

        public ImageHandlerService(IOptions<ImageServiceOptions> options)
        {
            this._options = options.Value;
        }

        public async Task<string> UploadImage(byte[] file, string fileName, string webRootPath)
        {
            var imageFile = Resize(file, _options.ImageSize);
            var imageSmallFile = Resize(imageFile, _options.PreviewImageSize);

            var imageFileName = fileName;

            using (var output = new FileStream(webRootPath + _options.ImagesStorePath + imageFileName, FileMode.Create, FileAccess.Write))
            {
                await output.WriteAsync(imageFile, 0, imageFile.Length);
            }

            using (var output = new FileStream(webRootPath + _options.PreviewImagesStorePath + imageFileName, FileMode.Create))
            {
                await output.WriteAsync(imageSmallFile, 0, imageSmallFile.Length);
            }

            return imageFileName;
        }

        public bool DeleteImage(string webRootPath, string imageUrl)
        {
            var pathBig = webRootPath + _options.ImagesStorePath + imageUrl;
            var pathSmall = webRootPath + _options.PreviewImagesStorePath + imageUrl;

            if (imageUrl == "none")
            {
                return false;
            }

            if (File.Exists(pathBig))
            {
                File.Delete(pathBig);
            }

            if (File.Exists(pathSmall))
            {
                File.Delete(pathSmall);
            }

            return true;
        }

        public string GenerateImageFileName(params string[] arg)
        {
            var dest = string.Empty;
            var random = new Random();

            foreach (var item in arg)
            {
                var tmpItem = Regex.Replace(item, @"[^a-zA-z0-9]+", String.Empty);
                if (tmpItem.Length > 40)
                {
                    tmpItem = tmpItem.Substring(0, 40);
                }
                dest += tmpItem;
            }
            return dest + random.Next(1000, 9999).ToString();
        }

        private byte[] Resize(byte[] sourceImage, int size)
        {
            using (var input = new MemoryStream(sourceImage))
            {
                using (var inputStream = new SKManagedStream(input))
                {
                    using (var original = SKBitmap.Decode(inputStream))
                    {
                        int width, height;

                        width = original.Width > original.Height ? size : original.Height * size / original.Width;
                        height = original.Width > original.Height ? original.Width * size / original.Height : size;

                        using (var resized = original.Resize(new SKImageInfo(width, height), SKBitmapResizeMethod.Lanczos3))
                        {
                            if (resized == null) { return null; }

                            using (var image = SKImage.FromBitmap(resized))
                            {
                                return image.Encode(SKEncodedImageFormat.Jpeg, _options.Quality).ToArray();
                            }
                        }
                    }
                }
            }
        }
    }
}
