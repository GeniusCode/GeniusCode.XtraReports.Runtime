using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using DevExpress.XtraReports.UI;
using GeniusCode.XtraReports.Runtime.Actions;
using GeniusCode.XtraReports.Runtime.UnitTests;

namespace GeniusCode.XtraReports.Runtime.Tests
{
    public class DataSourceTrackingController : ReportController
    {
        private readonly Action<XRSubreport, Object> _nestedActon;


        public DataSourceTrackingController(IEventAggregator eventAggregator, XtraReport view, Action<XRSubreport, object> nestedActon) : base(eventAggregator, view, null)
        {
            _nestedActon = nestedActon;
        }

        protected override IEnumerable<IReportControlAction> OnGetDefautActions()
        {
            yield return new PassDataSourceToSubreportControlAction((s, o) => _nestedActon(s,o));
        }

        public void TestPrint()
        {
            Print(r => r.ExportToMemory());
        }
    }
}