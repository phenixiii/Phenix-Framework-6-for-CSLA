using System;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// ��ɫ����
  /// </summary>
  [Serializable]
  public sealed class RoleInfo
  {
    /// <summary>
    /// ��ʼ��
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    [Newtonsoft.Json.JsonConstructor]
    public RoleInfo(string name, string caption) 
    {
      _name = name;
      _caption = caption;
    }

    #region ����

    private readonly string _name;
    /// <summary>
    /// ����
    /// </summary>
    public string Name
    {
      get { return _name; }
    }

    private readonly string _caption;
    /// <summary>
    /// ��ǩ
    /// </summary>
    public string Caption
    {
      get { return _caption; }
    }

    #endregion
  }
}
