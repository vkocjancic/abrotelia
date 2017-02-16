using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Abrotelia.Web.Code.Handlers
{
    public abstract class HttpHandlerBase : IHttpHandler
    {

        #region Fields

        protected Logger m_log;

        #endregion

        #region Constructors

        public HttpHandlerBase(Type classType)
        {
            m_log = LogManager.GetLogger(classType.FullName);
        }

        #endregion

        #region IHttpHandler implementation

        public virtual bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public abstract void ProcessRequest(HttpContext context);

        #endregion


    }
}