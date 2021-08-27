using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;
using System.Windows.Forms;
using Phenix.Core.Dictionary;
using Phenix.Core.Operate;

namespace Phenix.Services.Client.Design
{
  /// <summary>
  /// 类选择编辑器
  /// </summary>
  public abstract class ClassSelectorEditor : ObjectSelectorEditor
  {
    /// <summary>
    /// 类选择编辑器
    /// </summary>
    protected ClassSelectorEditor()
      : base(true) { }

    #region 属性

    /// <summary>
    /// 指示是否应该由用户调整下拉编辑器的大小
    /// </summary>
    public override bool IsDropDownResizable
    {
      get { return true; }
    }

    /// <summary>
    /// 程序集类类型
    /// </summary>
    protected virtual AssemblyClassType AssemblyClassType 
    { 
      get { return AssemblyClassType.Ordinary; }
    }

    /// <summary>
    /// 所继承的类
    /// </summary>
    protected abstract Type SubclassOfType { get; }

    #endregion

    #region 方法

    /// <summary>
    /// 填充标记项的分层集合
    /// 每个标记项用一个 System.Windows.Forms.TreeNode 来表示
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    protected override void FillTreeWithData(Selector selector,
      ITypeDescriptorContext context, IServiceProvider provider)
    {
      base.FillTreeWithData(selector, context, provider);

      List<string> assemblyNames = new List<string>();
      SortedList<string, SelectorNode> nodes = new SortedList<string, SelectorNode>();
      foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        try
        {
          string assemblyName = assembly.GetName().Name;
          if (Phenix.Core.AppUtilities.IsNotApplicationAssembly(assembly) || assemblyNames.Contains(assemblyName))
            continue;
          assemblyNames.Add(assemblyName);

          nodes.Clear();
          if (AssemblyClassType == AssemblyClassType.Enum)
          {
            foreach (Type type in assembly.GetExportedTypes())
              try
              {
                if (type.IsEnum)
                {
                  KeyCaptionAttribute keyCaptionAttribute = (KeyCaptionAttribute)Attribute.GetCustomAttribute(type, typeof(KeyCaptionAttribute));
                  if (keyCaptionAttribute != null)
                    nodes.Add(type.FullName, new SelectorNode(type.FullName, type));
                }
              }
              catch (TypeLoadException)
              {
                // ignored
              }
          }
          else
          {
            foreach (Type type in assembly.GetExportedTypes())
              try
              {
                if (!type.IsClass || type.IsAbstract || type.IsGenericType || type.IsCOMObject)
                  continue;
                if (SubclassOfType != null && type.IsSubclassOf(SubclassOfType))
                  nodes.Add(type.FullName, new SelectorNode(type.FullName, type));
                else
                {
                  DataDictionaryAttribute attribute = (DataDictionaryAttribute)Attribute.GetCustomAttribute(type, typeof(DataDictionaryAttribute));
                  if (attribute != null && attribute.ClassType == AssemblyClassType)
                    nodes.Add(type.FullName, new SelectorNode(type.FullName, type));
                }
              }
              catch (TypeLoadException)
              {
                // ignored
              }
          }
          if (nodes.Count > 0)
          {
            SelectorNode node = new SelectorNode(assemblyName, null);
            foreach (KeyValuePair<string, SelectorNode> kvp in nodes)
              node.Nodes.Add(kvp.Value);
            selector.Nodes.Add(node);
          }
          selector.ExpandAll();
        }
        catch (TypeLoadException)
        {
          // ignored
        }
        catch (NotSupportedException)
        {
          // ignored
        }
        catch (ArgumentException)
        {
          // ignored
        }
        catch (Exception ex)
        {
          if (MessageBox.Show(Phenix.Core.AppUtilities.GetErrorMessage(ex),
            MethodBase.GetCurrentMethod().Name.Substring(4), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
            break;
        }
    }

    #endregion
  }
}
