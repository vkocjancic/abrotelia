using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Abrotelia.Web.Code.Common
{
    public class PermalinkGenerator
    {

        #region Static methods

        public static string GetSlug(string textToConvert)
        {
            var regexPermalink = new Regex(@"[^\w]+");
            return regexPermalink.Replace(textToConvert, "-");
        }

        #endregion

    }
}