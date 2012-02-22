using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraReports.UI;
using GeniusCode.XtraReports.Runtime.Actions;

namespace GeniusCode.XtraReports.Runtime.Support
{
    public class ReportController : IReportController, IDisposable
    {
        private readonly XtraReport _view;
        private readonly IReportControlActionFacade _injectedFacade;

        public ScopedMessageSubscriber Subscriber { get; private set; }

        public ReportController(XtraReport view, IReportControlActionFacade injectedFacade = null)
        {
            _view = view;
            _injectedFacade = injectedFacade;
            GlobalMessageSubscriber.Init();
        }

        protected virtual IEnumerable<IReportControlAction> OnGetDefautActions()
        {
            yield return new PassDataSourceToSubreportControlAction();
        }

        protected virtual void OnRegisterAdditionalActions()
        {
        }

        private List<IReportControlAction> _toDos;

        protected void RegisterFor<T>(Action<T> toDo) where T : XRControl
        {
            var action = ReportControlAction<T>.WithNoPredicate(toDo);
            _toDos.Add(action);
        }

        public gcXtraReport Print(Action<gcXtraReport> printAction)
        {
            var actions = OnGetDefautActions();
            var defaultFacade = new ReportControlActionFacade(actions.ToArray());

            _toDos = new List<IReportControlAction>();
            OnRegisterAdditionalActions();
            var additionalActionsFacade = new ReportControlActionFacade(_toDos.ToArray());

            var newView = _view.ConvertReportToMyReportBase();
            newView.RuntimeRootReportHashCode = newView.GetHashCode();
            Subscriber = new ScopedMessageSubscriber(newView.RuntimeRootReportHashCode, c =>
                                                                                        {
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
                                                                                        });
            printAction(newView);
            return newView;
        }

        public void Dispose()
        {
            Subscriber.Dispose();
        }
    }
}