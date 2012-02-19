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

        public void AttemptActionsOnControl(XRControl control)
        {
            // TODO: Add Filter by Whitelist ReportActionName and/or ReportActionGroupName

            // Optimization - ignore XRControls that we don't have ReportActions for
            var foundMatchingRuntimeAction = (from type in _controlTypes
                                              where type.IsInstanceOfType(control)
                                              select type).Any();

            if (foundMatchingRuntimeAction == false)
                return;

            // Predicates
            var actions = from action in _runtimeActions
                          where action.ActionPredicate(control)
                          select action;

            // Execute matching Runtime Actions
            foreach (var action in actions)
                action.ActionToApply.Invoke(control);
        }
    }
}