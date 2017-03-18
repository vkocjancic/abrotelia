using Abrotelia.Core.Common;
using Abrotelia.Core.Data.Persistence;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abrotelia.JsonToDb.Extensions
{
    static class BsonDocumentExtension
    {

        #region Static methods

        public static PMPage ToPMPage(this BsonDocument document)
        {
            return new PMPage()
            {
                Author = document["Author"].AsString,
                Content = document["Content"].AsString,
                //FooterCategory = document["FooterCategory"].AsString,
                //HeaderCategory = document["HeaderCategory"].AsString,
                Id = document["_id"].AsString,
                PageStatus = (PageStatus)Enum.Parse(typeof(PageStatus), document["PageStatus"].AsString),
                PermaLink = document["PermaLink"].AsString,
                Title = document["Title"].AsString,
                Updated = document["Updated"].AsDateTime
            };
        }

        public static PMAuthor ToPMAuthor(this BsonDocument document)
        {
            return new PMAuthor()
            {
                Author = document["Author"].AsString,
                Description = document["Description"].AsString,
                FullName = document["FullName"].AsString,
                Id = document["_id"].AsString,
                Image = document["Image"].AsBinary,
                PermaLink = document["PermaLink"].AsString,
                Updated = document["Updated"].AsDateTime,
            };
        }

        public static PMGalleryItem ToPMGalleryItem(this BsonDocument document)
        {
            var produced = document["Produced"].AsInt32;
            return new PMGalleryItem()
            {
                Author = document["Author"].AsString,
                Description = document["Description"].AsString,
                Era = document["Era"].AsString,
                Height = document["Height"].AsInt32,
                Id = document["_id"].AsString,
                ImageId = document["ImageId"].AsGuid,
                IsFeatured = document["IsFeatured"].AsBoolean,
                ItemAuthorId = document["ItemAuthorId"].AsString,
                PermaLink = document["PermaLink"].AsString,
                Price = document["Price"].AsDecimal,
                Produced = (0 == produced) ? null : produced as int?,
                Techniques = document["Techniques"].AsArray.ToArray().Select(t => t.AsString).ToArray(),
                Title = document["Title"].AsString,
                Updated = document["Updated"].AsDateTime,
                Width = document["Width"].AsInt32
            };
        }

        #endregion

    }
}
