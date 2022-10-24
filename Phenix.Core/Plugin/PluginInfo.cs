using System;
using Phenix.Core.Reflection;

namespace Phenix.Core.Plugin
{
  /// <summary>
  /// 插件信息
  /// </summary>
  public sealed class PluginInfo
  {
    private PluginInfo(Type ownerType, PluginAttribute pluginAttribute)
    {
      _ownerType = ownerType;
      _pluginAttribute = pluginAttribute ?? new PluginAttribute(_ownerType.FullName);
    }

    #region 工厂

    internal static PluginInfo Fetch(Type ownerType)
    {
      ownerType = Utilities.LoadType(ownerType); //主要用于IDE环境

      return new PluginInfo(ownerType, AppUtilities.GetFirstCustomAttribute<PluginAttribute>(ownerType));
    }

    #endregion

    #region 属性

    private readonly Type _ownerType;

    /// <summary>
    /// 所属类
    /// </summary>
    public Type OwnerType
    {
      get { return _ownerType; }
    }

    private readonly PluginAttribute _pluginAttribute;

    private string _key;
    /// <summary>
    /// 唯一键
    /// </summary>
    public string Key
    {
      get
      {
        if (_key == null)
          _key = !String.IsNullOrEmpty(_pluginAttribute.Key)
            ? _pluginAttribute.Key
            : _ownerType.FullName;
        return _key;
      }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 字符串表示
    /// </summary>
    public override string ToString()
    {
      return String.Format("{0}[{1}]", Key, OwnerType.FullName);
    }

    #endregion
  }
}