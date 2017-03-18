using Abrotelia.Core.Data.Persistence;
using Abrotelia.Web.Code.Common;
using Abrotelia.Web.Code.ViewModels;

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
                Footer = new PMMenuItem() { Category = modelView.FooterCategory, Priority = modelView.FooterMenuPriority, Title = modelView.FooterMenuTitle },
                Header = new PMMenuItem() { Category = modelView.HeaderCategory, Priority = modelView.HeaderMenuPriority, Title = modelView.HeaderMenuTitle },
                PageStatus = modelView.PageStatus,
                PermaLink = PermalinkGenerator.GetSlug(modelView.Title).Replace("č", "c").Replace("š", "s").Replace("ž", "z"),
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
                FooterCategory = modelPersistence.Footer?.Category,
                FooterMenuPriority = modelPersistence.Footer?.Priority,
                FooterMenuTitle = modelPersistence.Footer?.Title,
                HeaderCategory = modelPersistence.Header?.Category,
                HeaderMenuPriority = modelPersistence.Header?.Priority,
                HeaderMenuTitle = modelPersistence.Header?.Title,
                PageStatus = modelPersistence.PageStatus,
                PermaLink = modelPersistence.PermaLink,
                Title = modelPersistence.Title,
                Updated = modelPersistence.Updated
            };
        } 

        #endregion

    }
}