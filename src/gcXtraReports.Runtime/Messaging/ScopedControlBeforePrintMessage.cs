using System.Linq;
using DevExpress.XtraReports.UI;

namespace GeniusCode.XtraReports.Runtime.Messaging
{
    public class ScopedControlBeforePrintMessage
    {
        public int RootReportHashcode { get; private set; }
        public XRControl Control { get; private set; }

        public ScopedControlBeforePrintMessage(int rootReportHashcode, XRControl control)
        {
            RootReportHashcode = rootReportHashcode;
            Control = control;
        }
    }
}