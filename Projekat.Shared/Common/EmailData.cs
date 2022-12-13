using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Shared.Common
{
    public class EmailData
    {
        public string Subject { get; set; }

        public string Content { get; set; }

        public string To { get; set; }

        public bool IsContentHtml { get; set; }

        public List<string> CcList { get; set; }

        public EmailData()
        {
            CcList = new List<string>();
        }
    }
}
