using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using DevExpress.XtraReports.UI;
using GeniusCode.XtraReports.Runtime.Actions;
using GeniusCode.XtraReports.Runtime.Messaging;
using GeniusCode.XtraReports.Runtime.Support;

namespace GeniusCode.XtraReports.Runtime
{
    public class ReportController : IReportController, IHandle<ScopedControlBeforePrintMessage>, IHandle<BeforeReportPrintMessage>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly XtraReport _view;
        private readonly IReportControlActionFacade _injectedFacade;

        public ReportController(IEventAggregator eventAggregator, XtraReport view, IReportControlActionFacade injectedFacade = null)
        {
            Visitors = new Dictionary<int, WeakReference>();
            _eventAggregator = eventAggregator;
            _view = view;
            _injectedFacade = injectedFacade;
            
            _additionalActions = new List<IReportControlAction>();
            _facades = new Lazy<IEnumerable<IReportControlActionFacade>>(BuildActionFacades);
        }

        protected virtual IEnumerable<IReportControlAction> OnGetDefautActions()
        {
            yield return new PassDataSourceToSubreportControlAction();
        }

        protected virtual void OnRegisterAdditionalActions()
        {
        }

        private readonly List<IReportControlAction> _additionalActions;

        protected void RegisterActionForControl<T>(Action<T> toDo) where T : XRControl
        {
            var action = ReportControlAction<T>.WithNoPredicate(toDo);
            _additionalActions.Add(action);
        }

        private gcXtraReport _printingReport;

        private IEnumerable<IReportControlActionFacade> BuildActionFacades()
        {
            var output = new List<IReportControlActionFacade>(3);

            //FIRST: Add Default Actions
            var defaultActions = OnGetDefautActions();
            var defaultFacade = new ReportControlActionFacade(defaultActions.ToArray());
            output.Add(defaultFacade);

            //SECOND: Add Additional Actions
            OnRegisterAdditionalActions();
            var additionalFacade = new ReportControlActionFacade(_additionalActions.ToArray());
            output.Add(additionalFacade);
            
            //THIRD: Add Injected Actions
            if(_injectedFacade != null)
                output.Add(_injectedFacade);

            return output;
        }



        public gcXtraReport Print(Action<gcXtraReport> printAction)
        {                 
            _printingReport = _view.ConvertReportToMyReportBase(_eventAggregator);
            _printingReport.RuntimeRootReportHashCode = _printingReport.GetHashCode();

            _eventAggregator.Subscribe(this);

            printAction(_printingReport);
            return _printingReport;
        }



        public void Handle(ScopedControlBeforePrintMessage message)
        {
            if (!ShouldApplyMessage(message)) return;

            var facades = _facades.Value;

            foreach (var reportControlActionFacade in facades)
            {
                reportControlActionFacade.AttemptActionsOnControl(message.Control);
            }
        }


        private bool ShouldApplyMessage(ScopedControlBeforePrintMessage message)
        {

            if (_printingReport == null)
                return false;

            return message.RootReportHashcode == _printingReport.RuntimeRootReportHashCode;
        }



        private bool ShouldApplyMessage(BeforeReportPrintMessage message)
        {
            if (_printingReport == null)
                return false;

            return message.Report.RuntimeRootReportHashCode == _printingReport.RuntimeRootReportHashCode;
        }

        private void VisitMethodRecursively(BeforeReportPrintMessage message)
        {
            var incomingHashcode = message.Report.GetHashCode();

            // prevent multiple visitors on the same report.
            // a report can print multiple times if it is a subreport
            if (Visitors.ContainsKey(incomingHashcode)) return;

            using (var visitor = new ReportVisitor(_eventAggregator, message.Report))
            {
                Visitors.Add(incomingHashcode, new WeakReference(visitor));
                visitor.Visit();
            }           
        }

        public readonly Dictionary<int, WeakReference> Visitors;
        private Lazy<IEnumerable<IReportControlActionFacade>> _facades;

        public void Handle(BeforeReportPrintMessage message)
        {
            if (!ShouldApplyMessage(message)) return;
            VisitMethodRecursively(message);
        }
    }
}