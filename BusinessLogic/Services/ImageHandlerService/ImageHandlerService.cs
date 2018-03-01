using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SkiaSharp;
namespace BusinessLogic.services
{
    public class ImageHandlerService : IImageHandlerService
    {
        private readonly ImageServiceOptions _imageServiceOptions;

        public ImageHandlerService(IOptions<ImageServiceOptions> options)
        {
            this._imageServiceOptions = options.Value;
        }

        public async Task<string> UploadImage(byte[] file, string fileName, string webRootPath)
        {
            var imageFile = new byte[0];
            var previewImageFile = new byte[0];

            try
            {
                imageFile = Resize(file, _imageServiceOptions.ImageSize);
                previewImageFile = Resize(imageFile, _imageServiceOptions.PreviewImageSize);
            }
            catch (Exception)
            {
                return "none";
            }

            var imageFileName = fileName;

            using (var output = new FileStream(webRootPath + _imageServiceOptions.ImagesStorePath + imageFileName, FileMode.Create, FileAccess.Write))
            {
                await output.WriteAsync(imageFile, 0, imageFile.Length);
            }

            using (var output = new FileStream(webRootPath + _imageServiceOptions.PreviewImagesStorePath + imageFileName, FileMode.Create))
            {
                await output.WriteAsync(previewImageFile, 0, previewImageFile.Length);
            }

            return imageFileName;
        }

        public bool DeleteImage(string webRootPath, string imageUrl)
        {
            var imageStorePath = webRootPath + _imageServiceOptions.ImagesStorePath + imageUrl;
            var previewImageStorePath = webRootPath + _imageServiceOptions.PreviewImagesStorePath + imageUrl;

            if (imageUrl == "none")
            {
                return false;
            }

            if (File.Exists(imageStorePath))
            {
                File.Delete(imageStorePath);
            }

            if (File.Exists(previewImageStorePath))
            {
                File.Delete(previewImageStorePath);
            }

            return true;
        }

        public string GenerateImageFileName(params string[] piecesOfFileName)
        {
            var fileName = string.Empty;
            var random = new Random();

            foreach (var piece in piecesOfFileName)
            {
                var pieceOfFileName = Regex.Replace(piece, @"[^a-zA-z0-9]+", String.Empty);
                var maxLength = _imageServiceOptions.FileNameMaxLength;

                if (pieceOfFileName.Length > maxLength)
                {
                    pieceOfFileName = pieceOfFileName.Substring(0, maxLength);
                }

                fileName += pieceOfFileName;
            }
            var fullFileName = fileName + random.Next(1000, 9999).ToString();

            return fullFileName;
        }

        private byte[] Resize(byte[] sourceImage, int size)
        {
            using (var input = new MemoryStream(sourceImage))
            using (var inputStream = new SKManagedStream(input))
            using (var original = SKBitmap.Decode(inputStream))
            {
                int width, height;

                width = original.Width > original.Height ? size : original.Height * size / original.Width;
                height = original.Width > original.Height ? original.Width * size / original.Height : size;

                using (var resized = original.Resize(new SKImageInfo(width, height), SKBitmapResizeMethod.Lanczos3))
                using (var image = SKImage.FromBitmap(resized))
                {

                    return image.Encode(SKEncodedImageFormat.Jpeg, _imageServiceOptions.Quality).ToArray();
                }
            }
        }
    }
}
