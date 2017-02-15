using Abrotelia.Web.Code.Repository;
using Abrotelia.Web.Code.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace Abrotelia.Web.Code.Common
{
    public static class Site
    {

        #region Properties

        public static string Copyright { get; private set; }
        public static string Description { get; private set; }
        public static string Image { get; private set; }
        public static string MetaDescription { get; private set; }
        public static string Title { get; private set; }
        public static string Theme { get; private set; }

        public static bool IsAuthorPage
        {
            get
            {
                return "author" == HttpContext.Current.Request["view"];
            }
        }

        public static bool IsGalleryItemPage
        {
            get
            {
                return "galleryItem" == HttpContext.Current.Request["view"];
            }
        }

        public static bool IsGalleryPage
        {
            get
            {
                return "gallery" == HttpContext.Current.Request["view"];
            }
        }

        public static VMAuthor CurrentAuthor
        {
            get
            {
                var slug = HttpUtility.HtmlEncode(HttpContext.Current.Request["slug"]);
                var id = HttpUtility.HtmlEncode(HttpContext.Current.Request["id"]);
                if (!string.IsNullOrEmpty(slug) && !string.IsNullOrEmpty(id))
                {
                    var repository = new AuthorsRepository();
                    return repository.GetById(id);
                }
                return null;
            }
        }

        public static VMGalleryItem CurrentGalleryItem
        {
            get
            {
                var slug = HttpUtility.HtmlEncode(HttpContext.Current.Request["slug"]);
                var id = HttpUtility.HtmlEncode(HttpContext.Current.Request["id"]);
                if (!string.IsNullOrEmpty(slug) && !string.IsNullOrEmpty(id))
                {
                    return VMGalleryItem.Load(id, new GalleryItemsRepository(), new AuthorsRepository());
                }
                return null;
            }
        }

        public static VMPage CurrentPage
        {
            get
            {
                var slug = HttpUtility.HtmlEncode(HttpContext.Current.Request["slug"]);
                if (!string.IsNullOrEmpty(slug))
                {
                    var repository = new PagesRepository();
                    return repository.GetBySlug(slug);
                }
                return null;
            }
        }

        public static string GalleryQuery
        {
            get
            {
                return HttpUtility.HtmlEncode(HttpContext.Current.Request["query"]);
            }
        }

        #endregion

        #region Constructors

        static Site()
        {
            Copyright = ConfigurationManager.AppSettings.Get("site:copyright");
            Description = ConfigurationManager.AppSettings.Get("site:description");
            Image = ConfigurationManager.AppSettings.Get("site:image");
            MetaDescription = ConfigurationManager.AppSettings.Get("site:metadescription");
            Theme = ConfigurationManager.AppSettings.Get("site:theme");
            Title = ConfigurationManager.AppSettings.Get("site:title");
        }

        #endregion

        #region Static methods

        public static string FingerPrint(string rootRelativePath)
        {
            if (null == HttpRuntime.Cache[rootRelativePath])
            {
                var relative = VirtualPathUtility.ToAbsolute("~" + rootRelativePath);
                var absolute = HostingEnvironment.MapPath(relative);
                if (!File.Exists(absolute))
                    throw new FileNotFoundException("File not found", absolute);
                var date = File.GetLastWriteTime(absolute);
                var index = relative.LastIndexOf('.');
                var result = relative.Insert(index, "_" + date.Ticks);
                HttpRuntime.Cache.Insert(rootRelativePath, result, new CacheDependency(absolute));
            }
            return HttpRuntime.Cache[rootRelativePath] as string;
        }


        public static VMGalleryItems GetFeaturedGalleryItems()
        {
            return VMGalleryItems.LoadFeatured(new GalleryItemsRepository(), new AuthorsRepository());
        }

        public static VMGalleryItems GetNewGalleryItems()
        {
            return VMGalleryItems.LoadNew(new GalleryItemsRepository(), new AuthorsRepository());
        }

        public static void SetConditionalGetHeaders(DateTime dateLastModified, HttpContextBase context)
        {
            var response = context.Response;
            var request = context.Request;
            dateLastModified = new DateTime(dateLastModified.Year, dateLastModified.Month, dateLastModified.Day, dateLastModified.Hour, dateLastModified.Minute, dateLastModified.Second);
            var incomingDate = request.Headers["If-Modified-Since"];
            response.Cache.SetLastModified(dateLastModified);
            var dateToTest = DateTime.MinValue;
            if (DateTime.TryParse(incomingDate, out dateToTest) && dateToTest == dateLastModified)
            {
                response.ClearContent();
                response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
                response.SuppressContent = true;
            }
        }

        #endregion

    }
}