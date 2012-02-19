using System.IO;
using System.Linq;
using DevExpress.XtraReports.UI;

namespace GeniusCode.XtraReports.Runtime.UnitTests
{
    public static class Extensions
    {
        public static void ExportToMemory(this XtraReport report)
        {
            report.ExportToHtml(new MemoryStream());
        }
    }
}