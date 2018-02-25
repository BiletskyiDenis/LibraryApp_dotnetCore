using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryServices.Models
{
    public class ImageServiceOptions
    {
        public int Quality { get; set; }
        public int ImageSize { get; set; }
        public int PreviewImageSize { get; set; }

        public string ImagesStorePath { get; set; }
        public string PreviewImagesStorePath { get; set; }
    }
}