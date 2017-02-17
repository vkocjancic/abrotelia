using Abrotelia.Core.Common;
using Abrotelia.Web.Code.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.ViewModels
{
    public class VMPage : VMBase
    {

        #region Properties

        public string Content { get; set; }
        public string FooterCategory { get; set; }
        public string HeaderCategory { get; set; }
        public PageStatus PageStatus { get; set; }
        public string Title { get; set; }

        #endregion

        #region Factory methods

        public static VMPage Load(string pageId, IPagesRepository repository)
        {
            return repository.GetById(pageId);
        }

        #endregion

        #region Public methods

        public void Save(IPagesRepository repository, bool isNew)
        {
            repository.Save(this, isNew);
        }

        #endregion

        #region Overridden methods

        protected override string GetDefaultDescription()
        {
            return Title;
        }

        protected override string GetDescription()
        {
            return Content;
        }

        #endregion

    }
}