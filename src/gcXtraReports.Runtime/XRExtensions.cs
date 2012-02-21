﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DevExpress.Data.Browsing;
using DevExpress.XtraReports.Native.Data;
using DevExpress.XtraReports.UI;
using GeniusCode.XtraReports.Runtime.Support;
using gcExtensions;

namespace GeniusCode.XtraReports.Runtime
{
    public static class XRExtensions
    {
        public static gcXtraReport ConvertReportToMyReportBase(this XtraReport report)
        {
            if (report == null) return null;

            // return, do not convert if input is already myReportBase
            var convertReportToMyReportBase = report as gcXtraReport;
            if (convertReportToMyReportBase != null)
                return convertReportToMyReportBase;

            return report.CloneLayoutAsMyReportBase();
        }

        public static gcXtraReport CloneLayoutAsMyReportBase(this XtraReport report)
        {
            var stream = new MemoryStream();
            report.SaveLayout(stream);
            stream.Position = 0;

            var newReport = new gcXtraReport();
            newReport.LoadLayout(stream);
            newReport.DataSource = report.DataSource;

            return newReport;
        }

        public static XtraReport NavigateToBaseReport(this XRControl input)
        {
            var currentParent = input.Report;

            while (!(input is XtraReport) && !ReferenceEquals(currentParent, currentParent.Report))
            {
                currentParent = currentParent.Report;
            }


            //NOTE: BAD!
            /*            if (!(currentParent is gcXtraReport) && currentParent is XtraReport)
                            currentParent = ((XtraReport) currentParent).ConvertReportToMyReportBase();*/

            return (XtraReport) currentParent;
        }

        #region Get DataSource

        /// <summary>
        /// Obtains DataSource for this Band.  Works at Runtime and Design-Time. Very important method.
        /// </summary>
        /// <param name="band"></param>
        /// <returns></returns>
        public static object GetDataSource(this Band band)
        {
            object result = null;

            // Single
            band.TryAs<DetailBand>(detailBand =>
                                       {
                                           var browser = detailBand.Report.GetListBrowser();
                                           result = browser.Current;
                                       });

            // Collection
            if (result == null)
            {
                var browser = band.Report.GetListBrowser();
                return browser.List;
            }

            return result;
        }

        public static ReportDataContext GetReportDataContext(this XtraReportBase report)
        {
            VerifyListBrowserAndDataSourceAreCreated(report);

            var field = typeof (XtraReportBase).GetField("fDataContext", BindingFlags.NonPublic | BindingFlags.Instance);

            return (ReportDataContext) field.GetValue(report);
        }

        public static ListBrowser GetListBrowser(this XtraReportBase report)
        {
            VerifyListBrowserAndDataSourceAreCreated(report);

            var field = typeof (XtraReportBase).GetField("dataBrowser", BindingFlags.NonPublic | BindingFlags.Instance);

            return (ListBrowser) field.GetValue(report);
        }

        private static void VerifyListBrowserAndDataSourceAreCreated(XtraReportBase report)
        {
            // Force ListBrowser to be created, along with DataContext
            // Very important.
            report.GetCurrentRow();
        }

        // Another method of passing Subreport it's parent datasource
        // This method did not work for me.
        //private static void ChangeSubreportParentForDataContext(this XRSubreport subreport)
        //{
        //    var report = subreport.ReportSource;

        //    //var dataContextContainer = subreport.Report;
        //    var dataContextContainer = subreport.RootReport;

        //    //container.Bands[BandKind.Detail].Controls.Add(report);

        //    var field = typeof(XtraReportBase).GetField("fParent", BindingFlags.NonPublic | BindingFlags.Instance);
        //    field.SetValue(report, dataContextContainer);
        //}

        #endregion

        #region Set DataSource

        public static void SetReportOnDataSourceAsCollection(this XtraReportBase report, object datasource)
        {
            if (report == null) return;

            if (datasource == null)
                report.DataSource = null;

                // Set Datasource, Must be a Collection
            else if (datasource is IEnumerable)
                report.DataSource = datasource;
            else
                report.DataSource = new List<object> {datasource};
        }

        #endregion

        public static IEnumerable<XRControl> GetAllControls(this XRControl control)
        {
            var myControls = control.Controls.Cast<XRControl>().ToList();

            // Recursive
            var bandChildControls = from band in myControls.OfType<Band>()
                                    from bandChildControl in GetAllControls(band)
                                    select bandChildControl;

            return myControls.Concat(bandChildControls);
        }

        public static IEnumerable<XRControl> GetAllControls(this Band band)
        {
            var myControls = band.Controls.Cast<XRControl>().ToList();

            // Recursive
            var childControls = from control in myControls
                                from childControl in GetAllControls(control)
                                select childControl;

            return myControls.Concat(childControls);
        }


        public static IEnumerable<XRSubreport> FindAllSubreports(this XtraReport report)
        {
            var q = from band in report.Bands.Cast<Band>()
                    from subreport in FindAllSubreports(band)
                    select subreport;

            return q;
        }

        public static IEnumerable<XRSubreport> FindAllSubreports(this Band band)
        {
            var mySubreports = band.Controls.OfType<XRSubreport>();

            // Recursive
            var childBandSubreports = from childBand in band.Controls.OfType<Band>()
                                      from subreport in FindAllSubreports(childBand)
                                      select subreport;

            return Enumerable.Concat(mySubreports, childBandSubreports);
        }
    }
}