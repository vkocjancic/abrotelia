using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.Common.ImageManipulation
{
    public abstract class ImageResizerBase : IImageResizer
    {

        #region Abstract methods

        public abstract MemoryStream CreateImage(byte[] imageContent);
        public abstract MemoryStream CreateThumbnail(byte[] imageContent);

        #endregion

        #region Protected methods

        protected virtual void DetermineSizeOfImage(int width, int height, out int calcWidth, out int calcHeight)
        {
            DetermineImageSize(width, height, 640, 480, out calcWidth, out calcHeight);
        }

        protected virtual void DetermineSizeOfThumbnail(int width, int height, out int calcWidth, out int calcHeight)
        {
            DetermineImageSize(width, height, 150, 120, out calcWidth, out calcHeight);
        }

        protected virtual void DetermineImageSize(int width, int height, int maxWidth, int maxHeight, out int calcWidth, out int calcHeight)
        {
            var maxRatio = maxWidth / (decimal)maxHeight;
            var imageRatio = width / (decimal)height;
            if (width >= (height * maxRatio))
            {
                calcWidth = maxWidth;
                calcHeight = Math.Min(maxHeight, (int)(maxWidth / imageRatio));
            }
            else
            {
                calcWidth = Math.Min(maxWidth, (int)(maxHeight * imageRatio));
                calcHeight = maxHeight;
            }
        }

        #endregion

    }
}