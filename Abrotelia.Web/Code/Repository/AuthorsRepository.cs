using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abrotelia.Web.Code.ViewModels;
using Abrotelia.Web.Code.Adapter;
using LiteDB;
using Abrotelia.Core.Data.Persistence;

namespace Abrotelia.Web.Code.Repository
{
    public class AuthorsRepository : BaseRepository, IAuthorsRepository
    {

        #region Properties

        protected IModelAdapter<PMAuthor, VMAuthor> Adapter => new AuthorModelAdapter();

        #endregion

        #region IAuthorsRepository implementation

        public void DeleteById(string id)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                db.GetCollection<PMAuthor>("authors").Delete(id);
            }
        }

        public IList<VMAuthor> GetAll()
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var model = new List<VMAuthor>();
                foreach (var author in db.GetCollection<PMAuthor>("authors").FindAll())
                {
                    model.Add(Adapter.ToViewModel(author));
                }
                return model;
            }
        }

        public VMAuthor GetById(string id)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var author = db.GetCollection<PMAuthor>("authors").FindById(id);
                return (null == author) ? null : Adapter.ToViewModel(author);
            }
        }

        public void Save(VMAuthor item, bool isNew)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var authors = db.GetCollection<PMAuthor>("authors");
                var docAuthor = Adapter.ToPersistenceModel(item);
                if (isNew)
                {
                    authors.Insert(docAuthor);
                }
                else
                {
                    authors.Update(docAuthor);
                }
            }
        }

        public IList<VMAuthor> Search(string query)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var model = new List<VMAuthor>();
                foreach (var author in db.GetCollection<PMAuthor>("authors").Find(Query.Contains("FullName", query)))
                {
                    model.Add(Adapter.ToViewModel(author));
                }
                return model;
            }
        }

        public IList<VMAuthor> SearchTypeahead(string query)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var model = new List<VMAuthor>();
                foreach (var author in db.GetCollection<PMAuthor>("authors").Find(
                    Query.Contains("FullName", query), 
                    limit: 10))
                {
                    model.Add(Adapter.ToViewModel(author));
                }
                return model;
            }
        }

        #endregion
    }
}