using Abrotelia.Web.Code.Repository;
using Abrotelia.Web.Code.ViewModels;
using System;
using System.Web;
using System.Web.Helpers;

namespace Abrotelia.Web.Code.Handlers
{
    public class NewsSubscriptionHandler : HttpHandlerBase
    {
        #region Constructors

        public NewsSubscriptionHandler() : base(typeof(NewsSubscriptionHandler)) { }

        #endregion

        #region HttpHandlerBase implementation

        public override void ProcessRequest(HttpContext context)
        {
            try
            {
                m_log.Info($"ProcessRequest invoked for {context.Request.UserHostAddress}");
                AntiForgery.Validate();
                m_log.Debug($"AntiForgery check OK for {context.Request.UserHostAddress}");
                var mode = context.Request.QueryString["mode"];
                var email = context.Request.Form["email"];
                var id = context.Request.Form["id"];
                m_log.Debug($"Subscription id: {id}; Mode: {mode}; Email: {email} for {context.Request.UserHostAddress}");
                if ("save" == mode)
                {
                    AddNewsSubscription(email, new NewsSubscriptionsRepository());
                    m_log.Info($"Subscription added for {context.Request.UserHostAddress}");
                }
                else if ("delete" == mode)
                {
                    DeleteNewsSubscription(id, new NewsSubscriptionsRepository());
                    m_log.Info($"Subscription deleted for {context.Request.UserHostAddress}");
                }
            }
            catch (Exception ex)
            {
                m_log.Info($"Error occured handling request for {context.Request.UserHostAddress}");
                m_log.Fatal(ex, $"{ex.Message} {ex.StackTrace}");
                throw new HttpException(500, "Internal server error");
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
