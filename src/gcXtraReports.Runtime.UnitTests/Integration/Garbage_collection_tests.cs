using System;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
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

        [Test]
        public void Should_remove_controllers_and_visitor_out_of_scope()
        {
            IEventAggregator e = new EventAggregator();
            var report = new XtraReport
                             {
                                 DataSource = new[] {new object()}
                             };
            
            var contollerReference = WeakReferenceFactory.CreateWeakReference(new ReportController(e,report));
            var visitors = contollerReference.Target.Visitors;
            
            var report2 = contollerReference.Target.Print(r => r.ExportToMemory());

            GC.Collect();

            report2.Should().NotBeNull();
            
            // controller should not be alive
            contollerReference.IsAlive.Should().BeFalse();
            // no visitors should not be alive
            visitors.Values.Any(wr => wr.IsAlive).Should().BeFalse();
        }
    }
}