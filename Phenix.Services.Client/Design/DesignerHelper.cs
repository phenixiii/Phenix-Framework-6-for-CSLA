using System.ComponentModel.Design;
using System.Reflection;
using System.Runtime.InteropServices;
using Phenix.Core;
using Phenix.Core.Log;

namespace Phenix.Services.Client.Design
{
  /// <summary>
  /// 设计器助手
  /// </summary>
  public static class DesignerHelper
  {
    /// <summary>
    /// 添加程序集
    /// </summary>
    public static void ReferenceAssembly(ITypeResolutionService service, string assemblyName)
    {
      try
      {
        if (service != null)
        {
          Assembly assembly = Phenix.Core.Reflection.Utilities.LoadAssembly(assemblyName);
          service.ReferenceAssembly(assembly != null ? assembly.GetName() : new AssemblyName(assemblyName));
        }
      }
      catch (COMException ex)
      {
        if (AppConfig.Debugging)
          EventLog.SaveLocal(MethodBase.GetCurrentMethod(), assemblyName, ex);
      }
    }
  }
}
