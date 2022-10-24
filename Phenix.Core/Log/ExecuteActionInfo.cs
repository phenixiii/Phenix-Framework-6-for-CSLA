using System;
using Phenix.Core.Mapping;
using Phenix.Core.Rule;

namespace Phenix.Core.Log
{
  /// <summary>
  /// 执行动作信息
  /// </summary>
  public sealed class ExecuteActionInfo
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="time">时间</param>
    /// <param name="userNumber">登录工号</param>
    /// <param name="entityCaption">实体标签</param>
    /// <param name="action">执行动作</param>
    /// <param name="fieldMapInfo">数据映射字段信息</param>
    /// <param name="oldFieldValue">旧字段值</param>
    /// <param name="newFieldValue">新字段值</param>
    public ExecuteActionInfo(DateTime time, string userNumber, string entityCaption, ExecuteAction action,
      FieldMapInfo fieldMapInfo, object oldFieldValue, object newFieldValue)
    {
      _time = time;
      _userNumber = userNumber;
      _entityCaption = entityCaption;
      _action = action;
      _fieldMapInfo = fieldMapInfo;
      _friendlyName = fieldMapInfo.FriendlyName;
      _oldFieldValue = oldFieldValue;
      _newFieldValue = newFieldValue;
    }

    #region 属性

    private readonly DateTime _time;
    /// <summary>
    /// 时间
    /// </summary>
    public DateTime Time
    {
      get { return _time; }
    }

    private readonly string _userNumber;
    /// <summary>
    /// 登录工号
    /// </summary>
    public string UserNumber
    {
      get { return _userNumber; }
    }

    private readonly string _entityCaption;
    /// <summary>
    /// 实体标签
    /// </summary>
    public string EntityCaption
    {
      get { return _entityCaption; }
    }

    private readonly ExecuteAction _action;
    /// <summary>
    /// 执行动作
    /// </summary>
    public ExecuteAction Action
    {
      get { return _action; }
    }
    /// <summary>
    /// 执行动作
    /// </summary>
    public string ActionCaption
    {
      get { return EnumKeyCaption.GetCaption(_action); }
    }

    private readonly FieldMapInfo _fieldMapInfo;
    /// <summary>
    /// 数据映射字段信息
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public FieldMapInfo FieldMapInfo
    {
      get { return _fieldMapInfo; }
    }

    private readonly string _friendlyName;
    /// <summary>
    /// 友好名
    /// </summary>
    public string FriendlyName
    {
      get { return _friendlyName; }
    }

    private readonly object _oldFieldValue;
    /// <summary>
    /// 旧字段值
    /// </summary>
    public object OldFieldValue
    {
      get { return _oldFieldValue; }
    }

    private readonly object _newFieldValue;
    /// <summary>
    /// 新字段值
    /// </summary>
    public object NewFieldValue
    {
      get { return _newFieldValue; }
    }

    /// <summary>
    /// 变更信息
    /// </summary>
    public string ChangeInfo
    {
      get
      {
        if ((Action & ExecuteAction.Insert) == ExecuteAction.Insert)
          return String.Format("{0}={1}", FriendlyName, NewFieldValue);
        if ((Action & ExecuteAction.Update) == ExecuteAction.Update)
          return String.Format("{0}={1}({2})", FriendlyName, NewFieldValue, OldFieldValue);
        return String.Format("{0}={1}", FriendlyName, OldFieldValue);
      }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 字符串表示
    /// </summary>
    public override string ToString()
    {
      return String.Format("*{0}[{1}] {2} {3} {4}", Time, UserNumber, ActionCaption, EntityCaption, ChangeInfo);
    }

    #endregion
  }
}
