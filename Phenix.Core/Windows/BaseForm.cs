using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Phenix.Core.Dictionary;

namespace Phenix.Core.Windows
{
  /// <summary>
  /// 基础窗体
  /// </summary>
  [DataDictionary(AssemblyClassType.Form)]
  public partial class BaseForm : Form, IForm
  {
    /// <summary>
    /// 基础窗体
    /// </summary>
    public BaseForm()
    {
      InitializeComponent();
    }

    #region 外部调用接口

    #region ExecuteMdi

    private static readonly Dictionary<string, IForm> _defaults = new Dictionary<string, IForm>(StringComparer.Ordinal);

    /// <summary>
    /// 执行MDI窗体
    /// isSingleton = true
    /// </summary>
    /// <param name="mdiParent">主窗体</param>
    public static T ExecuteMdi<T>(Form mdiParent)
      where T : IForm
    {
      return ExecuteMdi<T>(mdiParent, true);
    }

    /// <summary>
    /// 执行MDI窗体
    /// </summary>
    /// <param name="mdiParent">主窗体</param>
    /// <param name="isSingleton">是单例</param>
    public static T ExecuteMdi<T>(Form mdiParent, bool isSingleton)
      where T : IForm
    {
      IForm result;
      if (!isSingleton)
      {
        result = (T)Activator.CreateInstance(typeof(T), true);
        result.WindowState = FormWindowState.Maximized;
      }
      else
      {
        string key = typeof(T).FullName;
        //仅允许启动单个窗体
        if (!_defaults.TryGetValue(key, out result))
        {
          result = (T)Activator.CreateInstance(typeof(T), true);
          result.WindowState = FormWindowState.Maximized;
          _defaults[key] = result;
        }
      }
      //激活窗体
      result.Activate(mdiParent);
      return (T)result;
    }

    /// <summary>
    /// 移除MDI窗体
    /// </summary>
    public static T FindMdi<T>()
      where T : IForm
    {
      IForm result;
      _defaults.TryGetValue(typeof(T).FullName, out result);
      return (T)result;
    }

    /// <summary>
    /// 移除MDI窗体
    /// </summary>
    /// <param name="mdiForm">MDI窗体</param>
    public static bool RemoveMdi(IForm mdiForm)
    {
      if (mdiForm == null)
        throw new ArgumentNullException("mdiForm");
      //仅允许启动单个窗体
      return _defaults.Remove(mdiForm.GetType().FullName);
    }

    #endregion

    #region ExecuteDialog

    /// <summary>
    /// 执行Dialog窗体
    /// </summary>
    /// <param name="workSource">操作数据源</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static DialogResult ExecuteDialog<T>(object workSource)
      where T : IForm
    {
      T result = (T)Activator.CreateInstance(typeof(T), true);
      result.WorkSource = workSource;
      result.WindowState = FormWindowState.Normal;
      result.StartPosition = FormStartPosition.CenterScreen;
      return result.ShowDialog();
    }

    #endregion

    #endregion

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
      RemoveMdi(this);
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