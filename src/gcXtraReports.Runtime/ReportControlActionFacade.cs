using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraReports.UI;

namespace GeniusCode.XtraReports.Runtime.Actions
{
    public class ReportControlActionFacade : IReportControlActionFacade
    {
        private readonly List<IReportControlAction> _runtimeActions;
        private readonly List<Type> _controlTypes;

        public ReportControlActionFacade(IReportControlActionFactory actionProvider)
            : this(actionProvider.GetRuntimeActions().ToArray())
        {
        }

        public ReportControlActionFacade(IEnumerable<IReportControlActionFactory> actionProviders)
            : this(actionProviders.SelectMany(provider => provider.GetRuntimeActions()).ToArray())
        {
        }

        public ReportControlActionFacade(params IReportControlAction[] actions)
        {
            _runtimeActions = new List<IReportControlAction>(actions);

            _controlTypes = (from action in _runtimeActions
                             select action.ApplyToControlType).Distinct().ToList();
        }

        private IEnumerable<IReportControlAction> GetActionsforControl(XRControl control)
        {
            var typeAppropriateActions = from action in _runtimeActions
                                          where action.ApplyToControlType.IsInstanceOfType(control)
                                          select action;

            var predicatedActions = from action in typeAppropriateActions
                                     where action.ActionPredicate(control)
                                     select action;

            return predicatedActions;
        }

        public void AttemptActionsOnControl(XRControl control)
        {
            // TODO: Add Filter by Whitelist ReportActionName and/or ReportActionGroupName

            var actions = GetActionsforControl(control).ToList();
            actions.ForEach(a=> a.ActionToApply.Invoke(control));
        }
    }
}