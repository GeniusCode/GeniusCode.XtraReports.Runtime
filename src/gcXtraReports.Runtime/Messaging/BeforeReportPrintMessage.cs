using System.Linq;
using System.Drawing.Printing;
using GeniusCode.XtraReports.Runtime.Support;

namespace GeniusCode.XtraReports.Runtime.Messaging
{
    public class BeforeReportPrintMessage
    {
        public gcXtraReport Report { get; private set; }
        public PrintEventArgs PrintArgs { get; private set; }

        public BeforeReportPrintMessage(gcXtraReport report, PrintEventArgs e)
        {
            Report = report;
            PrintArgs = e;
        }
    }
}