using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.Adapter
{
    public interface IModelAdapter<TPersistenceModel, TViewModel>
    {

        TPersistenceModel ToPersistenceModel(TViewModel modelView);
        TViewModel ToViewModel(TPersistenceModel modelPersistence);

    }
}