using System;
using System.ComponentModel.Design;
using System.Windows.Forms;
using Phenix.Core.Windows;

namespace Phenix.Windows
{
  internal class TreeListManagerDesigner : ComponentDesigner
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

    public TreeListManager TreeListManager
    {
      get { return Component as TreeListManager; }
    }

    public override DesignerVerbCollection Verbs
    {
      get
      {
        base.Verbs.Add(new DesignerVerb(Phenix.Windows.Properties.Resources.ResetTreeListRules,
          new EventHandler(ResetTreeListRules)));
        return base.Verbs;
      }
    }

    #endregion

    #region 事件

    private void ResetTreeListRules(object sender, EventArgs e)
    {
      Initialize();
      //重置
      using (new DevExpress.Utils.WaitDialogForm(Phenix.Windows.Properties.Resources.ResetTreeListRules, Phenix.Core.Properties.Resources.PleaseWait))
      {
        RootComponent.SuspendLayout();
        RaiseComponentChanging(null);
        ShowMessageDialog.Execute(Phenix.Windows.Properties.Resources.ResetTreeListRules,
          Phenix.Windows.Properties.Resources.ResetTreeListRulesSucceed + Environment.NewLine + TreeListManager.ResetRules(RootComponent));
        RaiseComponentChanged(null, null, null);
        RootComponent.ResumeLayout(false);
      }
    }

    #endregion

    #region 方法

    private static void Initialize()
    {
      Phenix.Services.Client.Library.Registration.RegisterEmbeddedWorker(false);
    }

    public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
    {
      base.InitializeNewComponent(defaultValues);

      IDesignerHost host = GetService(typeof(IDesignerHost)) as IDesignerHost;
      if (host == null)
        return;

      Phenix.Windows.Helper.DesignerHelper.ReferenceAssembly(this.GetService(typeof(ITypeResolutionService)) as ITypeResolutionService);
    }

    #endregion
  }
}