using System.Linq;
using System;
using DevExpress.XtraReports.UI;

namespace GeniusCode.XtraReports.Runtime.Actions
{
    public sealed class ReportControlAction<T> : ReportControlActionBase<T> where T : XRControl
    {
        private readonly Func<T, bool> _predicate;
        private readonly Action<T> _action;

        public static ReportControlAction<T> WithNoPredicate(Action<T> action)
        {
            return new ReportControlAction<T>(a => true, action);
        }

        public ReportControlAction(Func<T, bool> predicate, Action<T> action)
        {
            _predicate = predicate;
            _action = action;
        }

        protected override bool ReturnShouldApplyAction(T control)
        {
            return _predicate(control);
        }

        protected override void PerformAction(T control)
        {
            _action(control);
        }
    }
}