using Abrotelia.Web.Code.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abrotelia.Web.Code.Repository
{
    public interface IAuthorsRepository : IRepository<VMAuthor>
    {

        IList<VMAuthor> SearchTypeahead(string query);
        IList<VMAuthor> Search(string query);

    }
}
