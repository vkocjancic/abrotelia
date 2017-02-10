using Abrotelia.Web.Code.Repository;
using System;
using System.Web;

namespace Abrotelia.Web.Code.Handlers
{
    public class ImageHandler : IHttpHandler
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
            var size = context.Request.QueryString["size"];
            var id = context.Request.QueryString["id"];
            if (!string.IsNullOrEmpty(id))
            {
                var repository = new GalleryItemsRepository();
                var image = repository.GetImageByIdAndSize(id, size);
                if (null == image)
                {
                    context.Response.ContentType = "image/jpeg";
                }
                else
                {
                    context.Response.ContentType = image.Mime;
                    context.Response.Cache.SetCacheability(HttpCacheability.Public);
                    context.Response.Cache.SetExpires(DateTime.Now.AddYears(1));
                    context.Response.Cache.SetMaxAge(new TimeSpan(365, 0, 0, 0));
                    context.Response.AddHeader("Last-Modified", DateTime.UtcNow.AddDays(-30).ToString("R"));
                    context.Response.BinaryWrite(image.Content);
                }
            }
            context.Response.Flush();
        }

        #endregion
    }
}
