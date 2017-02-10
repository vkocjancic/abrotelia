using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Abrotelia.Web.Code.Repository
{
    public abstract class BaseRepository
    {

        #region Properties

        public string DatabaseName => System.IO.Path.Combine(HostingEnvironment.MapPath("~/App_Data"), "abrotelia.db");

        #endregion

    }
}