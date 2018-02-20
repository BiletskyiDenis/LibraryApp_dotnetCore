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
    [Route("api/Journal")]
    public class JournalController : BaseAssetController<Journal>
    {
        public JournalController(ILibraryDataService assets, IImageHandlerService imageService, IHostingEnvironment appEnvironment) 
            : base(assets, imageService, appEnvironment)
        {
        }

        // GET: api/Journal
        [HttpGet]
        public IActionResult Get()
        {
            return Json(_assets.GetBooks().Dto<DtoJournal>());
        }

        // GET: api/Journal/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var asset = _assets.GetById(id);
            if (!(asset is Journal))
            {
                return NotFound();
            }

            return Json(asset.Dto<DtoJournal>());
        }
    }
}