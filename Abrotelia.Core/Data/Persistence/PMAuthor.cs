using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Core.Data.Persistence
{
    public class PMAuthor
    {

        #region Properties

        public string Id { get; set; }
        public string Author { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public string PermaLink { get; set; }
        public DateTime Updated { get; set; }

        #endregion

    }
}