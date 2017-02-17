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
    public class NewsSubscriptionsRepository : BaseRepository, INewsSubscriptionsRepository
    {

        #region Fields

        protected readonly string m_rosCollectionName = "newsSubscriptions";

        #endregion

        #region Properties

        protected IModelAdapter<PMNewsSubscription, VMNewsSubscription> Adapter => new NewsSubscriptionModelAdapter();

        #endregion

        #region INewsRepository implementation

        public void DeleteById(string id)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                db.GetCollection<PMNewsSubscription>(m_rosCollectionName).Delete(id);
            }
        }

        public IList<VMNewsSubscription> GetAll()
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var model = new List<VMNewsSubscription>();
                foreach (var item in db.GetCollection<PMNewsSubscription>(m_rosCollectionName).FindAll())
                {
                    model.Add(Adapter.ToViewModel(item));
                }
                return model;
            }
        }

        public VMNewsSubscription GetByEmail(string email)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var subscription = db.GetCollection<PMNewsSubscription>(m_rosCollectionName).FindOne(Query.EQ("Email", email));
                return (null == subscription) ? null : Adapter.ToViewModel(subscription);
            }
        }

        public VMNewsSubscription GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void Save(VMNewsSubscription item, bool isNew)
        {
            using (var db = new LiteDatabase(DatabaseName))
            {
                var subscriptions = db.GetCollection<PMNewsSubscription>(m_rosCollectionName);
                var docSubscription = Adapter.ToPersistenceModel(item);
                if (isNew)
                {
                    subscriptions.Insert(docSubscription);
                }
                else
                {
                    // do nothing
                }
            }
        } 

        #endregion
    }
}