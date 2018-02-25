using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApp.Extentions;
using LibraryApp.Models;
using LibraryData;
using LibraryData.Models;
using LibraryServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Brochure")]
    public class BrochureController : BaseAssetController<Brochure>
    {
        public BrochureController(ILibraryDataService libraryDataService, IImageHandlerService imageService, IHostingEnvironment appEnvironment) 
            : base(libraryDataService, imageService, appEnvironment)
        {
        }

        // GET: api/Brochure
        [HttpGet]
        public IActionResult Get()
        {
            return Json(_libraryDataService.GetAssetsFromType(AssetType.Brochure).Dto<BrochureViewModel>());
        }

        // GET: api/Brochure/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var asset = _libraryDataService.GetAsset(id);
            if (!(asset is Brochure))
            {
                return NotFound();
            }

            return Json(asset.Dto<BrochureViewModel>());
        }
    }
}