using System;
using System.ComponentModel;
using System.Windows.Forms;
using Phenix.Core.Dictionary;

namespace Phenix.Windows
{
  /// <summary>
  /// 基础窗体
  /// </summary>
  [DataDictionary(AssemblyClassType.Form)]
  public partial class BaseForm : DevExpress.XtraEditors.XtraForm, Phenix.Core.Windows.IForm
  {
    /// <summary>
    /// 基础窗体
    /// </summary>
    public BaseForm()
    {
      InitializeComponent();
    }

    #region 属性

    /// <summary>
    /// 指示在将键事件传递到具有焦点的控件前窗体是否将接收此键事件
    /// </summary>
    [DefaultValue(true)]
    public new bool KeyPreview
    {
      get { return base.KeyPreview; }
      set
      {
        if (!value)
          this.EnterMoveNextControl = false;
        base.KeyPreview = value;
      }
    }

    private bool _enterMoveNextControl = true;
    /// <summary>
    /// 当输入回车键时选择下一个可用控件并使其成为活动控件
    /// </summary>
    [DefaultValue(true), Description("当输入回车键时选择下一个可用控件并使其成为活动控件"), Category("Behavior")]
    public bool EnterMoveNextControl
    {
      get { return _enterMoveNextControl; }
      set
      {
        if (value)
          this.KeyPreview = true;
        _enterMoveNextControl = value;
      }
    }

    private bool _pgUpMoveBackControl = true;
    /// <summary>
    /// 当输入上页键时选择上一个可用控件并使其成为活动控件
    /// </summary>
    [DefaultValue(true), Description("当输入上页键时选择上一个可用控件并使其成为活动控件"), Category("Behavior")]
    public bool PgUpMoveBackControl
    {
      get { return _pgUpMoveBackControl; }
      set
      {
        if (value)
          this.KeyPreview = true;
        _pgUpMoveBackControl = value;
      }
    }

    private object _workSource;
    /// <summary>
    /// 操作数据源
    /// </summary>
    [DefaultValue(null), Browsable(false)]
    public object WorkSource
    {
      get { return _workSource; }
      set { _workSource = value; }
    }

    #endregion

    #region 事件

    /// <summary>
    /// OnShown
    /// </summary>
    protected override void OnShown(EventArgs e)
    {
      base.OnShown(e);
    }

    /// <summary>
    /// OnFormClosed
    /// </summary>
    protected override void OnFormClosed(FormClosedEventArgs e)
    {
      base.OnFormClosed(e);
      Phenix.Core.Windows.BaseForm.RemoveMdi(this);
    }

    /// <summary>
    /// OnKeyPress
    /// </summary>
    protected override void OnKeyPress(KeyPressEventArgs e)
    {
      if (EnterMoveNextControl && e.KeyChar == '\r')
      {
        e.Handled = true;
        SendKeys.Send("{TAB}"); //SelectNextControl(this, true, true, true, true); 
      }
      else
        base.OnKeyPress(e);
    }

    /// <summary>
    /// OnKeyDown
    /// </summary>
    protected override void OnKeyDown(KeyEventArgs e)
    {
      if (PgUpMoveBackControl && e.KeyCode == Keys.PageUp)
      {
        e.Handled = true;
        SendKeys.Send("+{TAB}"); //SelectNextControl(this, false, true, true, true); 
      }
      else
        base.OnKeyDown(e);
    }

    #endregion

    #region 方法

    /// <summary>
    /// 分析消息
    /// 由 PluginHost 调用
    /// </summary>
    /// <param name="message">消息</param>
    /// <returns>按需返回</returns>
    public virtual object AnalyseMessage(object message)
    {
      return this;
    }

    /// <summary>
    /// 激活MDI窗体
    /// </summary>
    /// <param name="mdiParent">主窗体</param>
    public void Activate(Form mdiParent)
    {
      MdiParent = mdiParent;
      Show();
      Activate();
      Application.DoEvents(); //触发窗体Shown事件
    }

    #endregion
  }
}