using System;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// "数据字典"标签
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public sealed class DataDictionaryAttribute : Attribute
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="classType">指示该类是什么程序集类类型</param>
    public DataDictionaryAttribute(AssemblyClassType classType)
      : base()
    {
      _classType = classType;
    }

    #region 属性

    private readonly AssemblyClassType _classType;
    /// <summary>
    /// 指示该类是什么程序集类类型
    /// </summary>
    public AssemblyClassType ClassType
    {
      get { return _classType; }
    }

    #endregion
  }
}