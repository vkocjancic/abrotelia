using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Abrotelia.ScheduledTasks.Tasks
{
    public class BackupTask
    {

        #region Properties

        protected string Location { get; set; }

        #endregion

        #region Constructors

        public BackupTask(string location)
        {
            Location = location;
        }

        #endregion

        #region Public methods

        public Task<ScheduledTaskResponse> CreateAsync()
        {
            return Task.Run(() =>
            {
                return Create();
            });
        }

        public ScheduledTaskResponse Create()
        {
            var response = new ScheduledTaskResponse();
            var path = System.Web.Hosting.HostingEnvironment.MapPath($"~/{Location}/");
            using (var zipFile = ZipFile.Create($"{path}\\backup\\Abrotelia_{DateTime.Now.ToString("yyyyMMddhhmmss")}.zip"))
            {
                zipFile.BeginUpdate();
                foreach(var file in System.IO.Directory.GetFiles(path, "*.*", System.IO.SearchOption.AllDirectories).Where(f => !f.Contains(@"\backup\")))
                {
                    zipFile.Add(file, file.Replace(path, ""));
                }
                zipFile.CommitUpdate();
            }
            return response;
        }

        #endregion

    }
}