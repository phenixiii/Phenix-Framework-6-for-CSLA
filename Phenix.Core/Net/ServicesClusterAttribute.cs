using System;
using Phenix.Core.Reflection;

namespace Phenix.Core.Net
{
  /// <summary>
  /// "服务集群"标签
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public sealed class ServicesClusterAttribute : Attribute
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="key">唯一键</param>
    public ServicesClusterAttribute(string key)
      : base()
    {
      _key = key;
    }
    
    #region 工厂

    /// <summary>
    /// 构建
    /// </summary>
    /// <param name="ownerType">所属类</param>
    public static ServicesClusterAttribute Fetch(Type ownerType)
    {
      ServicesClusterAttribute result = AppUtilities.GetFirstCustomAttribute<ServicesClusterAttribute>(ownerType);
      if (result != null)
        return result;
      Type listItemType = Utilities.FindListItemType(ownerType);
      if (listItemType != null)
        return Fetch(listItemType);
      return null;
    }

    #endregion

    #region 属性

    private readonly string _key;
    /// <summary>
    /// 唯一键
    /// 可配合Phenix.Core.Net.NetConfig的RegisterServicesCluster()函数按照指定键值注册的服务器地址来调用Services
    /// </summary>
    public string Key
    {
      get { return _key; }
    }

    #endregion
  }
}
