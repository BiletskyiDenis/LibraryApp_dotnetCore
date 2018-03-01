using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Domain.Models;
using BusinessLogic.services;
using Domain.Enums;
using ViewModel;
using DataAccess.Mappers;

namespace LibraryApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Book")]
    public class BookController : BaseAssetController<Book>
    {
        public BookController(ILibraryDataService libraryDataService, IImageHandlerService imageService, IHostingEnvironment appEnvironment)
            : base(libraryDataService, imageService, appEnvironment)
        {
        }

        // GET: api/Book
        [HttpGet]
        public IActionResult Get()
        {
            return Json(_libraryDataService.GetAssetsFromType(AssetType.Book).DtoMap<BookViewModel>());
        }

        // GET: api/Book/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var asset = _libraryDataService.GetAsset(id);
            if (!(asset is Book))
            {
                return NotFound();
            }

            return Json(asset.DtoMap<BookViewModel>());
        }
    }
}