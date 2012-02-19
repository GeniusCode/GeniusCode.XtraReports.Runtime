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
        /*[SRCategory(ReportStringId.CatData)]
        public XRSerializableCollection<DesignTimeDataSourceDefinition> DesignTimeDataSources { get; set; }*/

        public gcXtraReport()
        {
            /*DesignTimeDataSources = new XRSerializableCollection<DesignTimeDataSourceDefinition>();*/
        }

        /// <summary>
        /// Hashcode of Root Report at runtime, and not at design time.
        /// </summary>
        public int RuntimeRootReportHashCode { get; set; }

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
            EventAggregatorSingleton.Instance.Publish(message);

            base.OnBeforePrint(e);
        }
    }
}