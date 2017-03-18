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

        public static VMPages LoadHeaderMenu(IPagesRepository repository)
        {
            var model = new VMPages();
            model.Items = repository.GetAllForHeaderMenu();
            return model;
        }

        public static VMPages LoadFooterMenu(IPagesRepository repository)
        {
            var model = new VMPages();
            model.Items = repository.GetAllForFooterMenu();
            return model;
        }

        #endregion

    }
}