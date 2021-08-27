using System.Collections;
using System.ComponentModel.Design;

namespace Phenix.Services.Client.Design
{
  /// <summary>
  /// 组件属性设计器
  /// </summary>
  public class ComponentPropertyDesigner : ComponentDesigner
  {
    #region 方法

    /// <summary>
    /// 允许设计器从通过 System.ComponentModel.TypeDescriptor 公开的属性集中更改或移除项
    /// </summary>
    protected override void PostFilterProperties(IDictionary properties)
    {
      properties.Remove("GenerateMember");
      base.PostFilterProperties(properties);
    }
    
    #endregion
  }
}
