using Abrotelia.Core.Data.Persistence;
using Abrotelia.Web.Code.Adapter;
using Abrotelia.Web.Code.ViewModels;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.Repository
{
    public class PagesRepository : BaseRepository, IPagesRepository
    {

        #region Properties

        protected IModelAdapter<PMPage, VMPage> Adapter => new PageModelAdapter();

        #endregion

        #region Public methods

        public void DeleteById(string id)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                db.GetCollection<PMPage>("pages").Delete(id);
            }
        }

        public IList<VMPage> GetAll()
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var model = new List<VMPage>();
                foreach (var page in db.GetCollection<PMPage>("pages").FindAll())
                {
                    model.Add(Adapter.ToViewModel(page));
                }
                return model;
            }
        }

        public VMPage GetById(string id)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var page = db.GetCollection<PMPage>("pages").FindById(id);
                return (null == page) ? null : Adapter.ToViewModel(page);
            }
        }

        public VMPage GetBySlug(string slug)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var page = db.GetCollection<PMPage>("pages").FindOne(Query.EQ("PermaLink", slug));
                return (null == page) ? null : Adapter.ToViewModel(page);
            }
        }

        public void Save(VMPage item, bool isNew)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var pages = db.GetCollection<PMPage>("pages");
                var docPage = Adapter.ToPersistenceModel(item);
                if (isNew)
                {
                    pages.Insert(docPage);
                }
                else
                {
                    pages.Update(docPage);
                }
            }
        }

        #endregion

    }
}