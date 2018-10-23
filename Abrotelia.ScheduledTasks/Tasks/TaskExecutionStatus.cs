using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Abrotelia.ScheduledTasks.Tasks
{
    public abstract class TaskExecutionStatus
    {

        #region Declarations

        public static TaskExecutionStatus OK = new TaskExecutionStatusOk();
        public static TaskExecutionStatus Error = new TaskExecutionStatusError();

        #endregion

        #region Properties

        public string Message { get; set; }

        #endregion

        #region Constructors

        private TaskExecutionStatus() { }

        #endregion

        #region Abstract methods

        public abstract HttpResponseMessage ToResponse(HttpRequestMessage request);

        #endregion

        #region Implementation classes

        protected class TaskExecutionStatusOk : TaskExecutionStatus
        {

            #region Overridden methods

            public override HttpResponseMessage ToResponse(HttpRequestMessage request)
            {
                return request.CreateResponse(HttpStatusCode.OK);
            }

            #endregion

        }

        protected class TaskExecutionStatusError : TaskExecutionStatus
        {

            #region Overridden methods

            public override HttpResponseMessage ToResponse(HttpRequestMessage request)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, Message);
            }

            #endregion

        }

        #endregion

    }
}