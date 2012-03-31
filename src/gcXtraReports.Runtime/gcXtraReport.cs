using System;
using System.Linq;
using Caliburn.Micro;
using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;
using GeniusCode.XtraReports.Runtime.Messaging;

namespace GeniusCode.XtraReports.Runtime.Support
{
    [RootClass]
    // RootClassAttribute is REQUIRED for subreports!
        // Otherwise, custom classes are NOT allowed as subreports.
        // http://devexpress.com/Support/Center/p/Q300888.aspx
    public class gcXtraReport : XtraReport
    {
        private IEventAggregator _aggregator;
        /*[SRCategory(ReportStringId.CatData)]
        public XRSerializableCollection<DesignTimeDataSourceDefinition> DesignTimeDataSources { get; set; }*/

        public gcXtraReport(IEventAggregator aggregator)
        {
            _aggregator = aggregator;
            /*DesignTimeDataSources = new XRSerializableCollection<DesignTimeDataSourceDefinition>();*/
        }

        /// <summary>
        /// Hashcode of Root Report at runtime, and not at design time.
        /// </summary>

        private Guid _rootReportGuid;
        public Guid RootReportGuid {get { return _rootReportGuid; }}
        
        public void SetRootReportGuid(Guid newGuid)
        {
            // We need this check, because this method get called multiple times,
            // even after the value is first set!
            if (_rootReportGuid == newGuid) return;


            if (_rootReportGuid == Guid.Empty)
                _rootReportGuid = newGuid;
            else
                throw new InvalidOperationException("Root report Guid may only be set once!");
        }

        public Guid InitRootReportGuid()
        {
            SetRootReportGuid(Guid.NewGuid());
            return RootReportGuid;
        }

/*        protected override void DeclareCustomProperties()
        {
            // Serialize DesignTimeDataSources collection into .REPX file
            DeclareCustomObjectProperty(() => DesignTimeDataSources);
        }*/

        protected override void OnBeforePrint(System.Drawing.Printing.PrintEventArgs e)
        {
            // IMPORTANT: Must use an aggregator for End-User Designer, because reports are serialized / CodeDom - events cannot be attached
            // Reports pass themselves into the aggregator
            var message = new BeforeReportPrintMessage(this, e);
            _aggregator.Publish(message);

            base.OnBeforePrint(e);
        }

        protected override void OnDisposing()
        {
            _aggregator = null;
            base.OnDisposing();
           
        }
    }

}