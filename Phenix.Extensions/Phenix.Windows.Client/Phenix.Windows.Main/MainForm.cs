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
  /// ������
  /// </summary>
  public partial class MainForm : Phenix.Core.Windows.BaseForm
  {
    /// <summary>
    /// ������
    /// </summary>
    public MainForm()
    {
      InitializeComponent();
    }

    #region ������

    private void Initialize()
    {
      if (UserIdentity.CurrentIdentity != null)
        this.Text = string.Format("{0} - {1}", this.Text, UserIdentity.CurrentIdentity.Enterprise);
      using (new DevExpress.Utils.WaitDialogForm("���ڳ�ʼ�������� ...", Phenix.Core.Properties.Resources.PleaseWait))
      {
        Application.DoEvents();
        PluginHost.Default.Message += new System.EventHandler<Phenix.Core.Plugin.PluginEventArgs>(PluginHost_Message);
      }
      Humanistic();
      //�����������Ӵ�����������ע�͵�����
      Run(this.bbiWorkflowTask.Caption, (string)this.bbiWorkflowTask.Tag);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    private static void DoFinalize()
    {
      //ע��
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
    /// �����Ӵ�����
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void Run(string caption, string assemblyName)
    {
      using (new DevExpress.Utils.WaitDialogForm("���ڼ��� " + caption + " ���� ...", Phenix.Core.Properties.Resources.PleaseWait))
        try
        {
          PluginHost.Default.SendSingletonMessage(assemblyName, this);
        }
        catch (Exception ex)
        {
          MessageBox.Show("���ڽ�����: " + assemblyName + "\nлл��ע!...\n" + AppUtilities.GetErrorHint(ex), caption,
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

      ////���ԣ���ȡ������ŷ���
      //long i = Phenix.Core.Data.Sequence.Value; 

      ////�ָ��ϴα�����ţ������δ������16����ϵͳ��������ɷ������´��룩
      //long sequenceValue;
      //if (Int64.TryParse(Phenix.Core.AppSettings.ReadValue(typeof(Phenix.Core.Data.Sequence).FullName), out sequenceValue))
      //  typeof(Phenix.Core.Data.Sequence).GetField("_value", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, sequenceValue);
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      ////�������±�����ţ������δ������16����ϵͳ��������ɷ������´��룩
      //Phenix.Core.AppSettings.SaveValue(typeof(Phenix.Core.Data.Sequence).FullName, typeof(Phenix.Core.Data.Sequence).GetField("_value", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null).ToString());

      if (e.CloseReason == CloseReason.UserClosing)
      {
        e.Cancel = MessageBox.Show("��ȷ��Ҫ�˳�ϵͳ��?", this.Text,
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
      Run(e.Link.Caption, (string)e.Link.Item.Tag); //���������Ӵ���������������������˰�ť��Tag������
    }

    private void MainForm_Shown(object sender, EventArgs e)
    {
      //������̬ˢ��
      //Phenix.Renovate.Client.Subscriber.Default.Start();
    }

    private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      //�رն�̬ˢ��
      //Phenix.Renovate.Client.Subscriber.Default.Close();
    }

    private void weaveProberTestBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
    {
      ////���ԣ������׳��������ȱ���Բ���֯��̽�빦��
      //������ͨ��IDE��Phenix Teamwork Tools��Weave Prober��̽��֯�뱾����
      int i = 1;
      int j = 0;
      i = i / j;
    }
  }
}