using System;
using DevExpress.XtraReports.UI;

namespace Phenix.Windows.Helper
{
  internal class ReportSelectorEditor : Phenix.Services.Client.Design.ClassSelectorEditor
  {
    protected override Type SubclassOfType
    {
      get { return typeof(XtraReport); }
    }
  }
}
