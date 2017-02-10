using Abrotelia.Web.Code.Common;
using Abrotelia.Web.Code.Persistence;
using Abrotelia.Web.Code.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.Adapter
{
    public class AuthorModelAdapter : IModelAdapter<PMAuthor, VMAuthor>
    {

        #region IModelAdapter implementation

        public PMAuthor ToPersistenceModel(VMAuthor modelView)
        {
            return new PMAuthor()
            {
                Author = modelView.Author,
                Description = modelView.Description,
                FullName = modelView.FullName,
                Id = modelView.Id,
                Image = modelView.Image,
                PermaLink = PermalinkGenerator.GetSlug(modelView.FullName),
                Updated = modelView.Updated
            };
        }

        public VMAuthor ToViewModel(PMAuthor modelPersistence)
        {
            return new VMAuthor()
            {
                Author = modelPersistence.Author,
                Description = modelPersistence.Description,
                FullName = modelPersistence.FullName,
                Id = modelPersistence.Id,
                Image = modelPersistence.Image,
                PermaLink = modelPersistence.PermaLink,
                Updated = modelPersistence.Updated
            };
        }

        #endregion

    }
}