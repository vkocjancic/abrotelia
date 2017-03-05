using System.Collections.Generic;
using System.Linq;
using Abrotelia.Web.Code.ViewModels;
using LiteDB;
using Abrotelia.Web.Code.Adapter;
using System.IO;
using Abrotelia.Web.Code.Common.ImageManipulation;
using Abrotelia.Core.Data.Persistence;

namespace Abrotelia.Web.Code.Repository
{
    public class GalleryItemsRepository : BaseRepository, IGalleryItemsRepository
    {

        #region Fields

        protected readonly string m_roNormalImagePath = "$/gallery/normal";
        protected readonly string m_roThumbsImagePath = "$/gallery/thumbs";

        #endregion

        #region Properties

        protected IModelAdapter<PMGalleryItem, VMGalleryItem> Adapter => new GalleryItemModelAdapter();

        #endregion

        #region IGalleryItemsRepository implementation

        public void DeleteById(string id)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var docItem = db.GetCollection<PMGalleryItem>("galleryItems").FindById(id);
                if (null != docItem)
                {
                    if (null != docItem.ImageId)
                    {
                        DeleteImage(docItem.ImageId.Value.ToString());
                    }
                    db.GetCollection<PMGalleryItem>("galleryItems").Delete(id);
                }
            }
        }

        public void DeleteImage(string id)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                db.FileStorage.Delete($"{m_roNormalImagePath}/{id}");
                db.FileStorage.Delete($"{m_roThumbsImagePath}/{id}");
            }
        }

        public IList<VMGalleryItem> GetAll()
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var model = new List<VMGalleryItem>();
                foreach (var item in db.GetCollection<PMGalleryItem>("galleryItems").FindAll())
                {
                    model.Add(Adapter.ToViewModel(item));
                }
                return model;
            }
        }

        public IList<VMGalleryItem> GetAllFeatured()
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var model = new List<VMGalleryItem>();
                foreach (var item in db.GetCollection<PMGalleryItem>("galleryItems").Find(Query.EQ("IsFeatured", true)))
                {
                    model.Add(Adapter.ToViewModel(item));
                }
                return model;
            }
        }

        public IList<VMGalleryItem> GetByAuthor(string authorId)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var model = new List<VMGalleryItem>();
                foreach (var item in db.GetCollection<PMGalleryItem>("galleryItems").Find(Query.EQ("ItemAuthorId", authorId)))
                {
                    model.Add(Adapter.ToViewModel(item));
                }
                return model;
            }
        }

        public VMGalleryItem GetById(string id)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var item = db.GetCollection<PMGalleryItem>("galleryItems").FindById(id);
                return (null == item) ? null : Adapter.ToViewModel(item);
            }
        }

        public VMGalleryItem GetBySlug(string slug)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var item = db.GetCollection<PMGalleryItem>("galleryItems").FindOne(Query.EQ("PermaLink", slug));
                return (null == item) ? null : Adapter.ToViewModel(item);
            }
        }

        public VMImage GetImageByIdAndSize(string id, string size)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var fileInfo = db.FileStorage.FindById($"{(("normal" == size) ? m_roNormalImagePath : m_roThumbsImagePath)}/{id}");
                VMImage image = null;
                if (null != fileInfo)
                {
                    using (var ms = new MemoryStream())
                    {
                        fileInfo.CopyTo(ms);
                        image = new VMImage()
                        {
                            Content = ms.ToArray(),
                            Extension = fileInfo.Metadata.Get("extension"),
                            Mime = "image/jpeg"
                        };
                    }
                }
                return image;
            }
        }

        public IList<VMGalleryItem> GetNew()
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var model = new List<VMGalleryItem>();
                foreach (var item in db.GetCollection<PMGalleryItem>("galleryItems").FindAll())
                {
                    model.Add(Adapter.ToViewModel(item));
                }
                return model.OrderByDescending(m => m.Updated).Take(4).ToList();
            }
        }

        public void Save(VMGalleryItem item, bool isNew)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var items = db.GetCollection<PMGalleryItem>("galleryItems");
                var docItem = Adapter.ToPersistenceModel(item);
                if (isNew)
                {
                    items.Insert(docItem);
                }
                else
                {
                    items.Update(docItem);
                }
                if ((null != item.ImageId) && (null != item.ImageContent) && (0 < item.ImageContent.Length))
                {
                    using (var ms = ImageResizerFactory.CreateImage(item.ImageContent, item.ImageExtension))
                    using (var msThumb = ImageResizerFactory.CreateThumbnail(item.ImageContent, item.ImageExtension))
                    {
                        ms.Seek(0, SeekOrigin.Begin);
                        msThumb.Seek(0, SeekOrigin.Begin);
                        var dict = new Dictionary<string, BsonValue>();
                        dict.Add("extension", item.ImageExtension);
                        var path = $"{m_roNormalImagePath}/{item.ImageId.Value.ToString()}";
                        db.FileStorage.Upload(path, path, ms);
                        db.FileStorage.SetMetadata(path, new BsonDocument(dict));
                        path = $"{m_roThumbsImagePath}/{item.ImageId.Value.ToString()}";
                        db.FileStorage.Upload(path, path, msThumb);
                        db.FileStorage.SetMetadata(path, new BsonDocument(dict));
                    }
                }
            }
        }

        public IList<VMGalleryItem> Search(string query, IList<VMAuthor> authors)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var model = new List<VMGalleryItem>();
                int year = 0;
                int.TryParse(query, out year);
                foreach (var item in db.GetCollection<PMGalleryItem>("galleryItems").Find(
                    Query.Or(
                        Query.Or(
                            Query.Contains("Title", query),
                            Query.EQ("Produced", year)
                        ),
                        Query.Or(
                            Query.Or(
                                Query.Contains("Era", query),
                                Query.In("ItemAuthorId", new BsonArray(authors.Select(a => new BsonValue(a.Id))))
                            ),
                            Query.Or(
                                Query.EQ("Techniques", query),
                                Query.StartsWith("Techniques", query)
                            )
                        )
                    )))
                {
                    model.Add(Adapter.ToViewModel(item));
                }
                return model;
            }
        }

        public IList<VMGalleryItem> SearchTypeahead(string query)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var model = new List<VMGalleryItem>();
                int year = 0;
                int.TryParse(query, out year);
                foreach (var item in db.GetCollection<PMGalleryItem>("galleryItems").Find(
                    Query.Or(
                        Query.Contains("Title", query),
                        Query.EQ("Produced", year)
                    )
                    , limit: 10))
                {
                    model.Add(Adapter.ToViewModel(item));
                }
                return model;
            }
        }

        public VMGalleryItem ToggleFeatured(string id)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var items = db.GetCollection<PMGalleryItem>("galleryItems");
                var item = items.FindById(id);
                if (null != item)
                {
                    item.IsFeatured = !item.IsFeatured;
                    items.Update(item);
                }
                return Adapter.ToViewModel(item);
            }
        }

        #endregion
    }
}