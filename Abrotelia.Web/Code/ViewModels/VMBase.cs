using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Abrotelia.Web.Code.ViewModels
{
    public abstract class VMBase
    {

        #region Fields

        protected readonly int m_ronOptimalMetaDescriptionLength = 160;

        #endregion

        #region Properties

        public string Id { get; set; }
        public string Author { get; set; }
        public DateTime Updated { get; set; }

        public string MetaDescription {
            get
            {
                var regexRemoveHTML = new Regex(@"<[^>]*>|\[\@{2}.*\@{2}\]");
                var regexRemoveExtraSpace = new Regex(@"\s{2,}");
                var metaDescription = GetDefaultDescription();
                var description = GetDescription();
                if (!string.IsNullOrWhiteSpace(description))
                {
                    metaDescription = regexRemoveHTML.Replace(HttpUtility.HtmlDecode(description), " ");
                    metaDescription = regexRemoveExtraSpace.Replace(metaDescription, " ").Trim();
                    metaDescription = metaDescription.Substring(0, Math.Min(m_ronOptimalMetaDescriptionLength, metaDescription.Length));
                }
                return metaDescription;
            }
        }

        #endregion

        #region Abstract methods

        protected abstract string GetDefaultDescription();

        protected abstract string GetDescription();

        #endregion

    }
}