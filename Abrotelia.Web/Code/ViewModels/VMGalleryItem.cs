using Abrotelia.Web.Code.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.ViewModels
{
    public class VMGalleryItem : VMBase
    {

        #region Properties

        public string Description { get; set; }
        public string Era { get; set; }
        public int? Height { get; set; }
        public Guid? ImageId { get; set; }
        public byte[] ImageContent { get; set; }
        public string ImageExtension { get; set; }
        public bool IsFeatured { get; set; }
        public string ItemAuthorFullName { get; set; }
        public string ItemAuthorPermaLink { get; set; }
        public string ItemAuthorId { get; set; }
        public string PermaLink { get; set; }
        public decimal? Price { get; set; }
        public int? Produced { get; set; }
        public string[] Techniques { get; set; }
        public string Title { get; set; }
        public int? Width { get; set; }

        public string SizeDisplay
        {
            get
            {
                if ((null == Height) || (null == Width))
                {
                    return null;
                }
                return $"{Height.Value/10.0M} x {Width.Value/10.0M} cm";
            }
        }

        public string TechniquesDisplay
        {
            get
            {
                if (null == Techniques)
                {
                    return "";
                }
                else
                {
                    return string.Join(", ", Techniques);
                }
            }
        }

        public string FeaturedClass
        {
            get
            {
                return (IsFeatured) ? "glyphicon-star" : "glyphicon-star-empty";
            }
        }

        public string FeaturedText
        {
            get
            {
                return (IsFeatured) ? "Predstavljen artikel" : "Dodaj med predstavljene artikle";
            }
        }

        #endregion

        #region Factory methods

        public static VMGalleryItem Load(string id, IGalleryItemsRepository repository, IAuthorsRepository repositoryAuthors)
        {
            var model = repository.GetById(id);
            if (!string.IsNullOrEmpty(model.ItemAuthorId))
            {
                var author = repositoryAuthors.GetById(model.ItemAuthorId);
                model.ItemAuthorFullName = author.FullName;
                model.ItemAuthorPermaLink = author.PermaLink;
            }
            return model;
        }

        #endregion

        #region Public methods

        public void DeleteImage(IGalleryItemsRepository repository)
        {
            if (null != ImageId)
            {
                repository.DeleteImage(ImageId.Value.ToString());
            }
        }

        public void Save(IGalleryItemsRepository repository, bool isNew)
        {
            repository.Save(this, isNew);
        }

        #endregion

        #region Overridden methods

        protected override string GetDefaultDescription()
        {
            return Title;
        }

        protected override string GetDescription()
        {
            return Description;
        }

        #endregion

    }
}