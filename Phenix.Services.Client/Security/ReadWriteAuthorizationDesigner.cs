using System;
using System.ComponentModel.Design;
using System.Windows.Forms;
using Phenix.Core.Windows;

namespace Phenix.Services.Client.Security
{
  internal class ReadWriteAuthorizationDesigner : ComponentDesigner
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

    public ReadWriteAuthorization ReadWriteAuthorization
    {
      get { return Component as ReadWriteAuthorization; }
    }

    public override DesignerVerbCollection Verbs
    {
      get
      {
        base.Verbs.Add(new DesignerVerb(Phenix.Services.Client.Properties.Resources.ShowReadWriteAuthorizationRules,
          new EventHandler(ShowReadWriteAuthorizationRules)));
        return base.Verbs;
      }
    }

    #endregion

    #region 事件

    private void ShowReadWriteAuthorizationRules(object sender, EventArgs e)
    {
      ShowMessageDialog.Execute(
        Phenix.Services.Client.Properties.Resources.ShowReadWriteAuthorizationRules,
        RootComponent.Name + Environment.NewLine + Environment.NewLine +
        ReadWriteAuthorization.RuleMessage());
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