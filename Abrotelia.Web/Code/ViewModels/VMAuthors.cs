using Abrotelia.Web.Code.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.ViewModels
{
    public class VMAuthors : CollectionBase<VMAuthor>
    {

        #region Factory methods

        public static VMAuthors Load(IAuthorsRepository repository)
        {
            var model = new VMAuthors();
            model.Items = repository.GetAll().OrderBy(i => i.FullName).ToList();
            return model;
        }

        #endregion

    }
}