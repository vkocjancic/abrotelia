using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.ViewModels
{
    public class VMNewsSubscription : VMBase
    {

        #region Properties

        public string Email { get; set; }

        #endregion

        #region Overridden methods

        protected override string GetDefaultDescription()
        {
            throw new NotImplementedException();
        }

        protected override string GetDescription()
        {
            throw new NotImplementedException();
        }
        
        #endregion

    }
}