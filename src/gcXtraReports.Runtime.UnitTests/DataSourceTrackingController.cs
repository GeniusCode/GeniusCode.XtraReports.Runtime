using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraReports.UI;
using GeniusCode.XtraReports.Runtime.Actions;
using GeniusCode.XtraReports.Runtime.Support;

namespace GeniusCode.XtraReports.Runtime.UnitTests
{
    public class DataSourceTrackingController : ReportController
    {
        private readonly Action<XRSubreport, Object> _increment;

        public DataSourceTrackingController(XtraReport view, Action<XRSubreport,Object> increment)
            : base(view)
        {
            _increment = increment;
        }

        protected override IEnumerable<IReportControlAction> OnGetDefautActions()
        {
            yield return new PassDataSourceToSubreportControlAction((s, o) => _increment(s,o));
        }

        public void TestPrint()
        {
            Print(r => r.ExportToMemory());
        }
    }
}