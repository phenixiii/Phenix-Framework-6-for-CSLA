using System;
using System.ComponentModel.Design;
using System.Windows.Forms;
using Phenix.Core.Windows;

namespace Phenix.Services.Client.Security
{
  internal class ExecuteAuthorizationDesigner : ComponentDesigner
  {
    #region 属性

    public IDesignerHost DesignerHost
    {
      get { return GetService(typeof(IDesignerHost)) as IDesignerHost; }
    }

    public ContainerControl RootComponent
    {
      get { return DesignerHost.RootComponent as ContainerControl; }
    }

    public ExecuteAuthorization ExecuteAuthorization
    {
      get { return Component as ExecuteAuthorization; }
    }

    public override DesignerVerbCollection Verbs
    {
      get
      {
        base.Verbs.Add(new DesignerVerb(Phenix.Services.Client.Properties.Resources.ShowExecuteAuthorizationRules,
          new EventHandler(ShowExecuteAuthorizationRules)));
        return base.Verbs;
      }
    }

    #endregion

    #region 事件

    private void ShowExecuteAuthorizationRules(object sender, EventArgs e)
    {
      ShowMessageDialog.Execute(
        Phenix.Services.Client.Properties.Resources.ShowExecuteAuthorizationRules,
        RootComponent.Name + Environment.NewLine + Environment.NewLine +
        ExecuteAuthorization.RuleMessage());
    }

    #endregion

    #region 方法

    public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
    {
      base.InitializeNewComponent(defaultValues);

      IDesignerHost host = GetService(typeof(IDesignerHost)) as IDesignerHost;
      if (host == null)
        return;

      AppUtilities.ReferenceAssembly(this.GetService(typeof(ITypeResolutionService)) as ITypeResolutionService);
    }

    #endregion
  }
}