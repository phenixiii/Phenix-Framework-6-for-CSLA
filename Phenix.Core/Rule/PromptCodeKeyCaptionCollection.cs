using System;
using Phenix.Core.Operate;
using Phenix.Core.Security;

namespace Phenix.Core.Rule
{
  /// <summary>
  /// 提示码"键-标签"数组
  /// 主要用于填充下拉列表框内容
  /// </summary>
  [Serializable]
  public sealed class PromptCodeKeyCaptionCollection : KeyCaptionCollection<PromptCodeKeyCaption, String>
  {
    /// <summary>
    /// 初始化
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public PromptCodeKeyCaptionCollection(string name, DateTime? actionTime)
      : base()
    {
      _name = name;
      _actionTime = actionTime;
    }

    #region 工厂

    /// <summary>
    /// 构建填充
    /// </summary>
    /// <param name="name">名称</param>
    public static PromptCodeKeyCaptionCollection Fetch(string name)
    {
      return DataRuleHub.GetPromptCodes(name);
    }

    /// <summary>
    /// 构建填充
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static PromptCodeKeyCaptionCollection Fetch(string name, UserIdentity identity)
    {
      return DataRuleHub.GetPromptCodes(name, identity);
    }

    #endregion

    #region 属性

    private readonly string _name;
    /// <summary>
    /// 名称
    /// </summary>
    public string Name
    {
      get { return _name; }
    }

    private readonly DateTime? _actionTime;
    /// <summary>
    /// 活动时间
    /// </summary>
    public DateTime? ActionTime
    {
      get { return _actionTime; }
    }

    /// <summary>
    /// 标签
    /// </summary>
    public override string Caption
    {
      get { return Phenix.Core.Properties.Resources.PromptCodeFriendlyName; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 克隆
    /// </summary>
    public new PromptCodeKeyCaptionCollection Clone()
    {
      return (PromptCodeKeyCaptionCollection)base.Clone();
    }

    /// <summary>
    /// 新建
    /// </summary>
    public PromptCodeKeyCaption CreatePromptCodeKeyCaption(ReadLevel readLevel)
    {
      return CreatePromptCodeKeyCaption(readLevel, UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 新建
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    public PromptCodeKeyCaption CreatePromptCodeKeyCaption(ReadLevel readLevel, IIdentity identity)
    {
      if (identity == null)
        throw new ArgumentNullException("identity");
      return new PromptCodeKeyCaption(readLevel, identity);
    }

    /// <summary>
    /// 保存
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    public void Save(PromptCodeKeyCaption promptCode)
    {
      Save(promptCode, UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 保存
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Save(PromptCodeKeyCaption promptCode, UserIdentity identity)
    {
      DataRuleHub.AddPromptCode(Name, promptCode, identity);
      PromptCodeKeyCaption item = FindByKey(promptCode.Key);
      if (item != null)
      {
        item.Caption = promptCode.Caption;
        item.Value = promptCode.Value;
        item.ReadLevel = promptCode.ReadLevel;
      }
      else
        base.Add(promptCode);
    }

    /// <summary>
    /// 移除
    /// 根据 Key 匹配
    /// </summary>
    /// <param name="promptCode">提示码</param>
    public new bool Remove(PromptCodeKeyCaption promptCode)
    {
      if (promptCode == null)
        throw new ArgumentNullException("promptCode");
      DataRuleHub.RemovePromptCode(Name, promptCode.Key);
      return base.Remove(FindByKey(promptCode.Key));
    }

    /// <summary>
    /// 比较类型
    /// 主要用于IDE环境
    /// </summary>
    public static bool Equals(Type objectType)
    {
      if (objectType == null)
        return false;
      return String.CompareOrdinal(objectType.FullName, typeof(PromptCodeKeyCaptionCollection).FullName) == 0;
    }

    #endregion
  }
}
