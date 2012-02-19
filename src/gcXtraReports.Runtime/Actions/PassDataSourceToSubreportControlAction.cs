using System;
using System.Linq;
using DevExpress.XtraReports.UI;

namespace GeniusCode.XtraReports.Runtime.Actions
{
    public sealed class PassDataSourceToSubreportControlAction : ReportControlActionBase<XRSubreport>
    {
        private readonly Action<XRSubreport, object> _nestedAction;

        public PassDataSourceToSubreportControlAction()
            : this(null)
        {
        }

        public PassDataSourceToSubreportControlAction(Action<XRSubreport, object> nestedAction)
        {
            _nestedAction = nestedAction;
        }

        protected override void PerformAction(XRSubreport control)
        {
            var ds = control.SetDataSourceOnSubreport();

            if (_nestedAction != null)
                _nestedAction(control, ds);
        }
    }
}