using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryData;
using LibraryServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Delete")]
    public class RemoveController : Controller
    {
        protected readonly ILibraryDataService _libraryDataService;
        protected readonly IImageHandlerService _imageService;
        protected readonly IHostingEnvironment _appEnvironment;

        public RemoveController(ILibraryDataService libraryDataService, IImageHandlerService imageService, IHostingEnvironment appEnvironment)
        {
            this._libraryDataService = libraryDataService;
            this._imageService = imageService;
            this._appEnvironment = appEnvironment;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var asset = _libraryDataService.GetAsset(id);
            if (asset == null)
            {
                return NotFound();
            }

            if (asset.ImageUrl != "none")
            {
                _imageService.DeleteImage(_appEnvironment.WebRootPath, asset.ImageUrl);
            }

            _libraryDataService.RemoveAsset(asset);
            _libraryDataService.SaveChanges();
            return Ok();
        }
    }
}