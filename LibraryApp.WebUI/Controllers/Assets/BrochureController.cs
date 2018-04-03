using BusinessLogic.services;
using DataAccess.Mappers;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ViewModel;

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
            return Json(_libraryDataService.GetAssetsFromType(AssetType.Brochure).DtoMap<BrochureViewModel>());
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

            return Json(asset.DtoMap<BrochureViewModel>());
        }
    }
}