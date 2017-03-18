using Abrotelia.Web.Code.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.ViewModels
{
    public class VMAuthor : VMBase
    {

        #region Properties

        public string FullName { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public string PermaLink { get; set; }
        public string Surname {
            get
            {
                var nameTokens = FullName.Split(' ');
                var surname = nameTokens[nameTokens.Length - 1];
                if (char.IsLower(surname[0]))
                {
                    surname = nameTokens[nameTokens.Length - 2];
                }
                return surname;
            }
        }

        #endregion

        #region Factory methods

        public static VMAuthor Load(string authorId, IAuthorsRepository repository)
        {
            return repository.GetById(authorId);
        }

        #endregion

        #region Public methods

        public void Save(IAuthorsRepository repository, bool isNew)
        {
            repository.Save(this, isNew);
        }

        #endregion

        #region Overridden methods

        protected override string GetDefaultDescription()
        {
            return FullName;
        }

        protected override string GetDescription()
        {
            return Description;
        }

        #endregion

    }
}