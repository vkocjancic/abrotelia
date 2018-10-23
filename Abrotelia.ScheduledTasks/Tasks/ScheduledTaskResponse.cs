using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.ScheduledTasks.Tasks
{
    public class ScheduledTaskResponse
    {

        #region Properties

        public TaskExecutionStatus Status { get; set; } = TaskExecutionStatus.OK;

        #endregion

    }
}