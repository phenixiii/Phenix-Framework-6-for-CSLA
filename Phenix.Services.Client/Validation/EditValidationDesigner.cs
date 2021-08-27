using System;
using System.ComponentModel.Design;
using System.Windows.Forms;
using Phenix.Services.Client.Library;

namespace Phenix.Services.Client.Validation
{
  internal class EditValidationDesigner : ComponentDesigner
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

    public EditValidation EditValidation
    {
      get { return Component as EditValidation; }
    }

    public override DesignerVerbCollection Verbs
    {
      get
      {
        base.Verbs.Add(new DesignerVerb(Phenix.Services.Client.Properties.Resources.ResetEditFriendlyCaptions,
          new EventHandler(ResetEditFriendlyCaptions)));
        base.Verbs.Add(new DesignerVerb(Phenix.Services.Client.Properties.Resources.ResetGridFriendlyCaptions,
          new EventHandler(ResetGridFriendlyCaptions)));
        base.Verbs.Add(new DesignerVerb(Phenix.Services.Client.Properties.Resources.ResetGridFriendlyLayouts,
          new EventHandler(ResetGridFriendlyLayouts)));
        base.Verbs.Add(new DesignerVerb(Phenix.Services.Client.Properties.Resources.ResetFormRules,
          new EventHandler(ResetFormRules)));
        return base.Verbs;
      }
    }

    #endregion

    #region 事件

    private void ResetEditFriendlyCaptions(object sender, EventArgs e)
    {
      Initialize();
      //重置
      RootComponent.SuspendLayout();
      RaiseComponentChanging(null);
      EditValidation.ResetEditFriendlyCaptions();
      RaiseComponentChanged(null, null, null);
      RootComponent.ResumeLayout(false);
      //提示
      MessageBox.Show(Phenix.Services.Client.Properties.Resources.ResetEditFriendlyCaptionsSucceed,
        Phenix.Services.Client.Properties.Resources.ResetEditFriendlyCaptions, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void ResetGridFriendlyCaptions(object sender, EventArgs e)
    {
      Initialize();
      //重置
      RootComponent.SuspendLayout();
      RaiseComponentChanging(null);
      EditValidation.ResetGridFriendlyCaptions(RootComponent);
      RaiseComponentChanged(null, null, null);
      RootComponent.ResumeLayout(false);
      //提示
      MessageBox.Show(Phenix.Services.Client.Properties.Resources.ResetGridFriendlyCaptionsSucceed,
        Phenix.Services.Client.Properties.Resources.ResetGridFriendlyCaptions, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void ResetGridFriendlyLayouts(object sender, EventArgs e)
    {
      Initialize();
      //重置
      RootComponent.SuspendLayout();
      RaiseComponentChanging(null);
      bool succeed = EditValidation.ResetGridFriendlyLayouts(RootComponent);
      RaiseComponentChanged(null, null, null);
      RootComponent.ResumeLayout(false);
      //提示
      if (succeed)
        MessageBox.Show(Phenix.Services.Client.Properties.Resources.ResetGridFriendlyLayoutsSucceed,
          Phenix.Services.Client.Properties.Resources.ResetGridFriendlyLayouts, MessageBoxButtons.OK, MessageBoxIcon.Information);
      else
        MessageBox.Show(Phenix.Services.Client.Properties.Resources.ResetGridFriendlyLayoutsAbort,
          Phenix.Services.Client.Properties.Resources.ResetGridFriendlyLayouts, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    private void ResetFormRules(object sender, EventArgs e)
    {
      Initialize();
      //重置
      RootComponent.SuspendLayout();
      RaiseComponentChanging(null);
      EditValidation.ResetRules(RootComponent);
      RaiseComponentChanged(null, null, null);
      RootComponent.ResumeLayout(false);
      //提示
      MessageBox.Show(Phenix.Services.Client.Properties.Resources.ResetFormRulesSucceed,
        Phenix.Services.Client.Properties.Resources.ResetFormRules, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    #endregion

    #region 方法

    private static void Initialize()
    {
      Registration.RegisterEmbeddedWorker(false);
    }

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