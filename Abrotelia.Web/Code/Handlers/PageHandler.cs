using Abrotelia.Web.Code.Common;
using Abrotelia.Web.Code.Repository;
using Abrotelia.Web.Code.ViewModels;
using System;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;

namespace Abrotelia.Web.Code.Handlers
{
    public class PageHandler : HttpHandlerBase
    {

        #region Constructors

        public PageHandler() : base(typeof(PageHandler)) { }

        #endregion

        #region HttpHandlerBase implementation

        public override void ProcessRequest(HttpContext context)
        {
            try
            {
                m_log.Info("Processing request invoked");
                AntiForgery.Validate();
                m_log.Debug("AntiForgery check OK");
                if (!context.User.Identity.IsAuthenticated)
                {
                    m_log.Error("User not authenticated");
                    throw new HttpException(403, "No access");
                }
                var mode = context.Request.QueryString["mode"];
                var id = context.Request.Form["id"];
                m_log.Debug($"Page id: {id}; Mode: {mode}");
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
                    m_log.Info("Page saved");
                }
                else if ("delete" == mode)
                {
                    DeletePage(id, new PagesRepository());
                    m_log.Info("Page deleted");
                }
            }
            catch (Exception ex)
            {
                m_log.Fatal(ex, $"{ex.Message} {ex.StackTrace}");
                throw new HttpException(500, "Internal server error");
            }
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
