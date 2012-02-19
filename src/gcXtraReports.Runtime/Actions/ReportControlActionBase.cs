using System;
using System.Linq;
using DevExpress.XtraReports.UI;

namespace GeniusCode.XtraReports.Runtime.Actions
{
    public abstract class ReportControlActionBase<T> : ReportControlActionBase, IReportControlAction<T>
        where T : XRControl
    {
        protected override sealed bool ReturnShouldApplyAction(XRControl control)
        {
            return ReturnShouldApplyAction((T) control);
        }

        protected virtual bool ReturnShouldApplyAction(T control)
        {
            return true;
        }

        Func<T, bool> IReportControlAction<T>.Predicate
        {
            get { return ReturnShouldApplyAction; }
        }

        protected override sealed void PerformAction(XRControl control)
        {
            PerformAction((T) control);
        }

        protected abstract void PerformAction(T control);

        Action<T> IReportControlAction<T>.ActionToApply
        {
            get { return PerformAction; }
        }

        protected override sealed Type GetActionTargetType()
        {
            return typeof (T);
        }
    }

    public abstract class ReportControlActionBase : IReportControlAction
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        protected virtual bool ReturnShouldApplyAction(XRControl control)
        {
            return true;
        }

        Func<XRControl, bool> IReportControlAction.ActionPredicate
        {
            get { return ReturnShouldApplyAction; }
        }

        protected abstract void PerformAction(XRControl control);

        public Action<XRControl> ActionToApply
        {
            get { return PerformAction; }
        }

        protected abstract Type GetActionTargetType();

        public Type ApplyToControlType
        {
            get { return GetActionTargetType(); }
        }
    }
}