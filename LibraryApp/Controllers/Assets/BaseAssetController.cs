using System.IO;
using System.Threading.Tasks;
using BusinessLogic.services;
using Domain.Models;
using LibraryApp.Extentions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    public class BaseAssetController<T> : Controller where T : LibraryAsset
    {
        protected readonly ILibraryDataService _libraryDataService;
        protected readonly IImageHandlerService _imageService;
        protected readonly IHostingEnvironment _appEnvironment;

        public BaseAssetController(ILibraryDataService libraryDataService, IImageHandlerService imageService, IHostingEnvironment appEnvironment)
        {
            this._libraryDataService = libraryDataService;
            this._imageService = imageService;
            this._appEnvironment = appEnvironment;
        }

        [HttpPost]
        [HttpPut]
        public virtual async Task<IActionResult> PostPut(string asset, IFormFileCollection file)
        {
            var lbAsset = asset.DeserializeAsset<T>();

            if (lbAsset.ImageUrl == string.Empty)
                lbAsset.ImageUrl = "none";

            if (file.Count != 0)
            {
                if (lbAsset.Id == 0 || lbAsset.ImageUrl != "none")
                {
                    _imageService.DeleteImage(_appEnvironment.WebRootPath, lbAsset.ImageUrl);
                }

                lbAsset.ImageUrl = _imageService.GenerateImageFileName(lbAsset.Title, lbAsset.Publisher, lbAsset.Year.ToString()) + ".jpg";

                using (var image = new MemoryStream())
                {
                    await file[0].CopyToAsync(image);
                    await _imageService.UploadImage(image.ToArray(), lbAsset.ImageUrl, _appEnvironment.WebRootPath);
                }
            }

            if (lbAsset.Id > 0)
            {
                _libraryDataService.UpdateAsset(lbAsset);
                _libraryDataService.SaveChanges();

                return Ok();
            }

            _libraryDataService.AddAsset(lbAsset);
            _libraryDataService.SaveChanges();
            return Ok();
        }
    }
}