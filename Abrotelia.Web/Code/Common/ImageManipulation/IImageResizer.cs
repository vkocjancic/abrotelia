using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abrotelia.Web.Code.Common.ImageManipulation
{
    public interface IImageResizer
    {

        MemoryStream CreateImage(byte[] imageContent);
        MemoryStream CreateThumbnail(byte[] imageContent);

    }
}
