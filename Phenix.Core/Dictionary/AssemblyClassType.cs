using System;
using Phenix.Core.Operate;
using Phenix.Core.Rule;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// 程序集类类型
  /// </summary>
  [Serializable]
  [KeyCaption(FriendlyName = "程序集类类型")]
  public enum AssemblyClassType
  {
    /// <summary>
    /// 普通
    /// </summary>
    [EnumCaption("普通")]
    Ordinary,

    /// <summary>
    /// 窗体 
    /// </summary>
    [EnumCaption("窗体")]
    Form,

    /// <summary>
    /// 业务 
    /// </summary>
    [EnumCaption("业务")]
    Business,

    /// <summary>
    /// 业务集
    /// </summary>
    [EnumCaption("业务集")]
    Businesses,

    /// <summary>
    /// 指令
    /// </summary>
    [EnumCaption("指令")]
    Command,

    /// <summary>
    /// 枚举
    /// </summary>
    [EnumCaption("枚举")]
    Enum,
    
    /// <summary>
    /// 实体 
    /// </summary>
    [EnumCaption("实体")]
    Entity,

    /// <summary>
    /// 实体集
    /// </summary>
    [EnumCaption("实体集")]
    EntityList,
    
    /// <summary>
    /// 服务
    /// </summary>
    [EnumCaption("服务")]
    Service,

    /// <summary>
    /// WebAPI
    /// </summary>
    [EnumCaption("WebAPI")]
    ApiController,
  }
}
