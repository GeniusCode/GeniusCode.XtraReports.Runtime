using System;
using System.Linq;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;

namespace GeniusCode.XtraReports.Runtime.Support
{
    /// <summary>
    /// Factory for gcXtraReport
    /// </summary>
    public class ReportFactory : ReportTypeService
    {
        private int _reportCount = 0;

        public XtraReport GetDefaultReport()
        {
            _reportCount += 1;
            var reportName = String.Format("Report{0}",_reportCount);

            var report = CreateReport(reportName);
            return report;
        }

        public gcXtraReport GetNewReport()
        {
            return GetNewReport(string.Empty);
        }

        public gcXtraReport GetNewReport(string reportName)
        {
            return CreateReport(reportName);
        }

        public Type GetType(Type reportType)
        {
            return reportType;
        }

        #region Ryan's Custom Methods

        private static gcXtraReport CreateReport(string reportName)
        {
            var report = new gcXtraReport {Name = reportName, DisplayName = reportName};

            var detail = new DetailBand();
            var topMargin = new TopMarginBand();
            var bottomMargin = new BottomMarginBand();

            report.Bands.AddRange(new Band[]
                                      {
                                          detail,
                                          topMargin,
                                          bottomMargin
                                      });

            return report;
        }

        #endregion
    }
}