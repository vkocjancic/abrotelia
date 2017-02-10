using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.ViewModels
{
    public class VMImage
    {

        #region Properties

        public byte[] Content { get; set; }
        public string Mime { get; set; }
        public string Extension { get; set; }

        #endregion

    }
}