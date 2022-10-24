using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Phenix.Core.ComponentModel;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Reflection;

namespace Phenix.Core.Windows
{
  /// <summary>
  /// BindingSource助手
  /// </summary>
  public static class BindingSourceHelper
  {
    /// <summary>
    /// 获取数据源Current值
    /// </summary>
    /// <param name="source">数据源</param>
    /// <returns>Current值</returns>
    public static T GetDataSourceCurrent<T>(BindingSource source)
      where T : class 
    {
      return GetDataSourceCurrent(source) as T;
    }

    /// <summary>
    /// 获取数据源Current值
    /// </summary>
    /// <param name="source">数据源</param>
    /// <returns>Current值</returns>
    public static object GetDataSourceCurrent(BindingSource source)
    {
      if (source == null)
        return null;
      try
      {
        return source.Current;
      }
      catch (IndexOutOfRangeException)
      {
        return null;
      }
    }

    /// <summary>
    /// 获取数据源List值
    /// </summary>
    /// <param name="source">数据源</param>
    /// <returns>List值</returns>
    public static T GetDataSourceList<T>(BindingSource source)
      where T : IList
    {
      return (T)GetDataSourceList(source);
    }

    /// <summary>
    /// 获取数据源List值
    /// </summary>
    /// <param name="source">数据源</param>
    /// <returns>List值</returns>
    public static IList GetDataSourceList(BindingSource source)
    {
      if (source == null)
        return null;
      while (source.List is BindingSource)
        source = (BindingSource)source.List;
      return source.List;
    }

    /// <summary>
    /// 获取组件DataSource绑定的数据源
    /// </summary>
    /// <param name="component">组件</param>
    /// <returns>数据源</returns>
    public static BindingSource GetDataSource(object component)
    {
      if (component == null)
        return null;
      Type controlType = component.GetType();
      System.Reflection.PropertyInfo propertyInfo = controlType.GetProperty("DataSource");
      if (propertyInfo == null)
        return null;
      return propertyInfo.GetValue(component, null) as BindingSource;
    }

    /// <summary>
    /// 获取根数据源
    /// </summary>
    /// <param name="source">数据源</param>
    /// <returns>数据源</returns>
    public static BindingSource GetRootDataSource(BindingSource source)
    {
      if (source == null)
        return null;
      if (source.DataSource is BindingSource && !String.IsNullOrEmpty(source.DataMember))
        return GetRootDataSource((BindingSource)source.DataSource);
      return source;
    }
    
    /// <summary>
    /// 获取数据源的类型
    /// </summary>
    /// <param name="source">数据源</param>
    /// <returns>类型</returns>
    public static Type GetDataSourceType(BindingSource source)
    {
      if (source == null || source.DataSource == null)
        return null;
      if (!String.IsNullOrEmpty(source.DataMember))
      {
        Type coreType = null;
        if (source.DataSource is BindingSource)
          coreType = GetDataSourceCoreType((BindingSource)source.DataSource);
        else if (source.DataSource is Type)
          coreType = Utilities.GetCoreType((Type)source.DataSource);
        System.Reflection.PropertyInfo propertyInfo = Utilities.FindPropertyInfo(coreType, source.DataMember);
        return propertyInfo != null ? propertyInfo.PropertyType : null;
      }
      return source.DataSource as Type ?? source.DataSource.GetType();
    }

    /// <summary>
    /// 检索数据源的类型
    /// </summary>
    /// <param name="container">组件容器</param>
    /// <param name="baseType">基础类型, 为 null 时不做过滤</param>
    /// <returns>匹配的类型队列</returns>
    public static IList<Type> FindDataSourceTypes(IContainer container, Type baseType)
    {
      List<Type> result = new List<Type>();
      foreach (BindingSource item in ComponentHelper.FindComponents<BindingSource>(container))
      {
        Type type = GetDataSourceType(item);
        if (type == null || result.Contains(type))
          continue;
        if (baseType == null || type.IsSubclassOf(baseType))
          result.Add(type);
      }
      return result;
    }

    /// <summary>
    /// 获取数据源的核心类型
    /// </summary>
    /// <param name="source">数据源</param>
    /// <returns>类型</returns>
    public static Type GetDataSourceCoreType(BindingSource source)
    {
      return Utilities.GetCoreType(GetDataSourceType(source));
    }

