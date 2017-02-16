using Abrotelia.Web.Code.Repository;
using System;
using System.Web;

namespace Abrotelia.Web.Code.Handlers
{
    public class ImageHandler : HttpHandlerBase
    {

        #region Constructors

        public ImageHandler() : base(typeof(ImageHandler)) { }

        #endregion

        #region HttpHandlerBase implementation

        public override void ProcessRequest(HttpContext context)
        {
            try
            {
                m_log.Info($"Processing request invoked from {context.Request.UserHostAddress}");
                var size = context.Request.QueryString["size"];
                var id = context.Request.QueryString["id"];
                m_log.Debug($"Image id: {id}; Size: {size}");
                if (!string.IsNullOrEmpty(id))
                {
                    var repository = new GalleryItemsRepository();
                    var image = repository.GetImageByIdAndSize(id, size);
                    if (null == image)
                    {
                        m_log.Debug($"Image with id {id} was not found");
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
                        m_log.Info($"Image found and returned to {context.Request.UserHostAddress}");
                    }
                }
                context.Response.Flush();
            }
            catch (Exception ex)
            {
                m_log.Fatal(ex, $"{ex.Message} {ex.StackTrace}");
                throw new HttpException(500, "Internal server error");
            }
        }

        #endregion
    }
}
