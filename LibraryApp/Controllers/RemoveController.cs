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
        protected readonly ILibraryDataService _assets;
        protected readonly IImageHandlerService _imageService;
        protected readonly IHostingEnvironment _appEnvironment;

        public RemoveController(ILibraryDataService assets, IImageHandlerService imageService, IHostingEnvironment appEnvironment)
        {
            this._assets = assets;
            this._imageService = imageService;
            this._appEnvironment = appEnvironment;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var asset = _assets.GetById(id);
            if (asset == null)
            {
                return NotFound();
            }
            if (!_assets.DeleteAsset(asset))
            {
                return NotFound();
            }

            if (asset.ImageUrl != "none")
            {
                _imageService.DeleteImage(_appEnvironment.WebRootPath, asset.ImageUrl);
            }

            _assets.Save();

            return Ok();
        }
    }
}