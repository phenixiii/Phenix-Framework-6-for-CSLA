using System;

namespace Phenix.Core.Plugin
{
  /// <summary>
  /// "插件"标签
  /// 相同Key的类，在PluginHost上仅维持最后一个注册的类，并允许实例化
  /// 缺省情况下Key等于类的FullName
  /// 这样，可变相地实现插件的热插拔功能，我们可以定义新类，注册后可替换当前运行环境中的旧类、实例化后可替换当前运行环境中的旧类对象
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public sealed class PluginAttribute : Attribute
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="key">唯一键</param>
    public PluginAttribute(string key)
    {
      _key = key;
    }

    #region 属性


    private readonly string _key;

    /// <summary>
    /// 唯一键
    /// </summary>
    public string Key
    {
      get { return _key; }
    }

    #endregion
  }
}
