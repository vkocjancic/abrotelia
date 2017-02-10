using Abrotelia.Web.Code.Repository;
using Abrotelia.Web.Code.ViewModels;
using System;
using System.Web;
using System.Web.Helpers;

namespace Abrotelia.Web.Code.Handlers
{
    public class GalleryItemHandler : IHttpHandler
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
                EditGalleryItem(
                    id,
                    context.Request["pageHeader"],
                    context.Request["description"],
                    context.Request["galleryItemAuthor"],
                    context.Request["galleryItemProduced"],
                    context.Request["galleryItemTechniques"],
                    context.Request["galleryItemEra"],
                    context.Request["galleryItemPrice"],
                    context.Request["galleryItemWidth"],
                    context.Request["galleryItemHeight"],
                    (context.Request.Files.Count > 0) ? context.Request.Files[0] : null,
                    new GalleryItemsRepository()
                );
            }
            else if ("delete" == mode)
            {
                DeleteGalleryItem(id, new GalleryItemsRepository());
            }
            else if ("toggleFeatured" == mode)
            {
                ToggleFeatured(id, new GalleryItemsRepository());
            }
        }

        #endregion

        #region Private methods

        private void EditGalleryItem(string id, string title, string description, string itemAuthorId, string produced, string techniques,
            string era, string price, string width, string height, HttpPostedFile uploadedFile, IGalleryItemsRepository repository)
        {
            var item = repository.GetById(id);
            bool isNew = true;
            if (null == item)
            {
                item = new VMGalleryItem()
                {
                    Author = "admin",
                    Description = description,
                    Era = era,
                    Id = Guid.NewGuid().ToString(),
                    ItemAuthorId = itemAuthorId,
                    Title = title,
                    Updated = DateTime.Now,
                };
            }
            else
            {
                isNew = false;
                item.Description = description;
                item.Era = era;
                item.ItemAuthorId = itemAuthorId;
                item.Title = title;
                item.Updated = DateTime.Now;
            }
            if (string.IsNullOrEmpty(price))
            {
                item.Price = null;
            }
            else
            {
                item.Price = Convert.ToDecimal(price.Replace(".", ","));
            }
            if (string.IsNullOrEmpty(produced))
            {
                item.Produced = null;
            }
            else
            {
                item.Produced = Convert.ToInt32(produced);
            }
            if (string.IsNullOrEmpty(width))
            {
                item.Width = null;
            }
            else
            {
                item.Width = Convert.ToInt32(width);
            }
            if (string.IsNullOrEmpty(height))
            {
                item.Height = null;
            }
            else
            {
                item.Height = Convert.ToInt32(height);
            }
            if (string.IsNullOrEmpty(techniques))
            {
                item.Techniques = null;
            }
            else
            {
                item.Techniques = techniques.Split(',');
            }
            if ((null != uploadedFile) && (0 != uploadedFile.ContentLength))
            {
                if (null != item.ImageId)
                {
                    item.DeleteImage(repository);
                }
                item.ImageId = Guid.NewGuid();
                var imageContent = new byte[uploadedFile.ContentLength];
                uploadedFile.InputStream.Read(imageContent, 0, uploadedFile.ContentLength);
                item.ImageContent = imageContent;
                item.ImageExtension = System.IO.Path.GetExtension(uploadedFile.FileName);
            }
            if (isNew)
            {
                HttpContext.Current.Response.Write("/Admin/GalleryItems.cshtml");
            }
            else
            {
                HttpContext.Current.Response.Write(item.ImageId);
            }
            item.Save(repository, isNew);
        }

        private void DeleteGalleryItem(string id, GalleryItemsRepository repository)
        {
            repository.DeleteById(id);
        }

        private void ToggleFeatured(string id, GalleryItemsRepository repository)
        {
            var item = repository.ToggleFeatured(id);
            if (null != item)
            {
                HttpContext.Current.Response.Write(item.FeaturedText);
            }
            else
            {
                throw new HttpException(404, "Item not found");
            }
        }

        #endregion
    }
}
