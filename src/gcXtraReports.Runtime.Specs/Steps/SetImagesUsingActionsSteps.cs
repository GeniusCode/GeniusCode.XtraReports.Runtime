using System.IO;
using System.Linq;
using System.Reflection;
using Caliburn.Micro;
using DevExpress.XtraReports.UI;
using FluentAssertions;
using GeniusCode.XtraReports.Runtime.Actions;
using GeniusCode.XtraReports.Runtime.Support;
using TechTalk.SpecFlow;

namespace GeniusCode.XtraReports.Runtime.Specs.Steps
{
    [Binding]
    [Scope(Feature = "Images Should be Set By Action")]
    public class SetImagesUsingActionsSteps
    {
        private XtraReport _report;
        private string _imageFileName;
        private XRPictureBox _imageContainer;
        private IReportControlAction<XRPictureBox> _action
            ;

        private string _filename2;
        private ReportController _controller;

        [Given(@"a report exists")]
        public void GivenAReportExists()
        {
            _report = new XtraReport {DataSource = new[] {new object(), new object()}};
        }

        [Given(@"an image exists as a file")]
        public void GivenAnImageExistsAsAFile()
        {
            
            var stream = GetType().Assembly.GetManifestResourceStream("GeniusCode.XtraReports.Runtime.Specs.Steps.Penguins.jpg");

            _imageFileName = Helpers.GetNewTempFile() + ".jpg";

            using (Stream file = File.OpenWrite(_imageFileName))
            {
                Helpers.CopyStream(stream, file);
            }
        }

        [Given(@"the report contains an image placeholder")]
        public void GivenTheReportContainsAnImagePlaceholder()
        {
            var detail = new DetailBand();
            _imageContainer = new XRPictureBox {Name = "Penguins"};
            detail.Controls.Add(_imageContainer);
            _report.Bands.Add(detail);
        }


        [Given(@"an action exists to place the image into the placeholder")]
        public void GivenAnActionExistsToPlaceTheImageIntoThePlaceholder()
        {
            _action = ReportControlAction<XRPictureBox>.WithNoPredicate(p => p.ImageUrl = _imageFileName);
        }

        [When(@"the report runs")]
        public void WhenTheReportRuns()
        {
            _filename2 = Helpers.GetNewTempFile() + ".html";
            _controller = new ReportController(new EventAggregator(), _report, new ReportControlActionFacade(_action));
            _controller.Print(p => p.ExportToHtml(_filename2));
        }

        [Then(@"the image should be placed into the report")]
        public void ThenTheImageShouldBePlacedIntoTheReport()
        {
            var text = File.ReadAllText(_filename2);
            var toFind = string.Format("<img alt=\"\" src=\"{0}\"", _imageFileName);
            text.Contains(toFind).Should().BeTrue();
        }


    }
}
