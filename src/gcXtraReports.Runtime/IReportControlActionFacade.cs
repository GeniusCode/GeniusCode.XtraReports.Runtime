using System.Linq;
using DevExpress.XtraReports.UI;

namespace GeniusCode.XtraReports.Runtime.Actions
{
    public interface IReportControlActionFacade
    {
        void AttemptActionsOnControl(XRControl control);
    }
}