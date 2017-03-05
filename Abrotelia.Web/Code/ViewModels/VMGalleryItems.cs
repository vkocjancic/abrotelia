using Abrotelia.Web.Code.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.ViewModels
{
    public class VMGalleryItems : CollectionBase<VMGalleryItem>
    {

        #region Factory methods

        public static VMGalleryItems Load(IGalleryItemsRepository repository, IAuthorsRepository repositoryAuthors)
        {
            var model = new VMGalleryItems();
            model.Items = repository.GetAll().OrderBy(i => i.Title).ToList();
            foreach(var item in model.Items)
            {
                if (!string.IsNullOrEmpty(item.ItemAuthorId))
                {
                    var author = repositoryAuthors.GetById(item.ItemAuthorId);
                    item.ItemAuthorFullName = author.FullName;
                    item.ItemAuthorPermaLink = author.PermaLink;
                }
            }
            return model;
        }

        public static VMGalleryItems Load(string query, IGalleryItemsRepository repository, IAuthorsRepository repositoryAuthors)
        {
            var model = new VMGalleryItems();
            model.Items = repository.Search(query, repositoryAuthors.Search(query));
            foreach (var item in model.Items)
            {
                if (!string.IsNullOrEmpty(item.ItemAuthorId))
                {
                    var author = repositoryAuthors.GetById(item.ItemAuthorId);
                    item.ItemAuthorFullName = author.FullName;
                    item.ItemAuthorPermaLink = author.PermaLink;
                }
            }
            return model;
        }

        public static VMGalleryItems LoadByAuthor(string authorId, IGalleryItemsRepository repository, IAuthorsRepository repositoryAuthors)
        {
            var author = repositoryAuthors.GetById(authorId);
            var model = new VMGalleryItems();
            if (null != author)
            {
                model = LoadByAuthor(author, repository);
            }
            return model;
        }

        public static VMGalleryItems LoadByAuthor(VMAuthor author, IGalleryItemsRepository repository)
        {
            var model = new VMGalleryItems();
            model.Items = repository.GetByAuthor(author.Id);
            foreach (var item in model.Items)
            {
                if (!string.IsNullOrEmpty(item.ItemAuthorId))
                {
                    item.ItemAuthorFullName = author.FullName;
                    item.ItemAuthorPermaLink = author.PermaLink;
                }
            }
            return model;
        }

        public static VMGalleryItems LoadFeatured(IGalleryItemsRepository repository, IAuthorsRepository repositoryAuthors)
        {
            var model = new VMGalleryItems();
            model.Items = repository.GetAllFeatured();
            foreach (var item in model.Items)
            {
                if (!string.IsNullOrEmpty(item.ItemAuthorId))
                {
                    var author = repositoryAuthors.GetById(item.ItemAuthorId);
                    item.ItemAuthorFullName = author.FullName;
                    item.ItemAuthorPermaLink = author.PermaLink;
                }
            }
            return model;
        }

        public static VMGalleryItems LoadNew(IGalleryItemsRepository repository, IAuthorsRepository repositoryAuthors)
        {
            var model = new VMGalleryItems();
            model.Items = repository.GetNew();
            foreach (var item in model.Items)
            {
                if (!string.IsNullOrEmpty(item.ItemAuthorId))
                {
                    var author = repositoryAuthors.GetById(item.ItemAuthorId);
                    item.ItemAuthorFullName = author.FullName;
                    item.ItemAuthorPermaLink = author.PermaLink;
                }
            }
            return model;
        }

        #endregion

    }
}