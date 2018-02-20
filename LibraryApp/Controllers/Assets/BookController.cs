using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LibraryApp.Extentions;
using LibraryApp.Models;
using LibraryData;
using LibraryData.Models;
using LibraryServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace LibraryApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Book")]
    public class BookController : BaseAssetController<Book>
    {
        public BookController(ILibraryDataService assets, IImageHandlerService imageService, IHostingEnvironment appEnvironment)
            : base(assets, imageService, appEnvironment)
        {
        }

        // GET: api/Book
        [HttpGet]
        public IActionResult Get()
        {
            return Json(_assets.GetBooks().Dto<DtoBook>());
        }

        // GET: api/Book/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var asset = _assets.GetById(id);
            if (!(asset is Book))
            {
                return NotFound();
            }

            return Json(asset.Dto<DtoBook>());
        }
    }
}