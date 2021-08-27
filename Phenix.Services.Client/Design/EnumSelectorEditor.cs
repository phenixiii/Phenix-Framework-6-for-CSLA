using System;
using Phenix.Core.Dictionary;

namespace Phenix.Services.Client.Design
{
  /// <summary>
  /// 枚举选择编辑器
  /// </summary>
  public class EnumSelectorEditor : ClassSelectorEditor
  {
    #region 属性

    /// <summary>
    /// 程序集类类型
    /// </summary>
    protected override AssemblyClassType AssemblyClassType
    {
      get { return AssemblyClassType.Enum; }
    }

    /// <summary>
    /// 所继承的类
    /// </summary>
    protected override Type SubclassOfType
    {
      get { return null; }
    }

    #endregion
  }
}
