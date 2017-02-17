using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Core.Data.Persistence
{
    public class PMNewsSubscription
    {

        #region Properties

        public string Id { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public DateTime Updated { get; set; }

        #endregion

    }
}