using Abrotelia.Web.Code.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.Search
{
    public static class GalleryItemSearch
    {

        #region Static methods

        public static GalleryItemSearchResult Search(string query, IGalleryItemsRepository repoGalleryItems, IAuthorsRepository repoAuthors)
        {
            var result = new GalleryItemSearchResult();
            if (!string.IsNullOrWhiteSpace(query))
            {
                foreach (var galleryItem in repoGalleryItems.SearchTypeahead(query))
                {
                    if (!result.Items.Contains(galleryItem.Title))
                    {
                        result.Items.Add(galleryItem.Title);
                    }
                }
                foreach (var author in repoAuthors.SearchTypeahead(query))
                {
                    if (!result.Items.Contains(author.FullName))
                    {
                        result.Items.Add(author.FullName);
                    }
                }
                result.SortItems();
            }
            return result;
        }

        #endregion

    }
}