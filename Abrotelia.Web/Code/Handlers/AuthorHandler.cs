using Abrotelia.Web.Code.Repository;
using Abrotelia.Web.Code.ViewModels;
using System;
using System.Web;
using System.Web.Helpers;

namespace Abrotelia.Web.Code.Handlers
{
    public class AuthorHandler : HttpHandlerBase
    {

        #region Constructors

        public AuthorHandler() : base(typeof(AuthorHandler)) { }

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
                m_log.Debug($"Author id: {id}; Mode: {mode}");
                if ("save" == mode)
                {
                    EditAuthor(
                        id,
                        context.Request["fullName"],
                        context.Request["description"],
                        context.User.Identity.Name,
                        new AuthorsRepository());
                    m_log.Info("Author saved");
                }
                else if ("delete" == mode)
                {
                    DeleteAuthor(id, new AuthorsRepository());
                    m_log.Info("Author deleted");
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

        private void DeleteAuthor(string id, IAuthorsRepository repository)
        {
            HttpContext.Current.Response.Write("/Admin/Authors.cshtml");
            repository.DeleteById(id);
        }

        private void EditAuthor(string id, string fullName, string description, string user, IAuthorsRepository repository)
        {
            var author = repository.GetById(id);
            var isNew = true;
            if (null == author)
            {
                author = new VMAuthor()
                {
                    Id = Guid.NewGuid().ToString(),
                    Author = user,
                    Description = description,
                    FullName = fullName,
                    Updated = DateTime.Now
                };
            }
            else
            {
                isNew = false;
                author.FullName = fullName;
                author.Description = description;
                author.Updated = DateTime.Now;
            }
            HttpContext.Current.Response.Write("/Admin/Authors.cshtml");
            author.Save(repository, isNew);
        }

        #endregion
    }
}
