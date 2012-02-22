using System;
using System.Linq;
using DevExpress.XtraReports.UI;
using GeniusCode.XtraReports.Runtime.Support;

namespace GeniusCode.XtraReports.Runtime
{
    public interface IReportController
    {
        gcXtraReport Print(Action<gcXtraReport> printAction);
    }
}