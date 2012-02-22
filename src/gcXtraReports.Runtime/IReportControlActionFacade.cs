using System.Linq;
using DevExpress.XtraReports.UI;

namespace GeniusCode.XtraReports.Runtime
{
    public interface IReportControlActionFacade
    {
        void AttemptActionsOnControl(XRControl control);
    }
}