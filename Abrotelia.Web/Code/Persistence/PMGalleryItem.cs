using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.Persistence
{
    public class PMGalleryItem
    {

        #region Properties

        public string Id { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Era { get; set; }
        public int? Height { get; set; }
        public Guid? ImageId { get; set; }
        public bool IsFeatured { get; set; }
        public string ItemAuthorId { get; set; }
        public decimal? Price { get; set; }
        public int? Produced { get; set; }
        public string PermaLink { get; set; }
        public string[] Techniques { get; set; }
        public string Title { get; set; }
        public DateTime Updated { get; set; }
        public int? Width { get; set; }
        
        #endregion

    }
}