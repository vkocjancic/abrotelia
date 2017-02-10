using Abrotelia.Web.Code.Common;
using Abrotelia.Web.Code.Repository;
using Abrotelia.Web.Code.ViewModels;
using System;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;

namespace Abrotelia.Web.Code.Handlers
{
    public class PageHandler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler implementation

        public void ProcessRequest(HttpContext context)
        {
            AntiForgery.Validate();
            if (!context.User.Identity.IsAuthenticated)
            {
                throw new HttpException(403, "No access");
            }
            var mode = context.Request.QueryString["mode"];
            var id = context.Request.Form["id"];
            if ("save" == mode)
            {
                EditPage(
                    id,
                    context.Request["title"],
                    context.Request["content"],
                    context.Request["headerCategory"],
                    context.Request["footerCategory"],
                    context.User.Identity.Name,
                    new PagesRepository());
            }
            else if ("delete" == mode)
            {
                DeletePage(id, new PagesRepository());
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }

        #endregion

        #region Private methods

        private void DeletePage(string id, IPagesRepository repository)
        {
            HttpContext.Current.Response.Write("/Admin/Pages.cshtml");
            repository.DeleteById(id);
        }

        private void EditPage(string id, string title, string content, string headerCategory, string footerCategory, string user, IPagesRepository repository)
        {
            var page = repository.GetById(id);
            var isNew = true;
            if (null == page)
            {
                page = new VMPage()
                {
                    Id = Guid.NewGuid().ToString(),
                    Author = user,
                    Content = content,
                    FooterCategory = footerCategory,
                    HeaderCategory = headerCategory,
                    PageStatus = PageStatus.Published,
                    Title = title,
                    Updated = DateTime.Now
                };
            }
            else
            {
                isNew = false;
                page.Title = title;
                page.Content = content;
                page.FooterCategory = footerCategory;
                page.HeaderCategory = headerCategory;
                page.Updated = DateTime.Now;
            }
            HttpContext.Current.Response.Write("/Admin/Pages.cshtml");
            page.Save(repository, isNew);
        }

        #endregion
    }
}
