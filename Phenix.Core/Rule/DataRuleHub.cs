using System;
using System.Reflection;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Security;
using Phenix.Core.SyncCollections;

namespace Phenix.Core.Rule
{
  /// <summary>
  /// 申明提示码更新后事件处理函数
  /// </summary>
  /// <param name="promptCodeName">名称</param>
  public delegate void PromptCodeChangedEventHandler(string promptCodeName);

  /// <summary>
  /// 申明条件表达式更新后事件处理函数
  /// </summary>
  /// <param name="criteriaExpressionName">名称</param>
  public delegate void CriteriaExpressionChangedEventHandler(string criteriaExpressionName);

  /// <summary>
  /// 数据规则中心
  /// </summary>
  public static class DataRuleHub
  {
    #region 属性

    private static IDataRule _worker;
    /// <summary>
    /// 实施者
    /// </summary>
    public static IDataRule Worker
    {
      get
      {
        if (_worker == null)
          AppUtilities.RegisterWorker();
        return _worker;
      }
      set { _worker = value; }
    }

    private static readonly SynchronizedDictionary<string, PromptCodeKeyCaptionCollection> _promptCodesCache =
      new SynchronizedDictionary<string, PromptCodeKeyCaptionCollection>(StringComparer.Ordinal);

    private static readonly SynchronizedDictionary<string, CriteriaExpressionKeyCaptionCollection> _criteriaExpressionsCache =
      new SynchronizedDictionary<string, CriteriaExpressionKeyCaptionCollection>(StringComparer.Ordinal);

    #endregion

    #region 事件

    /// <summary>
    /// 提示码更新后事件
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public static event PromptCodeChangedEventHandler PromptCodeChanged;
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private static void OnPromptCodeChanged(string promptCodeName)
    {
      if (PromptCodeChanged == null)
        return;
      foreach (PromptCodeChangedEventHandler item in PromptCodeChanged.GetInvocationList())
        try
        {
          item.Invoke(promptCodeName);
        }
        catch (Exception ex)
        {
          EventLog.SaveLocal(MethodBase.GetCurrentMethod(), promptCodeName, ex);
          PromptCodeChanged -= item;
        }
    }

    /// <summary>
    /// 条件表达式更新后事件
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public static event CriteriaExpressionChangedEventHandler CriteriaExpressionChanged;
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private static void OnCriteriaExpressionChanged(string criteriaExpressionName)
    {
      if (CriteriaExpressionChanged == null)
        return;
      foreach (CriteriaExpressionChangedEventHandler item in CriteriaExpressionChanged.GetInvocationList())
        try
        {
          item.Invoke(criteriaExpressionName);
        }
        catch (Exception ex)
        {
          EventLog.SaveLocal(MethodBase.GetCurrentMethod(), criteriaExpressionName, ex);
          CriteriaExpressionChanged -= item;
        }
    }

    #endregion

    #region 方法

    internal static void ClearCache()
    {
      _promptCodesCache.Clear();
      _criteriaExpressionsCache.Clear();
    }

    /// <summary>
    /// 检查活动
    /// </summary>
    public static void CheckActive()
    {
      if (Worker == null)
      {
        Exception ex = new InvalidOperationException(Phenix.Core.Properties.Resources.NoService);
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
        throw ex;
      }
    }

    #region PromptCode

