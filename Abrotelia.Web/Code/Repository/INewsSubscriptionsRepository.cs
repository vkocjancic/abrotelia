using Abrotelia.Web.Code.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.Repository
{
    public interface INewsSubscriptionsRepository : IRepository<VMNewsSubscription>
    {

        VMNewsSubscription GetByEmail(string email);

    }
}