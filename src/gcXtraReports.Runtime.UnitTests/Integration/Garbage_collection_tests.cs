using System.Drawing;
using System.Linq;
using Caliburn.Micro;
using DevExpress.XtraReports.UI;
using FluentAssertions;
using GeniusCode.XtraReports.Runtime.Actions;
using GeniusCode.XtraReports.Runtime.Tests.Unit;
using GeniusCode.XtraReports.Runtime.UnitTests;
using NUnit.Framework;

namespace GeniusCode.XtraReports.Runtime.Tests.Integration
{
    [TestFixture]
    public class Garbage_collection_tests
    {
        /// <summary>
        /// This test has been used to target an occurance of premature garbage collection!
        /// </summary>
        [Test]
        public void Should_survive_garbage_collection()
        {
            //Print a report using a controller
            var color = Color.Green;
            var action = new ReportControlAction<XtraReport>(r => true, r => r.BackColor = color);

            var report0 = new XtraReport();

            var c = new ReportController(new EventAggregator(), report0, new ReportControlActionFacade(action));

            var newreport = c.Print(r => r.ExportToMemory());
            newreport.BackColor.Should().Be(color);


            // print a secondary report
            var counter = 0;
            var subReport = new XtraReport();
            var container = new XRSubreport { ReportSource = subReport };

            var detailBand = new DetailBand();
            detailBand.Controls.Add(container);

            var report = new XtraReport();
            report.Bands.Add(detailBand);

            report.DataSource = new[]
                                    {
                                        new object(),
                                        new object(),
                                        new object(),
                                        new object()
                                    };

            var controller = new DataSourceTrackingController(new EventAggregator(), report, (s, ds) => counter++);

            var report4 = controller.Print(r => r.ExportToMemory());
            counter.Should().Be(4);


        }        
    }
}