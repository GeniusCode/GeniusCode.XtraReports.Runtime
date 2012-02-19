using System;
using System.Linq;
using DevExpress.XtraReports.UI;
using FluentAssertions;
using GeniusCode.XtraReports.Runtime.Actions;
using GeniusCode.XtraReports.Runtime.Support;
using NUnit.Framework;

namespace GeniusCode.XtraReports.Runtime.UnitTests
{
    [TestFixture]
    public class ApiTweaks
    {
        [Test]
        public void Should_pass_root_hashcode()
        {
            var view = new XtraReport {DataSource = new[] {new object(), new object()}};

            var detailBand = new DetailBand();
            var container = new XRSubreport();
            var subReport = new gcXtraReport();

            container.ReportSource = subReport;
            detailBand.Controls.Add(container);
            view.Bands.Add(detailBand);

            IReportController myController = new ReportController(view);
            Action<XtraReport> printAction = r => r.ExportToMemory();
            var newView = myController.Print(printAction);

            var subReportsHashcode =
                ((gcXtraReport) ((XRSubreport) newView.Bands[BandKind.Detail].Controls[0]).ReportSource).RuntimeRootReportHashCode;

            newView.RuntimeRootReportHashCode.Should().NotBe(0);

            subReportsHashcode.Should().Be(newView.RuntimeRootReportHashCode);

        }

        [Test]
        public void Should_not_collide_with_two_controllers()
        {
            var view = new XtraReport {DataSource = new[] {new object(), new object()}};

            var counterA = 0;
            var counterB = 0;

            var actionA = ReportControlAction<XRControl>.WithNoPredicate(c => counterA++);
            var actionB = ReportControlAction<XRControl>.WithNoPredicate(c => counterB++);
            var facadeA = new ReportControlActionFacade(actionA);
            var facadeB = new ReportControlActionFacade(actionB);

            var controllerA = new ReportController(view, facadeA);
            var controllerB = new ReportController(view, facadeB);

            controllerA.Print(r => r.ExportToMemory());
            controllerB.Print(r => r.ExportToMemory());

            counterA.Should().Be(1);
            counterB.Should().Be(1);
        }


        //TODO: Run this test manually or rewrite to support batch runs
/*        [Test]
        public void Should_Dispose_visitors() // no memory leaks here!
        {
            var view = new XtraReport {DataSource = new[] {new object(), new object()}};
            var controllerA = new ReportController(view);
            var view2 = controllerA.Print(r => r.ExportToMemory());
            
            GlobalMessageSubscriber.Singleton.Visitors.Values.Count(wr => wr.IsAlive && ((XRRuntimeVisitor)wr.Target).ReportHashcode == view2.RuntimeRootReportHashCode).Should().Be(1);
            GC.Collect();
            GlobalMessageSubscriber.Singleton.Visitors.Values.Count(wr => wr.IsAlive && ((XRRuntimeVisitor)wr.Target).ReportHashcode == view2.RuntimeRootReportHashCode).Should().Be(0);
        }*/
        

    }

}