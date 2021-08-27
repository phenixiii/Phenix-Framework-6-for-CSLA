using System.ComponentModel.Design;

namespace Phenix.Services.Client
{
  /// <summary>
  /// 应用系统工具集
  /// </summary>
  public static class AppUtilities
  {
    internal static void ReferenceAssembly(ITypeResolutionService service)
    {
      Phenix.Services.Client.Design.DesignerHelper.ReferenceAssembly(service, "Csla");
      Phenix.Services.Client.Design.DesignerHelper.ReferenceAssembly(service, "Phenix.Core");
      Phenix.Services.Client.Design.DesignerHelper.ReferenceAssembly(service, "Phenix.Business");
    }
  }
}
