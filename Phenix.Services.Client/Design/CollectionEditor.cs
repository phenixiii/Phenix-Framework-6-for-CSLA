using System;
using System.Reflection;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Phenix.Services.Client.Design
{
  /// <summary>
  /// 集合编辑器
  /// </summary>
  public class CollectionEditor : System.ComponentModel.Design.CollectionEditor
  {
    /// <summary>
    /// 集合编辑器
    /// </summary>
    public CollectionEditor(Type type)
      : base(type) { }

    #region 方法

    /// <summary>
    /// 创建新的窗体以显示和编辑当前集合
    /// </summary>
    [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
    protected override CollectionForm CreateCollectionForm()
    {
      CollectionForm result = base.CreateCollectionForm();
      //属性编辑栏可见帮助
      FieldInfo fieldInfo = result.GetType().GetField("propertyBrowser", BindingFlags.NonPublic | BindingFlags.Instance);
      if (fieldInfo != null)
      {
        PropertyGrid grid = (PropertyGrid)fieldInfo.GetValue(result);
        grid.HelpVisible = true;
      }
      return result;
    }

    #endregion
  }
}