    /// <summary>
    /// 取提示码队列
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    public static PromptCodeKeyCaptionCollection GetPromptCodes(string name)
    {
      return GetPromptCodes(name, UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 取提示码队列
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static PromptCodeKeyCaptionCollection GetPromptCodes(string name, UserIdentity identity)
    {
      identity = identity ?? UserIdentity.CurrentIdentity;
      if (identity == null)
        return new PromptCodeKeyCaptionCollection(name, null);
      PromptCodeKeyCaptionCollection result;
      string key = String.Format("{0},{1}", name, identity.UserNumber);
      if (_promptCodesCache.TryGetValue(key, out result))
        if (result.ActionTime.HasValue && result.ActionTime.Value >= GetPromptCodeChangedTime(name))
          return result;
      try
      {
        CheckActive();
        result = Worker.GetPromptCodes(name, identity);
        _promptCodesCache[key] = result;
        return result;
      }
      finally
      {
        OnPromptCodeChanged(name);
      }
    }

    /// <summary>
    /// 取提示码更新时间
    /// </summary>
    public static DateTime? GetPromptCodeChangedTime(string name)
    {
      if (Worker == null)
        return null;
      return Worker.GetPromptCodeChangedTime(name);
    }

    /// <summary>
    /// 提示码已更新
    /// </summary>
    public static void PromptCodeHasChanged(string name)
    {
      try
      {
        CheckActive();
        Worker.PromptCodeHasChanged(name);
      }
      finally
      {
        OnPromptCodeChanged(name);
      }
    }

    /// <summary>
    /// 添加提示码
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    public static bool AddPromptCode(string name, PromptCodeKeyCaption promptCode)
    {
      return AddPromptCode(name, promptCode, UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 添加提示码
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static bool AddPromptCode(string name, PromptCodeKeyCaption promptCode, UserIdentity identity)
    {
      if (promptCode == null)
        throw new ArgumentNullException("promptCode");
      return AddPromptCode(name, promptCode.Key, promptCode.Caption, promptCode.Value, promptCode.ReadLevel, identity);
    }

    /// <summary>
    /// 添加提示码
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static bool AddPromptCode(string name, string key, string caption, string value, ReadLevel readLevel, UserIdentity identity)
    {
      identity = identity ?? UserIdentity.CurrentIdentity;
      try
      {
        CheckActive();
        return Worker.AddPromptCode(name, key, caption, value, readLevel, identity);
      }
      finally
      {
        OnPromptCodeChanged(name);
      }
    }

    /// <summary>
    /// 移除提示码
    /// </summary>
    public static void RemovePromptCode(string name, string key)
    {
      try
      {
        CheckActive();
        Worker.RemovePromptCode(name, key);
      }
      finally
      {
        OnPromptCodeChanged(name);
      }
    }

    /// <summary>
    /// 移除提示码
    /// </summary>
    public static void RemovePromptCode(string name, PromptCodeKeyCaption promptCode)
    {
      if (promptCode == null)
        throw new ArgumentNullException("promptCode");
      RemovePromptCode(name, promptCode.Key);
    }

    #endregion

    #region CriteriaExpression

    /// <summary>
    /// 取条件表达式队列
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    public static CriteriaExpressionKeyCaptionCollection GetCriteriaExpressions(Type ownerType, string name)
    {
      return GetCriteriaExpressions(ownerType, name, UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 取条件表达式队列
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static CriteriaExpressionKeyCaptionCollection GetCriteriaExpressions(Type ownerType, string name, UserIdentity identity)
    {
      identity = identity ?? UserIdentity.CurrentIdentity;
      if (identity == null)
        return new CriteriaExpressionKeyCaptionCollection(name, null);
      CriteriaExpressionKeyCaptionCollection result;
      string key = String.Format("{0},{1},{2}", (ownerType != null ? ownerType.FullName : String.Empty), name, identity.UserNumber);
      if (_criteriaExpressionsCache.TryGetValue(key, out result))
        if (result.ActionTime.HasValue && result.ActionTime.Value >= GetCriteriaExpressionChangedTime(name))
          return result;
      try
      {
        CheckActive();
        result = Worker.GetCriteriaExpressions(name, identity);
        result.OwnerType = ownerType;
        _criteriaExpressionsCache[key] = result;
        return result;
      }
      finally
      {
        OnCriteriaExpressionChanged(name);
      }
    }

    /// <summary>
    /// 取条件表达式更新时间
    /// </summary>
    public static DateTime? GetCriteriaExpressionChangedTime(string name)
    {
      if (Worker == null)
        return null;
      return Worker.GetCriteriaExpressionChangedTime(name);
    }

    /// <summary>
    /// 条件表达式已更新
    /// </summary>
    public static void CriteriaExpressionHasChanged(string name)
    {
      try
      {
        CheckActive();
        Worker.CriteriaExpressionHasChanged(name);
      }
      finally
      {
        OnCriteriaExpressionChanged(name);
      }
    }

    /// <summary>
    /// 添加条件表达式
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    public static bool AddCriteriaExpression(string name, CriteriaExpressionKeyCaption criteriaExpression)
    {
      return AddCriteriaExpression(name, criteriaExpression, UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 添加条件表达式
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static bool AddCriteriaExpression(string name, CriteriaExpressionKeyCaption criteriaExpression, UserIdentity identity)
    {
      if (criteriaExpression == null)
        throw new ArgumentNullException("criteriaExpression");
      return AddCriteriaExpression(name, criteriaExpression.Key, criteriaExpression.Caption, criteriaExpression.Value, criteriaExpression.ReadLevel, identity);
    }

    /// <summary>
    /// 添加条件表达式
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static bool AddCriteriaExpression(string name, string key, string caption, CriteriaExpression value, ReadLevel readLevel, UserIdentity identity)
    {
      try
      {
        identity = identity ?? UserIdentity.CurrentIdentity;
        CheckActive();
        return Worker.AddCriteriaExpression(name, key, caption, value, readLevel, identity);
      }
      finally
      {
        OnCriteriaExpressionChanged(name);
      }
    }

    /// <summary>
    /// 移除条件表达式
    /// </summary>
    public static void RemoveCriteriaExpression(string name, string key)
    {
      try
      {
        CheckActive();
        Worker.RemoveCriteriaExpression(name, key);
      }
      finally
      {
        OnCriteriaExpressionChanged(name);
      }
    }

    /// <summary>
    /// 移除条件表达式
    /// </summary>
    public static void RemoveCriteriaExpression(string name, CriteriaExpressionKeyCaption criteriaExpression)
    {
      if (criteriaExpression == null)
        throw new ArgumentNullException("criteriaExpression");
      RemoveCriteriaExpression(name, criteriaExpression.Key);
    }

    #endregion

    #endregion
  }
}