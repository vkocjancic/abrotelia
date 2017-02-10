using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.ViewModels
{
    public class CollectionBase<T>
    {

        #region Properties

        public IList<T> Items { get; set; } = new List<T>();

        #endregion

    }
}