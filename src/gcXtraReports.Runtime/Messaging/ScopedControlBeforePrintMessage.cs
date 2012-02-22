using System;
using System.Linq;
using DevExpress.XtraReports.UI;

namespace GeniusCode.XtraReports.Runtime.Messaging
{
    public class ScopedControlBeforePrintMessage
    {
        public Guid RootReportGuid { get; private set; }
        public XRControl Control { get; private set; }

        public ScopedControlBeforePrintMessage(Guid rootReportGuid, XRControl control)
        {
            RootReportGuid = rootReportGuid;

            Control = control;
        }
    }
}