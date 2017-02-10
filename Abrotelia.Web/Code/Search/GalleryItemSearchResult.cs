using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.Search
{
    public class GalleryItemSearchResult
    {
        #region Properties

        public List<string> Items { get; set; }

        #endregion

        #region Constructors

        public GalleryItemSearchResult()
        {
            Items = new List<string>();
        }

        #endregion

        #region Public methods

        public void SortItems()
        {
            Items.Sort();
        }

        #endregion

    }
}