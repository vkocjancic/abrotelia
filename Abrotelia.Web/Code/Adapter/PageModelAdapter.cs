using Abrotelia.Web.Code.Common;
using Abrotelia.Web.Code.Persistence;
using Abrotelia.Web.Code.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.Adapter
{
    public class PageModelAdapter : IModelAdapter<PMPage, VMPage>
    {
        
        #region IModelAdapter implementation

        public PMPage ToPersistenceModel(VMPage modelView)
        {
            return new PMPage()
            {
                Id = modelView.Id,
                Author = modelView.Author,
                Content = modelView.Content,
                FooterCategory = modelView.FooterCategory,
                HeaderCategory = modelView.HeaderCategory,
                PageStatus = modelView.PageStatus,
                PermaLink = PermalinkGenerator.GetSlug(modelView.Title),
                Title = modelView.Title,
                Updated = modelView.Updated
            };
        }

        public VMPage ToViewModel(PMPage modelPersistence)
        {
            return new VMPage()
            {
                Id = modelPersistence.Id,
                Author = modelPersistence.Author,
                Content = modelPersistence.Content,
                FooterCategory = modelPersistence.FooterCategory,
                HeaderCategory = modelPersistence.HeaderCategory,
                PageStatus = modelPersistence.PageStatus,
                Title = modelPersistence.Title,
                Updated = modelPersistence.Updated
            };
        } 

        #endregion

    }
}