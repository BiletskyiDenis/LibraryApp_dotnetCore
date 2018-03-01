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
    [Route("api/Journal")]
    public class JournalController : BaseAssetController<Journal>
    {
        public JournalController(ILibraryDataService libraryDataService, IImageHandlerService imageService, IHostingEnvironment appEnvironment) 
            : base(libraryDataService, imageService, appEnvironment)
        {
        }

        // GET: api/Journal
        [HttpGet]
        public IActionResult Get()
        {
            return Json(_libraryDataService.GetAssetsFromType(AssetType.Journal).DtoMap<JournalViewModel>());
        }

        // GET: api/Journal/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var asset = _libraryDataService.GetAsset(id);
            if (!(asset is Journal))
            {
                return NotFound();
            }

            return Json(asset.DtoMap<JournalViewModel>());
        }
    }
}