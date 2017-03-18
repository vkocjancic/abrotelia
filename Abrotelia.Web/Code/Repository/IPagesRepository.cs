using Abrotelia.Web.Code.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abrotelia.Web.Code.Repository
{
    public interface IPagesRepository : IRepository<VMPage>
    {
        VMPage GetBySlug(string slug);
        IList<VMPage> GetAllForHeaderMenu();
        IList<VMPage> GetAllForFooterMenu();

    }
}
