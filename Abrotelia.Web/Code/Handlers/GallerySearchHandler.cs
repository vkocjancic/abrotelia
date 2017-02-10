using Abrotelia.Web.Code.Repository;
using Abrotelia.Web.Code.Search;
using Newtonsoft.Json;
using System;
using System.Web;

namespace Abrotelia.Web.Code.Handlers
{
    public class GallerySearchHandler : IHttpHandler
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
            var type = context.Request.QueryString["type"];
            var query = context.Request.QueryString["query"];
            if (null == query)
                throw new HttpException(400, "Bad request");
            query = HttpUtility.HtmlEncode(query);
            if ("typeahead" == type)
            {
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                HttpContext.Current.Response.ContentType = "application/json";
                HttpContext.Current.Response.Write(
                    JsonConvert.SerializeObject(
                        GalleryItemSearch.Search(query, new GalleryItemsRepository(), new AuthorsRepository())));
            }
        }

        #endregion
    }
}
