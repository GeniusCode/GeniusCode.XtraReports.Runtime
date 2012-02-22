using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using DevExpress.XtraReports.UI;
using FluentAssertions;
using GeniusCode.XtraReports.Runtime.Actions;
using GeniusCode.XtraReports.Runtime.Support;
using GeniusCode.XtraReports.Runtime.UnitTests;
using NUnit.Framework;

namespace GeniusCode.XtraReports.Runtime.Tests.Unit
{
    [TestFixture]
    public class Report_controller_tests
    {
        public class Controller_with_default_action_to_change_text : ReportController
        {
            public Controller_with_default_action_to_change_text(IEventAggregator eventAggregator, XtraReport view, IReportControlActionFacade injectedFacade) : base(eventAggregator, view, injectedFacade)
            {
            }

            protected override IEnumerable<IReportControlAction> OnGetDefautActions()
            {               
                var toReturn = new List<IReportControlAction>(base.OnGetDefautActions())
                                   {ReportControlAction<XRLabel>.WithNoPredicate(l => l.Text = "TextChanged")};

                return toReturn;
            }
        }

        public class Controller_with_protected_action_to_change_text : ReportController
        {
            public Controller_with_protected_action_to_change_text(IEventAggregator eventAggregator, XtraReport view, IReportControlActionFacade injectedFacade)
                : base(eventAggregator, view, injectedFacade)
            {
            }

            protected override void OnRegisterAdditionalActions()
            {
                RegisterActionForControl<XRLabel>(l => l.Text = "TextChanged");
            }

        }

        [Test]
        public void Should_fire_default_actions()
        {
            var aggregator = new EventAggregator();
            var view = new XtraReport { DataSource = new[] { new object(), new object() } };

            view.Bands.Add(new DetailBand());

            view.Bands[0].Controls.Add(new XRLabel {Text = "Original Text"});

            var controller = new Controller_with_default_action_to_change_text(aggregator, view,null);
            
            var newReport = controller.Print(p => p.ExportToMemory());
            newReport.Bands[0].Controls[0].Text.Should().Be("TextChanged");
        }

        [Test]
        public void Should_fire_injected_actions()
        {
            var aggregator = new EventAggregator();
            var view = new XtraReport { DataSource = new[] { new object(), new object() } };

            view.Bands.Add(new DetailBand());

            view.Bands[0].Controls.Add(new XRLabel { Text = "Original Text" });

            var action = ReportControlAction<XRLabel>.WithNoPredicate(l => l.Text = "TextChanged");
            var controller = new ReportController(aggregator, view,new ReportControlActionFacade(action));

            var newReport = controller.Print(p => p.ExportToMemory());

            newReport.Bands[0].Controls[0].Text.Should().Be("TextChanged");
        }

        [Test]
        public void Should_fire_protected_actions()
        {
            var aggregator = new EventAggregator();
            var view = new XtraReport { DataSource = new[] { new object(), new object() } };

            view.Bands.Add(new DetailBand());

            view.Bands[0].Controls.Add(new XRLabel { Text = "Original Text" });

            var controller = new Controller_with_protected_action_to_change_text(aggregator, view, null);

            var newReport = controller.Print(p => p.ExportToMemory());
            newReport.Bands[0].Controls[0].Text.Should().Be("TextChanged");
        }

        [Test]
        public void Should_not_collide_with_another_controller_when_both_printing_using_save_event_aggregator()
        {
            var view = new XtraReport { DataSource = new[] { new object(), new object() } };

            var counterA = 0;
            var counterB = 0;

            IEventAggregator aggregator = new EventAggregator();

            var actionA = ReportControlAction<XRControl>.WithNoPredicate(c => counterA++);
            var actionB = ReportControlAction<XRControl>.WithNoPredicate(c => counterB++);
            var facadeA = new ReportControlActionFacade(actionA);
            var facadeB = new ReportControlActionFacade(actionB);

            var controllerA = new ReportController(aggregator, view, facadeA);
            var controllerB = new ReportController(aggregator, view, facadeB);

            controllerA.Print(r => r.ExportToMemory());
            controllerB.Print(r => r.ExportToMemory());

            counterA.Should().Be(1);
            counterB.Should().Be(1);
        }




    }
}