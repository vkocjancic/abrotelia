using Abrotelia.Web.Code.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.ViewModels
{
    public class VMNewsSubscriptions : CollectionBase<VMNewsSubscription>
    {

        #region Factory methods

        public static VMNewsSubscriptions Load(INewsSubscriptionsRepository repository)
        {
            var model = new VMNewsSubscriptions();
            model.Items = repository.GetAll();
            return model;
        }

        public static VMNewsSubscription LoadByEmail(string email, INewsSubscriptionsRepository repository)
        {
            return repository.GetByEmail(email);
        }

        #endregion

    }
}