    /// <summary>
    /// 检索数据源的核心类型
    /// </summary>
    /// <param name="container">组件容器</param>
    /// <param name="baseType">基础类型, 为 null 时不做过滤</param>
    /// <returns>匹配的类型队列</returns>
    public static IList<Type> FindDataSourceCoreTypes(IContainer container, Type baseType)
    {
      List<Type> result = new List<Type>();
      foreach (BindingSource item in ComponentHelper.FindComponents<BindingSource>(container))
      {
        Type type = GetDataSourceCoreType(item);
        if (type == null || result.Contains(type))
          continue;
        if (baseType == null || type.IsSubclassOf(baseType))
          result.Add(type);
      }
      return result;
    }

    /// <summary>
    /// 检索数据源的条件数据源
    /// </summary>
    /// <param name="container">组件容器</param>
    /// <param name="source">数据源</param>
    /// <returns>匹配的条件数据源</returns>
    public static BindingSource FindCriteriaBindingSource(IContainer container, BindingSource source)
    {
      Type coreType = GetDataSourceCoreType(source);
      if (coreType != null)
      {
        ClassMapInfo classMapInfo = ClassMemberHelper.GetClassMapInfo(coreType);
        if (classMapInfo != null && classMapInfo.DefaultCriteriaType != null)
          foreach (BindingSource item in ComponentHelper.FindComponents<BindingSource>(container))
            if (classMapInfo.IsCriteriaType(GetDataSourceType(item)))
              return item;
      }
      return null;
    }

    /// <summary>
    /// 数据源DataSource是集合类型
    /// </summary>
    /// <param name="source">数据源</param>
    public static bool? DataSourceIsEnumerable(BindingSource source)
    {
      Type dataSourceType = GetDataSourceType(source);
      if (dataSourceType == null)
        return null;
      return Utilities.FindListItemType(dataSourceType) != null;
    }

    /// <summary>
    /// 为数据源DataSource赋值
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="source">数据源</param>
    /// <param name="data">数据</param>
    /// <param name="locateItem">定位项</param>
    /// <param name="locatePositionMaximum">定位游标极限</param>
    /// <param name="hintComponent">用于填写提示信息</param>
    /// <param name="hint">提示信息</param>
    public static void SetDataSource(Control container, BindingSource source, object data, object locateItem, int locatePositionMaximum,
      object hintComponent, string hint)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (source.DataSource is BindingSource && !String.IsNullOrEmpty(source.DataMember))
        return;
      if (container != null && container.InvokeRequired)
        container.BeginInvoke(new ExecuteSetDataSourceDelegate(ExecuteSetDataSource),
          new object[] { source, data, locateItem, locatePositionMaximum, hintComponent, hint });
      else
        ExecuteSetDataSource(source, data, locateItem, locatePositionMaximum, hintComponent, hint);
    }
    private delegate void ExecuteSetDataSourceDelegate(BindingSource source, object data, object locateItem, int locatePositionMaximum, object hintComponent, string hint);
    private static void ExecuteSetDataSource(BindingSource source, object data, object locateItem, int locatePositionMaximum, object hintComponent, string hint)
    {
      DateTime startDateTime = DateTime.Now;
      source.DataSource = data;
      DateTime endDateTime = DateTime.Now;
      string text = String.Format(Properties.Resources.SetDataSourceHint, hint, endDateTime.Subtract(startDateTime).TotalSeconds);
      ApplyText(hintComponent, text);
      if (EventLog.MustSaveLog)
        EventLog.SaveLocal(text);
      LocatePosition(source, locateItem, locatePositionMaximum);
      Application.DoEvents();
    }

    private static void ApplyText(object component, string text)
    {
      if (component == null)
        return;
      Type componentType = component.GetType();
      System.Reflection.PropertyInfo propertyInfo = componentType.GetProperty("Text");
      if (propertyInfo != null)
      {
        propertyInfo.SetValue(component, text, null);
        return;
      }
      propertyInfo = componentType.GetProperty("Caption");
      if (propertyInfo != null)
      {
        propertyInfo.SetValue(component, text, null);
        return;
      }
    }

    /// <summary>
    /// 定位数据源游标
    /// </summary>
    public static void LocatePosition(BindingSource source, object item, int maximum)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (source.DataSource == null || source.Count == 0)
        source.Position = -1;
      else
      {
        if (item != null && source.Count <= maximum)
        {
          int i = source.IndexOf(item);
          if (i >= 0)
          {
            source.Position = i;
            return;
          }
        }
        source.Position = 0;
      }
    }
  }
}