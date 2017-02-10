using Abrotelia.Web.Code.Repository;
using Abrotelia.Web.Code.ViewModels;
using System;
using System.Web;
using System.Web.Helpers;

namespace Abrotelia.Web.Code.Handlers
{
    public class AuthorHandler : IHttpHandler
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
            if (!context.User.Identity.IsAuthenticated)
            {
                throw new HttpException(403, "No access");
            }
            var mode = context.Request.QueryString["mode"];
            var id = context.Request.Form["id"];
            if ("save" == mode)
            {
                EditAuthor(
                    id,
                    context.Request["fullName"],
                    context.Request["description"],
                    context.User.Identity.Name,
                    new AuthorsRepository());
            }
            else if ("delete" == mode)
            {
                DeleteAuthor(id, new AuthorsRepository());
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
