using Abrotelia.Web.Code.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abrotelia.Web.Code.Repository
{
    public interface IGalleryItemsRepository : IRepository<VMGalleryItem>
    {

        void DeleteImage(string id);
        IList<VMGalleryItem> GetAllFeatured();
        IList<VMGalleryItem> GetByAuthor(string authorId);
        VMImage GetImageByIdAndSize(string id, string size);
        IList<VMGalleryItem> GetNew();
        IList<VMGalleryItem> Search(string query, IList<VMAuthor> authors);
        IList<VMGalleryItem> SearchTypeahead(string query);
        VMGalleryItem ToggleFeatured(string id);
        
    }
}
