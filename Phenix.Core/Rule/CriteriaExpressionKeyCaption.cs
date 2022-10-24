using System;
using Phenix.Core.Data;
using Phenix.Core.Mapping;
using Phenix.Core.Operate;
using Phenix.Core.Security;

namespace Phenix.Core.Rule
{
  /// <summary>
  /// 条件表达式"键-标签"
  /// 主要用于填充下拉列表框内容
  /// </summary>
  [Serializable]
  public sealed class CriteriaExpressionKeyCaption : KeyCaption<CriteriaExpressionKeyCaption, CriteriaExpression>, IKeyCaption, ISecurityInfo
  {
    /// <summary>
    /// 初始化
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public CriteriaExpressionKeyCaption(string key, string caption, CriteriaExpression value, ReadLevel readLevel, DateTime addtime, string userNumber)
      : base(key, caption ?? Phenix.Core.Properties.Resources.CriteriaFriendlyName, value)
    {
      _readLevel = readLevel;
      _addtime = addtime;
      _userNumber = userNumber;
    }

    private CriteriaExpressionKeyCaption(CriteriaExpression value, ReadLevel readLevel, IIdentity identity)
      : this(Sequence.Value.ToString(), Phenix.Core.Properties.Resources.CriteriaFriendlyName, value, readLevel, DateTime.Now, identity.UserNumber) { }

    #region 属性

    /// <summary>
    /// 键
    /// </summary>
    public new string Key
    {
      get { return base.Key as string; }
    }
    object IKeyCaption.Key
    {
      get { return Key; }
    }

    private ReadLevel _readLevel;
    /// <summary>
    /// 读取级别
    /// </summary>
    public ReadLevel ReadLevel
    {
      get { return _readLevel; }
      set
      {
        if (_readLevel == value)
          return;
        _readLevel = value;
        PropertyHasChanged();
      }
    }

    private readonly DateTime _addtime;
    /// <summary>
    /// 添加时间
    /// </summary>
    public DateTime Addtime
    {
      get { return _addtime; }
    }

    private readonly string _userNumber;
    /// <summary>
    /// 登录工号
    /// </summary>
    public string UserNumber
    {
      get { return _userNumber; }
    }

    #endregion
    
    #region 方法

    /// <summary>
    /// 新建条件表达式"键-标签"
    /// </summary>
    public static CriteriaExpressionKeyCaption New(CriteriaExpressionPropertyKeyCaptionCollection criteriaExpressionProperties, ReadLevel readLevel, IIdentity identity)
    {
      CriteriaExpression value = criteriaExpressionProperties.BuildSelectedCriteriaExpressionGroup();
      return (object)value != null ? new CriteriaExpressionKeyCaption(value, readLevel, identity) : null;
    }
    
    /// <summary>
    /// 添加条件表达式组
    /// (False or Group_N or Group_1) And Group0
    /// </summary>
    /// <param name="criteriaExpressionProperties">条件表达式属性</param>
    public CriteriaExpression AddCriteriaExpressionGroup(CriteriaExpressionPropertyKeyCaptionCollection criteriaExpressionProperties)
    {
      CriteriaExpression result = criteriaExpressionProperties.BuildSelectedCriteriaExpressionGroup();
      if ((object)result == null)
        return null;
      if ((object)Value == null)
        Value = result;
      else if (Value.Left.CriteriaExpressionType == CriteriaExpressionType.CriteriaOperate)
        Value = new CriteriaExpression(new CriteriaExpression(CriteriaExpression.False, CriteriaLogical.Or, result), CriteriaLogical.And, Value);
      else
      {
        CriteriaExpression p = Value;
        do
        {
          if (p.Left.CriteriaExpressionType == CriteriaExpressionType.Short)
          {
            p.Left = new CriteriaExpression(p.Left, CriteriaLogical.Or, result);
            break;
          }
          p = p.Left;
        } while (true);
      }
      return result;
    }

    /// <summary>
    /// 移除条件表达式组
    /// </summary>
    /// <param name="criteriaExpressionGroup">条件表达式组</param>
    public bool RemoveCriteriaExpressionGroup(CriteriaExpression criteriaExpressionGroup)
    {
      if ((object)Value == null || (object)Value == (object)criteriaExpressionGroup || (object)Value.Right == (object)criteriaExpressionGroup)
        return false;
      CriteriaExpression p = Value;
      do
      {
        if ((object)p.Left.Right == (object)criteriaExpressionGroup)
        {
          p.Left = p.Left.Left;
          return true;
        }
        p = p.Left;
      } while (p.Left.CriteriaExpressionType != CriteriaExpressionType.Short);
      return false;
    }

    #endregion
  }
}