using System;
using Phenix.Core.Data;
using Phenix.Core.Operate;
using Phenix.Core.Security;

namespace Phenix.Core.Rule
{
  /// <summary>
  /// 提示码"键-标签"
  /// 主要用于填充下拉列表框内容
  /// </summary>
  [Serializable]
  public sealed class PromptCodeKeyCaption : KeyCaption<PromptCodeKeyCaption, String>, IKeyCaption, ISecurityInfo
  {
    /// <summary>
    /// 初始化
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public PromptCodeKeyCaption(string key, string caption, string value, ReadLevel readLevel, DateTime addtime, string userNumber)
      : base(key, caption, value)
    {
      _readLevel = readLevel;
      _addtime = addtime;
      _userNumber = userNumber;
    }

    internal PromptCodeKeyCaption(ReadLevel readLevel, IIdentity identity)
      : this(Sequence.Value.ToString(), String.Empty, String.Empty, readLevel, DateTime.Now, identity.UserNumber) { }

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
  }
}