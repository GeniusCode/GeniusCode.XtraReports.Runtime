using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using DevExpress.XtraReports.UI;
using GeniusCode.XtraReports.Runtime.Actions;
using GeniusCode.XtraReports.Runtime.Messaging;

namespace GeniusCode.XtraReports.Runtime.Support
{
    public class ReportController : IReportController, IHandle<ScopedControlBeforePrintMessage>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly XtraReport _view;
        private readonly IReportControlActionFacade _injectedFacade;

        public ReportController(IEventAggregator eventAggregator, XtraReport view, IReportControlActionFacade injectedFacade = null)
        {
            _eventAggregator = eventAggregator;
            _view = view;
            _injectedFacade = injectedFacade;
            GlobalMessageSubscriber.Init();
            _eventAggregator.Subscribe(this);
        }

        protected virtual IEnumerable<IReportControlAction> OnGetDefautActions()
        {
            yield return new PassDataSourceToSubreportControlAction();
        }

        protected virtual void OnRegisterAdditionalActions()
        {
        }

        private List<IReportControlAction> _additionalActions;

        protected void RegisterFor<T>(Action<T> toDo) where T : XRControl
        {
            var action = ReportControlAction<T>.WithNoPredicate(toDo);
            _additionalActions.Add(action);
        }

        private gcXtraReport _printingReport;
        private IReportControlActionFacade[] _actionFacades;

        private IReportControlActionFacade[] BuildActionFacades()
        {
            var defaultActions = OnGetDefautActions();
            var defaultFacade = new ReportControlActionFacade(defaultActions.ToArray());

            OnRegisterAdditionalActions();
            _additionalActions

        }



        public gcXtraReport Print(Action<gcXtraReport> printAction)
        {           

         

            _additionalActionsFacade = new ReportControlActionFacade(_additionalActions.ToArray());

            _printingReport = _view.ConvertReportToMyReportBase();
            _printingReport.RuntimeRootReportHashCode = _printingReport.GetHashCode();

            printAction(_printingReport);
            return _printingReport;
        }



        public void Handle(ScopedControlBeforePrintMessage message)
        {
            


            if (_printingReport == null || _printingReport.RuntimeRootReportHashCode != message.RootReportHashcode) return;

            
            
            if (_additionalActionsFacade != null)
            {
                _additionalActionsFacade.AttemptActionsOnControl(message.Control);
            }


            defaultFacade.
                AttemptActionsOnControl(
                    c);
            additionalActionsFacade.
                AttemptActionsOnControl(
                    c);

            if (_injectedFacade != null)
                _injectedFacade.
                    AttemptActionsOnControl
                    (c);
        }
    }
}