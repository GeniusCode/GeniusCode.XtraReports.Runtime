using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Caliburn.Micro;
using DevExpress.XtraReports.UI;
using FluentAssertions;
using GeniusCode.XtraReports.Runtime.Actions;
using GeniusCode.XtraReports.Runtime.Support;
using GeniusCode.XtraReports.Runtime.Tests.Unit;
using GeniusCode.XtraReports.Runtime.UnitTests;
using GeniusCode.XtraReports.Tests.Models;
using GeniusCode.XtraReports.Tests.Reports;
using NUnit.Framework;

namespace GeniusCode.XtraReports.Runtime.Tests.Integration
{
    [TestFixture]
    public class General_scenarios
    {
        [Test]
        public void Should_apply_actions_to_entire_report()
        {
            var color = Color.Green;
            var action = new ReportControlAction<XtraReport>(r => true, r => r.BackColor = color);

            var report = new XtraReport();
            var newReport = new ReportController(new EventAggregator(), report, new ReportControlActionFacade(action)).Print(r => r.ExportToMemory());

            Assert.AreEqual(color, newReport.BackColor);
        }

        [Test]
        public void Should_fire_actions_on_table_members()
        {
            var transformColor = Color.Blue;
            var action = new ReportControlAction<XRControl>(c => true, c => c.BackColor = transformColor);

            var table = new XRTable();
            var row = new XRTableRow();
            var cell = new XRTableCell();
            row.Cells.Add(cell);
            table.Rows.Add(row);

            var report = new XtraReport();
            report.Bands.Add(new DetailBand());
            report.Bands[0].Controls.Add(table);

            //var subscriber = XRRuntimeSubscriber.SubscribeWithActions(action);
            var reportb = new ReportController(new EventAggregator(), report, new ReportControlActionFacade(action)).Print(r => r.ExportToMemory());

            var tableB = (XRTable)reportb.Bands[0].Controls[0];
            var rowB = tableB.Rows[0];
            var cellb = rowB.Cells[0];




            Assert.AreEqual(transformColor, cellb.BackColor);
        }

        [Test]
        public void Should_convert_subreport_to_gc_xtra_report()
        {
            var report = new XtraReport();
            var detailBand = new DetailBand();
            var subReportContainer = new XRSubreport { ReportSource = new XtraReport() };
            report.Bands.Add(detailBand);
            detailBand.Controls.Add(subReportContainer);


            var controller = new ReportController(new EventAggregator(), report);
            var newReport = controller.Print(p => p.ExportToMemory());

            var newContainer = (XRSubreport)newReport.Bands[0].Controls[0];
            newContainer.ReportSource.GetType().Should().Be(typeof(gcXtraReport));
        }


        [Test]
        public void Handler_wireup_should_be_predicatable()
        {
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
            controller.Print(r => r.ExportToMemory());
            counter.Should().Be(4);
        }

        [Test]
        public void Should_handle_detail_reports_with_subreport()
        {
            var textvalues = new List<Tuple<int, string>>();
            var report = new XtraReportWithSubReportInDetailReport();
            report.DataSource = new List<Person2>
                                    {
                                        new Person2
                                            {
                                                Name = "Douglas Sam",
                                                Age = 17,
                                                Dogs = new List<Dog> {new Dog {Name = "Rex"}, new Dog {Name = "Rudy"}}
                                            },
                                        new Person2
                                            {
                                                Name = "Fred Thomas",
                                                Age = 35,
                                                Dogs =
                                                    new List<Dog> {new Dog {Name = "Sally"}, new Dog {Name = "Stubert"}}
                                            },
                                        new Person2
                                            {
                                                Name = "Alex Matthew",
                                                Age = 100,
                                                Dogs =
                                                    new List<Dog>
                                                        {new Dog {Name = "Nibbles"}, new Dog {Name = "Norbert"}}
                                            }

                                    };
            int counter = 0;
            var action = ReportControlAction<XRLabel>.WithNoPredicate(l =>
            {
                counter++;
                textvalues.Add(new Tuple<int, string>(l.Report.GetHashCode(), l.Text));
            });
            var facade = new ReportControlActionFacade(action);

            var c = new ReportController(new EventAggregator(), report, facade);
            var newReport = c.Print(a => a.ExportToMemory());
            //Not safe for batch test runs GlobalMessageSubscriber.Singleton.Visitors.Count.Should().Be(2);
            //not safe for batchess counter.Should().Be(6);


        }

        [Test]
        public void Should_handle_detail_reports()
        {
            var textvalues = new List<Tuple<int, string>>();
            var report = new XtraReportWithLabelInDetailReport();
            report.DataSource = new List<Person2>
                                    {
                                        new Person2
                                            {
                                                Name = "Douglas Sam",
                                                Age = 17,
                                                Dogs = new List<Dog> {new Dog {Name = "Rex"}, new Dog {Name = "Rudy"}}
                                            },
                                        new Person2
                                            {
                                                Name = "Fred Thomas",
                                                Age = 35,
                                                Dogs =
                                                    new List<Dog> {new Dog {Name = "Sally"}, new Dog {Name = "Stubert"}}
                                            },
                                        new Person2
                                            {
                                                Name = "Alex Matthew",
                                                Age = 100,
                                                Dogs =
                                                    new List<Dog>
                                                        {new Dog {Name = "Nibbles"}, new Dog {Name = "Norbert"}}
                                            }

                                    };
            int counter = 0;
            var action = ReportControlAction<XRLabel>.WithNoPredicate(l =>
            {
                counter++;
                textvalues.Add(new Tuple<int, string>(l.Report.GetHashCode(), l.Text));
            });
            var facade = new ReportControlActionFacade(action);

            var c = new ReportController(new EventAggregator(), report, facade);
            var newReport = c.Print(a => a.ExportToMemory());

            counter.Should().Be(6);


        }

        [Test]
        public void XtraReport_should_fire_event_properly()
        {
            var report = new XtraReportWithSubReportInDetailReport();
            report.DataSource = new List<Person2>
                                    {
                                        new Person2
                                            {
                                                Name = "Douglas Sam",
                                                Age = 17,
                                                Dogs = new List<Dog> {new Dog {Name = "Rex"}, new Dog {Name = "Rudy"}}
                                            },
                                        new Person2
                                            {
                                                Name = "Fred Thomas",
                                                Age = 35,
                                                Dogs =
                                                    new List<Dog> {new Dog {Name = "Sally"}, new Dog {Name = "Stubert"}}
                                            },
                                        new Person2
                                            {
                                                Name = "Alex Matthew",
                                                Age = 100,
                                                Dogs =
                                                    new List<Dog>
                                                        {new Dog {Name = "Nibbles"}, new Dog {Name = "Norbert"}}
                                            }

                                    };
            var detailReportBand = report.Controls.OfType<DetailReportBand>().Single();
            var sr = (XRSubreport)detailReportBand.Bands[BandKind.Detail].Controls[0];
            var label = (XRLabel)sr.ReportSource.Bands[BandKind.Detail].Controls[0];
            var counter = 0;
            label.BeforePrint += (s, o) => counter++;
            report.ExportToMemory();
            counter.Should().Be(6);
        }

    }


}
