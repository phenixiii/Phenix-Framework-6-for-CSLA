using System;
using System.Collections.Generic;
using Phenix.Core.Dictionary;

namespace Phenix.Core.Security
{
  /// <summary>
  /// 用户身份接口
  /// </summary>
  public interface IIdentity : System.Security.Principal.IIdentity
  {
    #region 属性

    /// <summary>
    /// 用户ID
    /// </summary>
    long? UserId { get; }

    /// <summary>
    /// 用户名称
    /// </summary>
    string UserName { get; }

    /// <summary>
    /// 登录工号
    /// </summary>
    string UserNumber { get; }

    /// <summary>
    /// 是否匿名用户
    /// </summary>
    bool IsGuest { get; }

    /// <summary>
    /// 是否管理员
    /// </summary>
    bool IsAdmin { get; }

    /// <summary>
    /// 拥有管理员角色?
    /// </summary>
    bool HaveAdminRole { get; }

    /// <summary>
    /// 所属企业
    /// </summary>
    string Enterprise  { get; }

    /// <summary>
    /// 所属部门ID
    /// </summary>
    long? DepartmentId { get; }

    /// <summary>
    /// 所属部门
    /// </summary>
    DepartmentInfo Department { get; }

    /// <summary>
    /// 担任岗位ID
    /// </summary>
    long? PositionId { get; }

    /// <summary>
    /// 担任岗位
    /// </summary>
    PositionInfo Position { get; }

    /// <summary>
    /// 上级(一级)
    /// </summary>
    IDictionary<string, IIdentity> Superiors { get; }

    /// <summary>
    /// 上级(全部)
    /// </summary>
    IDictionary<string, IIdentity> AllSuperiors { get; }

    /// <summary>
    /// 工友
    /// </summary>
    IDictionary<string, IIdentity> Workmates { get; }

    /// <summary>
    /// 下级(一级)
    /// </summary>
    IDictionary<string, IIdentity> Subordinates { get; }

    /// <summary>
    /// 下级(全部)
    /// </summary>
    IDictionary<string, IIdentity> AllSubordinates { get; }

    /// <summary>
    /// 角色资料队列
    /// </summary>
    IDictionary<string, RoleInfo> Roles { get; }

    /// <summary>
    /// 可授权角色资料队列
    /// </summary>
    IDictionary<string, RoleInfo> GrantRoles { get; }

    /// <summary>
    /// 切片资料队列
    /// </summary>
    IDictionary<string, SectionInfo> Sections { get; }

    #endregion

    #region 方法

    #region 上下级关系

    /// <summary>
    /// 是上级
    /// </summary>
    bool HaveSuperior(string userNumber);

    /// <summary>
    /// 是下属
    /// </summary>
    bool HaveSubordinate(string userNumber);

    #endregion

    #region 切片

    /// <summary>
    /// 取本用户可操作的类所关联切片
    /// null代表不存在切片
    /// </summary>
    /// <param name="objectType">类</param>
    IList<string> GetSectionNames(Type objectType);

    /// <summary>
    /// 取本用户可操作的类及其条件类所关联切片
    /// null代表不存在切片
    /// </summary>
    /// <param name="objectType">类</param>
    /// <param name="criteriaType">条件类</param>
    IList<string> GetSectionNames(Type objectType, Type criteriaType);

    #endregion

    #region 权限验证

    /// <summary>
    /// 是否允许设置数据
    /// 只读则为false
    /// </summary>
    /// <param name="info">数据</param>
    bool AllowSet(ISecurityInfo info);

    /// <summary>
    /// 是否允许设置数据
    /// 只读则为false
    /// </summary>
    /// <param name="data">数据</param>
    bool AllowSet(object data);

    /// <summary>
    /// 确定是否属于指定的角色
    /// </summary>
    /// <param name="role">角色</param>
    bool IsInRole(string role);

    /// <summary>
    /// 确定是否被拒绝
    /// </summary>
    /// <param name="allowRoles">可用角色队列</param>
    /// <param name="denyRoles">禁用角色队列</param>
    bool IsByDeny(IList<string> allowRoles, IList<string> denyRoles);
    
    #endregion

    #endregion
  }
}