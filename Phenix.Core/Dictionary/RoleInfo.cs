using System;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// 角色资料
  /// </summary>
  [Serializable]
  public sealed class RoleInfo
  {
    /// <summary>
    /// 初始化
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    [Newtonsoft.Json.JsonConstructor]
    public RoleInfo(string name, string caption) 
    {
      _name = name;
      _caption = caption;
    }

    #region 属性

    private readonly string _name;
    /// <summary>
    /// 名称
    /// </summary>
    public string Name
    {
      get { return _name; }
    }

    private readonly string _caption;
    /// <summary>
    /// 标签
    /// </summary>
    public string Caption
    {
      get { return _caption; }
    }

    #endregion
  }
}
