using System.ComponentModel.Design;

namespace Phenix.Windows.Helper
{
  internal static class DesignerHelper
  {
    public static void ReferenceAssembly(ITypeResolutionService service)
    {
      Phenix.Services.Client.Design.DesignerHelper.ReferenceAssembly(service, "Csla");
      Phenix.Services.Client.Design.DesignerHelper.ReferenceAssembly(service, "Phenix.Core");
      Phenix.Services.Client.Design.DesignerHelper.ReferenceAssembly(service, "Phenix.Business");
      Phenix.Services.Client.Design.DesignerHelper.ReferenceAssembly(service, "Phenix.Services.Client");

      //以下注册DevExpress版本为v13.2, 请按需调整
      Phenix.Services.Client.Design.DesignerHelper.ReferenceAssembly(service, "DevExpress.Data.v13.2");
      //Phenix.Core.ComponentModel.Design.DesignerHelper.ReferenceAssembly(service, "DevExpress.Design.v13.2");
      Phenix.Services.Client.Design.DesignerHelper.ReferenceAssembly(service, "DevExpress.Utils.v13.2");
      Phenix.Services.Client.Design.DesignerHelper.ReferenceAssembly(service, "DevExpress.XtraBars.v13.2");
      Phenix.Services.Client.Design.DesignerHelper.ReferenceAssembly(service, "DevExpress.XtraEditors.v13.2");
      //Phenix.Core.ComponentModel.Design.DesignerHelper.ReferenceAssembly(service, "DevExpress.XtraBars.v13.2.Design");
      Phenix.Services.Client.Design.DesignerHelper.ReferenceAssembly(service, "DevExpress.XtraGrid.v13.2");
      Phenix.Services.Client.Design.DesignerHelper.ReferenceAssembly(service, "DevExpress.XtraLayout.v13.2");
      Phenix.Services.Client.Design.DesignerHelper.ReferenceAssembly(service, "DevExpress.XtraNavBar.v13.2");
      Phenix.Services.Client.Design.DesignerHelper.ReferenceAssembly(service, "DevExpress.XtraTreeList.v13.2");
      Phenix.Services.Client.Design.DesignerHelper.ReferenceAssembly(service, "DevExpress.XtraWizard.v13.2");
    }
  }
}
