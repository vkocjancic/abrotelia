using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abrotelia.Web.Code.Repository
{
    public interface IRepository<T>
    {

        void DeleteById(string id);
        IList<T> GetAll();
        T GetById(string id);
        void Save(T item, bool isNew);

    }
}
