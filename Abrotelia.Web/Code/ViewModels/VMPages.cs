using Abrotelia.Web.Code.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.ViewModels
{
    public class VMPages : CollectionBase<VMPage>
    {

        #region Factory methods

        public static VMPages Load(IPagesRepository repository)
        {
            var model = new VMPages();
            model.Items = repository.GetAll();
            return model;
        }

        #endregion

    }
}