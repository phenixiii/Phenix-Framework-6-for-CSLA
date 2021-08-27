using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using Phenix.Core;
using Phenix.Core.Dictionary;
using Phenix.Core.Security;

namespace Phenix.Services.Client.Security
{
  /// <summary>
  /// 执行授权组件
  /// </summary>
  [Description("控制WinForm界面上控件/组件所绑定业务对象过程的执行授权, 涉及其属性: Enabled、Visible(Visibility)")]
  [Designer(typeof(ExecuteAuthorizationDesigner))]
  [ProvideProperty("ApplyAuthorization", typeof(Component))] //被应用到授权控制
  [ProvideProperty("AllowVisible", typeof(Component))] //是否允许可见
  [ProvideProperty("ApplyRuleItems", typeof(Component))] //执行授权规则内容
  [ToolboxItem(true), ToolboxBitmap(typeof(ExecuteAuthorization), "Phenix.Services.Client.Security.ExecuteAuthorization")]
  public sealed class ExecuteAuthorization : Component, IExtenderProvider, ISupportInitialize
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public ExecuteAuthorization()
      : base() { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="container">组件容器</param>
    public ExecuteAuthorization(IContainer container)
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

    private AssemblyClassInfo _classInfo;
    private AssemblyClassInfo ClassInfo
    {
      get
      {
        if (_classInfo == null)
        {
          if (Host != null)
            _classInfo = DataDictionaryHub.GetClassInfo(Host.GetType());
        }
        return _classInfo;
      }
    }

    private readonly Dictionary<Component, ComponentAuthorizationStatus> _authorizationStatuses = 
      new Dictionary<Component, ComponentAuthorizationStatus>();
    private readonly Dictionary<Component, ComponentRuleStatus> _ruleStatuses = 
      new Dictionary<Component, ComponentRuleStatus>();
    private readonly Dictionary<BindingSource, List<Component>> _bindingSourceInfos =
      new Dictionary<BindingSource, List<Component>>();

    #endregion

    #region 扩展程序属性

    /// <summary>
    /// 被应用到授权控制
    /// </summary>
    [DefaultValue(false), Description("被应用到授权控制"), Category("Phenix")]
    public bool GetApplyAuthorization(Component source)
    {
      ComponentAuthorizationStatus status;
      if (_authorizationStatuses.TryGetValue(source, out status))
        return status.Apply;
      return false;
    }

    /// <summary>
    /// 被应用到授权控制
    /// </summary>
    public void SetApplyAuthorization(Component source, bool value)
    {
      ComponentAuthorizationStatus status;
      if (_authorizationStatuses.TryGetValue(source, out status))
        status.Apply = value;
      else
        _authorizationStatuses.Add(source, new ComponentAuthorizationStatus { Apply = value });
    }

    /// <summary>
    /// 是否允许可见
    /// </summary>
    [DefaultValue(true), Description("是否允许可见\n本组件被应用到授权, 当授权不可执行时是否允许可见"), Category("Phenix")]
    public bool GetAllowVisible(Component source)
    {
      ComponentAuthorizationStatus status;
      if (_authorizationStatuses.TryGetValue(source, out status))
        return status.AllowVisible;
      return true;
    }

    /// <summary>
    /// 是否允许可见
    /// </summary>
    public void SetAllowVisible(Component source, bool value)
    {
      ComponentAuthorizationStatus status;
      if (_authorizationStatuses.TryGetValue(source, out status))
        status.AllowVisible = value;
      else
        _authorizationStatuses.Add(source, new ComponentAuthorizationStatus { AllowVisible = value });
    }

    /// <summary>
    /// 执行授权规则内容
    /// </summary>
    [Description("执行授权规则内容\n本组件绑定到具体的执行授权规则队列"), Category("Phenix")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Editor(typeof(Phenix.Services.Client.Design.CollectionEditor), typeof(UITypeEditor))]
    public Collection<ExecuteRule> GetApplyRuleItems(Component source)
    {
      ComponentRuleStatus status;
      if (!_ruleStatuses.TryGetValue(source, out status))
      {
        status = new ComponentRuleStatus();
        _ruleStatuses.Add(source, status);
      }
      return status.ExecuteRules;
    }

    #endregion

    #region 事件

    private void Host_Shown(object sender, EventArgs e)
    {
      RegisterComponentAuthorizationRules();
      InitializeRule();
    }

    private void Control_EnabledChanged(object sender, EventArgs e)
    {
      Control control = (Control)sender;
      if (control.Enabled)
        ResetComponentAuthorizationRules(control);
    }

    #region BindingSource 事件

    private void BindingSource_CurrentChanged(object sender, EventArgs e)
    {
      List<Component> components;
      if (_bindingSourceInfos.TryGetValue((BindingSource)sender, out components))
        foreach (Component item in components)
          ResetComponentAuthorizationRules(item);
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
      return extendee is Component &&
        extendee.GetType().GetProperty("Name") != null &&
        extendee.GetType().GetProperty("Enabled") != null;
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

    private void InitializeBindingSourceInfos(BindingSource bindingSource, Component component)
    {
      if (bindingSource == null)
        return;
      List<Component> components;
      if (!_bindingSourceInfos.TryGetValue(bindingSource, out components))
      {
        components = new List<Component>();
        _bindingSourceInfos[bindingSource] = components;
      }
      if (!components.Contains(component))
        components.Add(component);
    }

    private void InitializeRule()
    {
      foreach (KeyValuePair<Component, ComponentRuleStatus> kvp in _ruleStatuses)
        foreach (ExecuteRule executeRule in kvp.Value.ExecuteRules)
          InitializeBindingSourceInfos(executeRule.BindingSource, kvp.Key);
      foreach (KeyValuePair<BindingSource, List<Component>> kvp in _bindingSourceInfos)
        kvp.Key.CurrentChanged += new EventHandler(BindingSource_CurrentChanged);
      DoResetComponentAuthorizationRules(true, true);
    }

    /// <summary>
    /// 是否拒绝执行
    /// </summary>
    public bool DenyExecute(Component component)
    {
      if (component == null)
        return false;
      if (DenyExecuteRule(component))
        return true;
      return DenyExecuteRights(component);
    }

    private bool DenyExecuteRule(Component component)
    {
      ComponentRuleStatus status;
      if (_ruleStatuses.TryGetValue(component, out status))
        foreach (ExecuteRule item in status.ExecuteRules)
          if (item.DenyExecute())
            return true;
      return false;
    }

    private bool DenyExecuteRights(Component component)
    {
      if (ClassInfo != null && ClassInfo.Authorised)
      {
        AssemblyClassMethodInfo classMethodInfo = ClassInfo.GetClassMethodInfo(FindName(component));
        if (classMethodInfo != null)
          return classMethodInfo.DenyExecute();
      }
      return false;
    }

    private bool? IsAllowVisible(Component component)
    {
      if (ClassInfo != null && ClassInfo.Authorised)
      {
        AssemblyClassMethodInfo classMethodInfo = ClassInfo.GetClassMethodInfo(FindName(component));
        if (classMethodInfo != null)
          return classMethodInfo.AllowVisible;
      }
      return null;
    }

    /// <summary>
    /// 重置组件的执行授权
    /// component = null
    /// </summary>
    public void ResetComponentAuthorizationRules()
    {
      ResetComponentAuthorizationRules(null);
    }

    /// <summary>
    /// 重置组件的执行授权
    /// </summary>
    /// <param name="component">组件</param>
    public void ResetComponentAuthorizationRules(Component component)
    {
      if (Host.InvokeRequired)
        Host.BeginInvoke(new ExecuteResetComponentAuthorizationRulesDelegate(ExecuteResetComponentAuthorizationRules),
          new object[] { component });
      else
        ExecuteResetComponentAuthorizationRules(component);
    }
    private delegate void ExecuteResetComponentAuthorizationRulesDelegate(Component component);
    private void ExecuteResetComponentAuthorizationRules(Component component)
    {
      if (component == null)
        DoResetComponentAuthorizationRules(false, false);
      else
        DoResetComponentAuthorizationRules(component, false, false);
    }

    private void DoResetComponentAuthorizationRules(bool keepDisabled, bool keepInvisible)
    {
      foreach (KeyValuePair<Component, ComponentAuthorizationStatus> kvp in _authorizationStatuses)
        DoResetComponentAuthorizationRules(kvp.Key, keepDisabled, keepInvisible);
      foreach (KeyValuePair<Component, ComponentRuleStatus> kvp in _ruleStatuses)
        DoResetComponentAuthorizationRules(kvp.Key, keepDisabled, keepInvisible);
    }

    private void DoResetComponentAuthorizationRules(Component component, bool keepDisabled, bool keepInvisible)
    {
      Control control = component as Control;
      if (control != null)
        control.EnabledChanged -= new EventHandler(Control_EnabledChanged);
      try
      {
        if (DenyExecute(component))
        {
          ApplyEnabledRule(component, false, keepDisabled);
          ComponentAuthorizationStatus status;
          if (_authorizationStatuses.TryGetValue(component, out status))
          {
            bool? isAllowVisible = IsAllowVisible(component);
            if (isAllowVisible.HasValue)
            {
              if (!isAllowVisible.Value)
                ApplyVisibleRule(component, false, keepInvisible);
            }
            else if (!status.AllowVisible)
              ApplyVisibleRule(component, false, keepInvisible);
          }
        }
        else
        {
          ApplyEnabledRule(component, true, keepDisabled);
          ApplyVisibleRule(component, true, keepInvisible);
        }
      }
      finally
      {
        if (control != null)
          control.EnabledChanged += new EventHandler(Control_EnabledChanged);
      }
    }

    private static string FindName(Component component)
    {
      Type type = component.GetType();
      PropertyInfo propertyInfo = type.GetProperty("Name");
      if (propertyInfo != null)
        return propertyInfo.GetValue(component, null) as string;
      return String.Empty;
    }

    private static string FindCaption(Component component)
    {
      Type type = component.GetType();
      PropertyInfo propertyInfo = type.GetProperty("Text");
      if (propertyInfo != null)
        return propertyInfo.GetValue(component, null) as string;
      propertyInfo = type.GetProperty("Caption");
      if (propertyInfo != null)
        return propertyInfo.GetValue(component, null) as string;
      return String.Empty;
    }

    private static string FindTag(Component component)
    {
      Type type = component.GetType();
      PropertyInfo propertyInfo = type.GetProperty("Tag");
      if (propertyInfo != null)
        return propertyInfo.GetValue(component, null) as string;
      return String.Empty;
    }

    private static void ApplyEnabledRule(Component component, bool enabled, bool keepDisabled)
    {
      Type type = component.GetType();
      PropertyInfo propertyInfo = type.GetProperty("Enabled");
      if (propertyInfo != null)
      {
        if (enabled && keepDisabled)
        {
          if (!(bool)propertyInfo.GetValue(component, null))
            return;
        }
        propertyInfo.SetValue(component, enabled, null);
      }
    }

    private static void ApplyVisibleRule(Component component, bool visible, bool keepInvisible)
    {
      Type type = component.GetType();
      PropertyInfo propertyInfo;
      //for Developer Express .NET
      propertyInfo = type.GetProperty("Visibility");
      if (propertyInfo != null)
      {
        if (visible && keepInvisible)
        {
          if ((int)propertyInfo.GetValue(component, null) == 1)  //BarItemVisibility.Never
            return;
        }
        propertyInfo.SetValue(component, visible ? 0 : 1, null); //BarItemVisibility.Always : BarItemVisibility.Never
        return;
      }
      //for VS
      propertyInfo = type.GetProperty("Visible");
      if (propertyInfo != null)
      {
        if (visible && keepInvisible)
        {
          if (!(bool)propertyInfo.GetValue(component, null))
            return;
        }
        propertyInfo.SetValue(component, visible, null);
        return;
      }
    }

    private void RegisterComponentAuthorizationRules()
    {
      if (UserIdentity.CurrentIdentity == null ||
        UserIdentity.CurrentIdentity.IsAdmin && AppConfig.Debugging && ClassInfo == null)
      {
        List<string> methodNames = new List<string>(_authorizationStatuses.Count);
        List<string> methodCaptions = new List<string>(_authorizationStatuses.Count);
        List<string> methodTags = new List<string>(_authorizationStatuses.Count);
        List<bool> allowVisibles = new List<bool>(_authorizationStatuses.Count);
        foreach (KeyValuePair<Component, ComponentAuthorizationStatus> kvp in _authorizationStatuses)
          if (kvp.Value.Apply)
          {
            methodNames.Add(FindName(kvp.Key));
            methodCaptions.Add(FindCaption(kvp.Key));
            methodTags.Add(FindTag(kvp.Key));
            allowVisibles.Add(kvp.Value.AllowVisible);
          }
        if (methodNames.Count > 0)
        {
          Type type = Host.GetType();
          DataDictionaryHub.AddAssemblyClassInfo(type, Host.Text, AssemblyClassType.Form);
          DataDictionaryHub.AddAssemblyClassMethodInfos(type, methodNames.ToArray(), methodCaptions.ToArray(), methodTags.ToArray(), allowVisibles.ToArray());
        }
      }
    }

    internal string RuleMessage()
    {
      string result = String.Empty;
      foreach (KeyValuePair<Component, ComponentAuthorizationStatus> kvp in _authorizationStatuses)
        if (kvp.Value.Apply)
        {
          result += FindName(kvp.Key) + "(" + FindCaption(kvp.Key) + "," + FindTag(kvp.Key) + "):" + Environment.NewLine;
          result += kvp.Value.ToString() + Environment.NewLine + Environment.NewLine;
        }
      foreach (KeyValuePair<Component, ComponentRuleStatus> kvp in _ruleStatuses)
        if (kvp.Value.ExecuteRules.Count > 0)
        {
          result += FindName(kvp.Key) + "(" + FindCaption(kvp.Key) + "," + FindTag(kvp.Key) + "):" + Environment.NewLine;
          result += kvp.Value.ToString() + Environment.NewLine + Environment.NewLine;
        }
      return result;
    }

    #endregion

    #region 内嵌类

    [Serializable]
    private class ComponentAuthorizationStatus
    {
      public bool Apply { get; set; }

      private bool _allowVisible = true;
      public bool AllowVisible
      {
        get { return _allowVisible; }
        set { _allowVisible = value; }
      }

      public override string ToString()
      {
        return "Apply Authorization && AllowVisible" + AppConfig.EQUAL_SEPARATOR + AllowVisible.ToString();
      }
    }

    [Serializable]
    private class ComponentRuleStatus
    {
      private readonly Collection<ExecuteRule> _executeRules = new Collection<ExecuteRule>();
      public Collection<ExecuteRule> ExecuteRules
      {
        get { return _executeRules; }
      }

      public override string ToString()
      {
        string result = String.Empty;
        foreach (ExecuteRule item in ExecuteRules)
          result += item.ToString() + Environment.NewLine;
        return result;
      }
    }

    #endregion
  }
}