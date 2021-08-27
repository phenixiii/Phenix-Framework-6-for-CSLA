using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraBars;
using Phenix.Core;
using Phenix.Core.Log;
using Phenix.Core.Plugin;
using Phenix.Core.Security;

namespace Phenix.Windows.Main
{
  /// <summary>
  /// 主窗体
  /// </summary>
  public partial class MainForm : Phenix.Core.Windows.BaseForm
  {
    /// <summary>
    /// 主窗体
    /// </summary>
    public MainForm()
    {
      InitializeComponent();
    }

    #region 工作间

    private void Initialize()
    {
      if (UserIdentity.CurrentIdentity != null)
        this.Text = string.Format("{0} - {1}", this.Text, UserIdentity.CurrentIdentity.Enterprise);
      using (new DevExpress.Utils.WaitDialogForm("正在初始化主窗体 ...", Phenix.Core.Properties.Resources.PleaseWait))
      {
        Application.DoEvents();
        PluginHost.Default.Message += new System.EventHandler<Phenix.Core.Plugin.PluginEventArgs>(PluginHost_Message);
      }
      Humanistic();
      //启动工作流子窗体插件，可以注释掉不用
      Run(this.bbiWorkflowTask.Caption, (string)this.bbiWorkflowTask.Tag);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    private static void DoFinalize()
    {
      //注销
      Phenix.Services.Client.Security.LogOnHelper.LogOff();
    }
    
    private void Humanistic()
    {
      UserIdentity identity = UserIdentity.CurrentIdentity;
      if (identity != null)
      {
        bsiUserName.Caption = identity.UserName;
        bsiDepartment.Caption = identity.Department != null 
          ? identity.Department.Name + (identity.Department.InHeadquarters.HasValue && identity.Department.InHeadquarters.Value ? "!" : String.Empty)
          : null;
        bsiPosition.Caption = identity.Position != null ? identity.Position.Name : null;
        bsiServicesAddress.Caption = Phenix.Core.Net.NetConfig.ServicesAddress;
      }
    }

    /// <summary>
    /// 启动子窗体插件
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void Run(string caption, string assemblyName)
    {
      using (new DevExpress.Utils.WaitDialogForm("正在加载 " + caption + " 窗体 ...", Phenix.Core.Properties.Resources.PleaseWait))
        try
        {
          PluginHost.Default.SendSingletonMessage(assemblyName, this);
        }
        catch (Exception ex)
        {
          MessageBox.Show("正在建设中: " + assemblyName + "\n谢谢关注!...\n" + AppUtilities.GetErrorHint(ex), caption,
            MessageBoxButtons.OK, MessageBoxIcon.Information);
          EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
        }
    }

    #endregion

    private void PluginHost_Message(object sender, PluginEventArgs e)
    {
      bsiHint.Caption = e.Message.ToString();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      Initialize();
      Application.DoEvents();

      ////测试：获取本地序号方法
      //long i = Phenix.Core.Data.Sequence.Value; 

      ////恢复上次本地序号（针对暂未升级到16年版的系统，升级后可废弃以下代码）
      //long sequenceValue;
      //if (Int64.TryParse(Phenix.Core.AppSettings.ReadValue(typeof(Phenix.Core.Data.Sequence).FullName), out sequenceValue))
      //  typeof(Phenix.Core.Data.Sequence).GetField("_value", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, sequenceValue);
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      ////保存最新本地序号（针对暂未升级到16年版的系统，升级后可废弃以下代码）
      //Phenix.Core.AppSettings.SaveValue(typeof(Phenix.Core.Data.Sequence).FullName, typeof(Phenix.Core.Data.Sequence).GetField("_value", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null).ToString());

      if (e.CloseReason == CloseReason.UserClosing)
      {
        e.Cancel = MessageBox.Show("您确定要退出系统吗?", this.Text,
          MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No;
        if (e.Cancel)
          return;
      }
      DoFinalize();
    }

    private void bbiChangeUser_ItemClick(object sender, ItemClickEventArgs e)
    {
      System.Diagnostics.Process.Start(
        Path.Combine(AppConfig.BaseDirectory, AppDomain.CurrentDomain.FriendlyName),
        MethodBase.GetCurrentMethod().Name);
      Application.Exit();
    }

    private void bbiExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      this.Close();
    }

    private void bbiUserManage_ItemClick(object sender, ItemClickEventArgs e)
    {
      Run(e.Link.Caption, (string)e.Link.Item.Tag); //需启动的子窗体插件，其程序集名称埋在了按钮的Tag属性里
    }

    private void MainForm_Shown(object sender, EventArgs e)
    {
      //启动动态刷新
      //Phenix.Renovate.Client.Subscriber.Default.Start();
    }

    private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      //关闭动态刷新
      //Phenix.Renovate.Client.Subscriber.Default.Close();
    }

    private void weaveProberTestBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
    {
      ////测试：故意抛出被零除的缺陷以测试织入探针功能
      //请事先通过IDE的Phenix Teamwork Tools的Weave Prober将探针织入本函数
      int i = 1;
      int j = 0;
      i = i / j;
    }
  }
}