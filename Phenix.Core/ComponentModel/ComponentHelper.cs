using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Phenix.Core.Mapping;

namespace Phenix.Core.ComponentModel
{
  /// <summary>
  /// Component助手
  /// </summary>
  public static class ComponentHelper
  {
    #region 方法
    
    /// <summary>
    /// 检索指定类型T的全部组件
    /// </summary>
    /// <param name="container">组件容器</param>
    /// <returns>匹配的组件队列</returns>
    public static IList<T> FindComponents<T>(IContainer container)
      where T : IComponent
    {
      List<T> result = new List<T>();
      if (container != null)
        foreach (IComponent item in container.Components)
          if (item is T)
            result.Add((T)item);
      return result;
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    /// <param name="container">组件容器</param>
    /// <param name="component">组件</param>
    /// <param name="defaultName">缺省名称</param>
    public static string AddComponent(IContainer container, IComponent component, string defaultName)
    {
      if (container == null)
        throw new ArgumentNullException("container");
      if (String.IsNullOrEmpty(defaultName))
        defaultName = component.GetType().Name;
      defaultName = CodingStandards.GetParameterByFieldName(defaultName);
      int i = 0;
      string name = new Regex(@"\W").Replace(defaultName, ""); //\W: 匹配任何非单词字符。等价于“[^A-Za-z0-9_]”
      do
      {
        try
        {
          container.Add(component, name);
          break;
        }
        catch (ArgumentException)
        {
          i = i + 1;
          name = defaultName + i.ToString();
        }
      } while (true);
      return name;
    }
    
    #endregion
  }
}