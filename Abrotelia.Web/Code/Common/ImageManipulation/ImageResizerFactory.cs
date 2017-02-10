using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.Common.ImageManipulation
{
    public static class ImageResizerFactory
    {

        #region Static methods

        public static MemoryStream CreateImage(byte[] imageContent, string extension)
        {
            MemoryStream ms = null;
            IImageResizer imageResizer = null;
            if ((".jpg" == extension) || (".jpeg" == extension))
            {
                imageResizer = new JpegImageResizer();
            }
            ms = imageResizer?.CreateImage(imageContent);
            return ms;
        }

        public static MemoryStream CreateThumbnail(byte[] imageContent, string extension)
        {
            MemoryStream ms = null;
            IImageResizer imageResizer = null;
            if ((".jpg" == extension) || (".jpeg" == extension))
            {
                imageResizer = new JpegImageResizer();
            }
            ms = imageResizer?.CreateThumbnail(imageContent);
            return ms;
        }

        #endregion

    }
}