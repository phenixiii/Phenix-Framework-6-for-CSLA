using System;
using Phenix.Core.Mapping;
using Phenix.Core.Security;

namespace Phenix.Core.Rule
{
  /// <summary>
  /// 数据规则接口
  /// </summary>
  public interface IDataRule
  {
    #region 方法

    #region PromptCode

    /// <summary>
    /// 取提示码队列
    /// </summary>
    PromptCodeKeyCaptionCollection GetPromptCodes(string name, UserIdentity identity);

    /// <summary>
    /// 取提示码更新时间
    /// </summary>
    DateTime? GetPromptCodeChangedTime(string name);

    /// <summary>
    /// 提示码已更新
    /// </summary>
    void PromptCodeHasChanged(string name);

    /// <summary>
    /// 添加提示码
    /// </summary>
    bool AddPromptCode(string name, string key, string caption, string value, ReadLevel readLevel, UserIdentity identity);

    /// <summary>
    /// 移除提示码
    /// </summary>
    void RemovePromptCode(string name, string key);

    #endregion

    #region CriteriaExpression

    /// <summary>
    /// 取条件表达式队列
    /// </summary>
    CriteriaExpressionKeyCaptionCollection GetCriteriaExpressions(string name, UserIdentity identity);

    /// <summary>
    /// 取条件表达式更新时间
    /// </summary>
    DateTime? GetCriteriaExpressionChangedTime(string name);

    /// <summary>
    /// 条件表达式已更新
    /// </summary>
    void CriteriaExpressionHasChanged(string name);

    /// <summary>
    /// 添加条件表达式
    /// </summary>
    bool AddCriteriaExpression(string name, string key, string caption, CriteriaExpression value, ReadLevel readLevel, UserIdentity identity);

    /// <summary>
    /// 移除条件表达式
    /// </summary>
    void RemoveCriteriaExpression(string name, string key);
    
    #endregion

    #endregion
  }
}
