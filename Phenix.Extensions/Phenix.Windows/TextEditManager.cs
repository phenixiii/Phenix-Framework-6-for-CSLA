using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Phenix.Core;

namespace Phenix.Windows
{
  /// <summary>
  /// TextEdit管理组件 
  /// </summary>
  [Description("TextEdit管理组件")]
  [ProvideProperty("SelectAllOnEnter", typeof(TextEdit))] //切入焦点时可将文本设置为全选状态
  [ToolboxItem(true), ToolboxBitmap(typeof(TextEditManager), "Phenix.Windows.TextEditManager")]
  public sealed class TextEditManager : Component, IExtenderProvider, ISupportInitialize
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public TextEditManager()
      : base() { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="container">组件容器</param>
    public TextEditManager(IContainer container)
      : base()
    {
      if (container == null)
        throw new ArgumentNullException("container");
      container.Add(this);
    }

    #region 属性

    private new bool DesignMode
    {
      get { return base.DesignMode || AppConfig.DesignMode; }
    }

    private Control _host;
    /// <summary>
    /// 所属容器
    /// </summary>
    [DefaultValue(null), Browsable(false)]
    public Control Host
    {
      get
      {
        if (_host == null)
        {
          if (DesignMode)
          {
            IDesignerHost designer = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (designer != null)
              _host = designer.RootComponent as Control;
          }
        }
        return _host;
      }
      set
      {
        if (!DesignMode && _host != null)
          throw new InvalidOperationException("运行期不允许修改Host");
        _host = value;
        if (!DesignMode)
        {
          Form form = value as Form;
          if (form != null)
            form.Shown += new EventHandler(Host_Shown);
        }
      }
    }

    private readonly Dictionary<TextEdit, TextEditRuleStatus> _ruleStatuses = new Dictionary<TextEdit, TextEditRuleStatus>();

    #endregion

    #region 扩展程序属性

    /// <summary>
    /// 切入焦点时可将文本设置为全选状态
    /// </summary>
    [Description("切入焦点时可将文本设置为全选状态"), Category("Phenix")]
    public bool GetSelectAllOnEnter(TextEdit source)
    {
      TextEditRuleStatus result;
      if (_ruleStatuses.TryGetValue(source, out result))
        return result.SelectAllOnEnter;
      return true;
    }

    /// <summary>
    /// 切入焦点时可将文本设置为全选状态
    /// </summary>
    public void SetSelectAllOnEnter(TextEdit source, bool value)
    {
      TextEditRuleStatus status;
      if (_ruleStatuses.TryGetValue(source, out status))
        status.SelectAllOnEnter = value;
      else
        _ruleStatuses.Add(source, new TextEditRuleStatus { SelectAllOnEnter = value });
    }
    
    #endregion

    #region 事件

    private void Host_Shown(object sender, EventArgs e)
    {
      InitializeRule();
    }

    #region TextEdit 事件

    private void TextEdit_Enter(object sender, EventArgs e)
    {
      TextEdit textEdit = (TextEdit)sender;
      textEdit.SelectAll();
    }

    #endregion

    #endregion

    #region 方法

    #region IExtenderProvider 成员

    /// <summary>
    /// 是否可以将扩展程序属性提供给指定的对象
    /// </summary>
    /// <param name="extendee">要接收扩展程序属性的对象</param>
    public bool CanExtend(object extendee)
    {
      return extendee is TextEdit;
    }

    #endregion

    #region ISupportInitialize 成员

    ///<summary>
    /// 开始初始化
    ///</summary>
    public void BeginInit()
    {
    }

    ///<summary>
    /// 结束初始化
    ///</summary>
    public void EndInit()
    {
      if (!DesignMode && !(Host is Form))
        Host_Shown(null, null);
    }

    #endregion

    private void InitializeRule()
    {
      foreach (KeyValuePair<TextEdit, TextEditRuleStatus> kvp in _ruleStatuses)
        if (kvp.Value.SelectAllOnEnter)
        {
          kvp.Key.Enter += new EventHandler(TextEdit_Enter);
        }
    }

    #endregion

    #region 内嵌类

    [Serializable]
    private class TextEditRuleStatus
    {
      private bool _selectAllOnEnter = true;
      public bool SelectAllOnEnter
      {
        get { return _selectAllOnEnter; }
        set { _selectAllOnEnter = value; }
      }
    }

    #endregion
  }
}