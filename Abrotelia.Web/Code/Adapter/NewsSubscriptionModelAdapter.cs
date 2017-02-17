using Abrotelia.Core.Data.Persistence;
using Abrotelia.Web.Code.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.Adapter
{
    public class NewsSubscriptionModelAdapter : IModelAdapter<PMNewsSubscription, VMNewsSubscription>
    {
        #region IModelAdapter implementation

        public PMNewsSubscription ToPersistenceModel(VMNewsSubscription modelView)
        {
            return new PMNewsSubscription()
            {
                Author = modelView.Author,
                Email = modelView.Email,
                Id = modelView.Id,
                Updated = modelView.Updated
            };
        }

        public VMNewsSubscription ToViewModel(PMNewsSubscription modelPersistence)
        {
            return new VMNewsSubscription()
            {
                Author = modelPersistence.Author,
                Email = modelPersistence.Email,
                Id = modelPersistence.Id,
                Updated = modelPersistence.Updated
            };
        } 

        #endregion
    }
}