﻿using Abrotelia.Web.Code.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.Persistence
{
    public class PMPage
    {
        
        #region Properties

        public string Id { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public string FooterCategory { get; set; }
        public string HeaderCategory { get; set; }
        public PageStatus PageStatus { get; set; }
        public string PermaLink { get; set; }
        public string Title { get; set; }
        public DateTime Updated { get; set; }

        #endregion

    }
}