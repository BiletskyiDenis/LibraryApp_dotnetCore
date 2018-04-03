using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.services
{
    public class ImageServiceOptions
    {
        public int Quality { get; set; }
        public int ImageSize { get; set; }
        public int PreviewImageSize { get; set; }
        public string ImagesStorePath { get; set; }
        public string PreviewImagesStorePath { get; set; }
        /// <summary>
        /// Maximum length of a filename for uploaded images
        /// </summary>
        public int FileNameMaxLength { get; set; }
    }
}