using System.Linq;
using System;
using Caliburn.Micro;
using DevExpress.XtraReports.UI;
using GeniusCode.XtraReports.Runtime.Support;

namespace GeniusCode.XtraReports.Runtime
{
    public static class SubreportExtensions
    {
        public static object SetDataSourceOnSubreport(this XRSubreport subreport)
        {
            var datasource = subreport.Band.GetDataSource();

            // Good code below!
            if (datasource != null)
            {
                var report = subreport.ReportSource;

                report.SetReportOnDataSourceAsCollection(datasource);
            }

            return datasource;
        }

        public static int SetRootHashCodeOnSubreport(this XRSubreport subreportContainer, IEventAggregator aggregator)
        {
            var myReportBase = (gcXtraReport)subreportContainer.NavigateToBaseReport();
            var hashcode = myReportBase.RuntimeRootReportHashCode;

            if (hashcode == 0)
                throw new Exception("Report did not have a root hashcode.");

            var subreportAsMyReportbase = ConvertReportSourceToMyReportBaseIfNeeded(subreportContainer, aggregator);

            if (subreportAsMyReportbase != null)
                subreportAsMyReportbase.RuntimeRootReportHashCode = hashcode;

            return hashcode;
        }

        private static gcXtraReport ConvertReportSourceToMyReportBaseIfNeeded(this XRSubreport subreportContainer, IEventAggregator aggregator)
        {
            var subreportAsMyReportbase = subreportContainer.ReportSource as gcXtraReport;

            if (subreportAsMyReportbase == null)
            {
                subreportAsMyReportbase = subreportContainer.ReportSource.ConvertReportToMyReportBase(aggregator);
                subreportContainer.ReportSource = subreportAsMyReportbase;
            }

            return subreportAsMyReportbase;
        }


        //   #endregion
    }
}