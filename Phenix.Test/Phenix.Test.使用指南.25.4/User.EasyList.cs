using System;
using Phenix.Core.Data;

namespace Phenix.Test.使用指南._25._4
{
  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  public class UserEasyList : EntityListBase<UserEasyList, UserEasy>
  {
    private UserEasyList()
    {
      //禁止添加代码
    }

    /// <summary>
    /// 构建实体
    /// </summary>
    protected override object CreateInstance()
    {
      return new UserEasyList();
    }

    /// <summary>
    /// 标签
    /// 缺省为 TEntity 上的 ClassAttribute.FriendlyName
    /// 用于提示信息等
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string Caption
    {
      get { return base.Caption; }
    }
  }
}