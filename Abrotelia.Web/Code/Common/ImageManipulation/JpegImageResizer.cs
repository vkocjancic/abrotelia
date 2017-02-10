using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.Common.ImageManipulation
{
    public class JpegImageResizer : ImageResizerBase
    {

        #region ImageResizerBase implementation

        public override MemoryStream CreateImage(byte[] imageContent)
        {
            MemoryStream msImage = new MemoryStream();
            using (var ms = new MemoryStream(imageContent))
            using (var image = System.Drawing.Image.FromStream(ms))
            {
                int calcWidth;
                int calcHeight;
                DetermineSizeOfImage(image.Width, image.Height, out calcWidth, out calcHeight);
                var newImage = new Bitmap(calcWidth, calcHeight);
                using (var graphics = Graphics.FromImage(newImage))
                {
                    // zakomentirano zaradi out of memory exception-a
                    //graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //graphics.SmoothingMode = SmoothingMode.HighQuality;
                    //graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    //graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.DrawImage(image, 0, 0, calcWidth, calcHeight);
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 80L);
                    newImage.Save(msImage, ImageCodecInfo.GetImageEncoders()[1], encoderParameters);
                }
                msImage.Seek(0, SeekOrigin.Begin);
            }
            return msImage;
        }

        public override MemoryStream CreateThumbnail(byte[] imageContent)
        {
            MemoryStream msThumb = new MemoryStream();
            using (var ms = new MemoryStream(imageContent))
            using (var image = System.Drawing.Image.FromStream(ms))
            {
                int thumbWidth;
                int thumbHeight;
                DetermineSizeOfThumbnail(image.Width, image.Height, out thumbWidth, out thumbHeight);
                using (var imageThumb = image.GetThumbnailImage(thumbWidth, thumbHeight, 
                    new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero))
                {
                    imageThumb.Save(msThumb, System.Drawing.Imaging.ImageFormat.Jpeg);
                    msThumb.Seek(0, SeekOrigin.Begin);
                }
            }
            return msThumb;
        }

        private bool ThumbnailCallback()
        {
            return true;
        }

        #endregion

    }
}