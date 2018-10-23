using Abrotelia.ScheduledTasks.Tasks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Abrotelia.ScheduledTasks.Controllers
{
    public class BackupController : ApiController
    {
        // GET api/<controller>
        public async Task<HttpResponseMessage> Get()
        {
            var task = new BackupTask(ConfigurationManager.AppSettings["task.backup.location"]);
            return (await task.CreateAsync()).Status.ToResponse(Request);
        }
    }
}