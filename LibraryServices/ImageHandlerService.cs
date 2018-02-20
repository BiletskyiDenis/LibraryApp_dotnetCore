using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SkiaSharp;
namespace LibraryServices
{
    public class ImageHandlerService : IImageHandlerService
    {
        const int quality = 80;
        const int size = 300;
        const int size_small = 50;

        private readonly string imageStorePath = "/img/assets/";
        private readonly string imageSmallStorePath = "/img/assets_small/";

        public async Task<string> UploadImage(byte[] file, string fileName, string webRootPath)
        {
            var imageFile = Resize(file, size);
            var imageSmallFile = Resize(imageFile, size_small);

            var imageFileName = fileName;

            using (var output = new FileStream(webRootPath + imageStorePath + imageFileName, FileMode.Create, FileAccess.Write))
            {
                await output.WriteAsync(imageFile, 0, imageFile.Length);
            }

            using (var output = new FileStream(webRootPath + imageSmallStorePath + imageFileName, FileMode.Create))
            {
                await output.WriteAsync(imageSmallFile, 0, imageSmallFile.Length);
            }

            return imageFileName;
        }

        public bool DeleteImage(string webRootPath, string ImageUrl)
        {

            var pathBig = webRootPath + imageStorePath + ImageUrl;
            var pathSmall = webRootPath + imageSmallStorePath + ImageUrl;
            if (ImageUrl == "none")
                return false;

            if (File.Exists(pathBig))
                File.Delete(pathBig);

            if (File.Exists(pathSmall))
                File.Delete(pathSmall);

            return true;
        }

        public string GenerateImageFileName(params string[] arg)
        {
            var dest = string.Empty;
            var rn = new Random();

            foreach (var item in arg)
            {
                var tmpItem = Regex.Replace(item, @"[^a-zA-z0-9]+", String.Empty);
                if (tmpItem.Length > 40)
                {
                    tmpItem = tmpItem.Substring(0, 40);
                }
                dest += tmpItem;
            }
            return dest + rn.Next(1000, 9999).ToString();
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
                        if (original.Width > original.Height)
                        {
                            width = size;
                            height = original.Height * size / original.Width;
                        }
                        else
                        {
                            width = original.Width * size / original.Height;
                            height = size;
                        }

                        using (var resized = original
                               .Resize(new SKImageInfo(width, height), SKBitmapResizeMethod.Lanczos3))
                        {
                            if (resized == null) return null;

                            using (var image = SKImage.FromBitmap(resized))
                            {
                                using (var output = new MemoryStream())
                                {
                                    image.Encode(SKEncodedImageFormat.Jpeg, quality)
                                        .SaveTo(output);
                                    return output.ToArray();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
