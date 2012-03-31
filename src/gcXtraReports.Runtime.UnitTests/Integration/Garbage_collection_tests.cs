using System;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using Caliburn.Micro;
using DevExpress.XtraReports.UI;
using FluentAssertions;
using GeniusCode.XtraReports.Runtime.Actions;
using GeniusCode.XtraReports.Runtime.Support;
using GeniusCode.XtraReports.Runtime.Tests.Unit;
using GeniusCode.XtraReports.Runtime.UnitTests;
using NUnit.Framework;

namespace GeniusCode.XtraReports.Runtime.Tests.Integration
{
    [TestFixture]
    public class Garbage_collection_tests
    {
        public class WeakReference<T> : WeakReference where T : class
        {
            public WeakReference(T target) : base(target)
            {
            }

            public WeakReference(TestAttribute target, bool trackResurrection) : base(target, trackResurrection)
            {
            }

            protected WeakReference(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }

            public new T Target { 
                get { return base.Target as T; }
                set { base.Target = value; }
            }
        }
        public static class WeakReferenceFactory
        {
            public static WeakReference<T> CreateWeakReference<T>(T target) where T : class
            {
                return new WeakReference<T>(target);
            }
        }

/*        [Test] 
        public void Should_remove_controllers_and_visitor_out_of_scope()
        {

            WeakReference<ReportVisitor> wr;
            WeakReference<ReportController> rwr;
            WeakReference<gcXtraReport> reportwr;
            WeakReference<XtraReport> initialReport_reference;
            
            IEventAggregator e = new EventAggregator();
            using (var report = new XtraReport
                             {
                                 DataSource = new[] { new object() }
                             })
            {

                initialReport_reference = WeakReferenceFactory.CreateWeakReference(report);


                using (var controller = new ReportController(e, report))
                {
                    using (gcXtraReport ouputReport = controller.Print(r => r.ExportToMemory()))
                    {
                        reportwr = WeakReferenceFactory.CreateWeakReference(ouputReport);
                    }
                    wr = WeakReferenceFactory.CreateWeakReference(controller.Visitors.First().Value);
                    rwr = WeakReferenceFactory.CreateWeakReference(controller);

                }
            }


            GC.Collect();
            initialReport_reference.IsAlive.Should().BeFalse();
            wr.IsAlive.Should().BeFalse();
            
            // controller should not be alive
            rwr.IsAlive.Should().BeFalse();
            reportwr.IsAlive.Should().BeFalse();
            
            
        }*/
    }
}