using Abrotelia.Web.Code.Repository;
using Abrotelia.Web.Code.ViewModels;
using System;
using System.Web;
using System.Web.Helpers;

namespace Abrotelia.Web.Code.Handlers
{
    public class NewsSubscriptionHandler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            AntiForgery.Validate();
            var mode = context.Request.QueryString["mode"];
            var email = context.Request.Form["email"];
            var id = context.Request.Form["id"];
            if ("save" == mode)
            {
                AddNewsSubscription(email, new NewsSubscriptionsRepository());
            }
            else if ("delete" == mode)
            {
                DeleteNewsSubscription(id, new NewsSubscriptionsRepository());
            }
        }

        #endregion

        #region Private methods

        private void AddNewsSubscription(string email, INewsSubscriptionsRepository repository)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new HttpException(400, "Bad request");
            }
            else if (null == VMNewsSubscriptions.LoadByEmail(email, repository))
            {
                var subscription = new VMNewsSubscription()
                {
                    Email = email,
                    Id = Guid.NewGuid().ToString(),
                    Updated = DateTime.Now
                };
                HttpContext.Current.Response.Write("true");
                repository.Save(subscription, true);
            }
            else
            {
                throw new HttpException(409, "Conflict");
            }
        }

        private void DeleteNewsSubscription(string id, INewsSubscriptionsRepository repository)
        {
            HttpContext.Current.Response.Write("/Admin/NewsSubscriptions.cshtml");
            repository.DeleteById(id);
        }

        #endregion
    }
}
