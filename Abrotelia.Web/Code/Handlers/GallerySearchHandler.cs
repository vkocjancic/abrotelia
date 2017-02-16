using Abrotelia.Web.Code.Repository;
using Abrotelia.Web.Code.Search;
using Newtonsoft.Json;
using System;
using System.Web;

namespace Abrotelia.Web.Code.Handlers
{
    public class GallerySearchHandler : HttpHandlerBase
    {

        #region Constructors

        public GallerySearchHandler() : base(typeof(GallerySearchHandler)) { }

        #endregion

        #region IHttpHandler implementation

        public override void ProcessRequest(HttpContext context)
        {
            try
            {
                m_log.Info("Processing request invoked");
                var type = context.Request.QueryString["type"];
                var query = context.Request.QueryString["query"];
                m_log.Debug($"Search type: {type}; Query: {query}");
                if (null == query)
                {
                    m_log.Error("Query was not specified");
                    throw new HttpException(400, "Bad request");
                }
                query = HttpUtility.HtmlEncode(query);
                if ("typeahead" == type)
                {
                    HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    HttpContext.Current.Response.ContentType = "application/json";
                    HttpContext.Current.Response.Write(
                        JsonConvert.SerializeObject(
                            GalleryItemSearch.Search(query, new GalleryItemsRepository(), new AuthorsRepository())));
                    m_log.Info("Search completed");
                }
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
