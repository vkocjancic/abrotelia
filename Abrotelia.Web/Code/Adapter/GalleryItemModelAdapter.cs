using Abrotelia.Web.Code.Common;
using Abrotelia.Web.Code.Persistence;
using Abrotelia.Web.Code.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.Adapter
{
    public class GalleryItemModelAdapter : IModelAdapter<PMGalleryItem, VMGalleryItem>
    {

        #region IModelAdapter implementation

        public PMGalleryItem ToPersistenceModel(VMGalleryItem modelView)
        {
            return new PMGalleryItem()
            {
                Author = modelView.Author,
                Description = modelView.Description,
                Era = modelView.Era,
                Height = modelView.Height,
                Id = modelView.Id,
                ImageId = modelView.ImageId,
                IsFeatured = modelView.IsFeatured,
                ItemAuthorId = modelView.ItemAuthorId,
                PermaLink = PermalinkGenerator.GetSlug(modelView.Title),
                Price = modelView.Price,
                Produced = modelView.Produced,
                Techniques = modelView.Techniques,
                Title = modelView.Title,
                Updated = modelView.Updated,
                Width = modelView.Width
            };
        }

        public VMGalleryItem ToViewModel(PMGalleryItem modelPersistence)
        {
            return new VMGalleryItem()
            {
                Author = modelPersistence.Author,
                Description = modelPersistence.Description,
                Era = modelPersistence.Era,
                Height = modelPersistence.Height,
                Id = modelPersistence.Id,
                ImageId = modelPersistence.ImageId,
                IsFeatured = modelPersistence.IsFeatured,
                ItemAuthorId = modelPersistence.ItemAuthorId,
                PermaLink = modelPersistence.PermaLink,
                Price = modelPersistence.Price,
                Produced = modelPersistence.Produced,
                Techniques = modelPersistence.Techniques,
                Title = modelPersistence.Title,
                Updated = modelPersistence.Updated,
                Width = modelPersistence.Width
            };
        }

        #endregion

    }
}