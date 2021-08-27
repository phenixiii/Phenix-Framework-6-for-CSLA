using System;

namespace Phenix.Security.Business
{
  /// <summary>
  /// 程序集
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class AssemblyReadOnly : Assembly<AssemblyReadOnly>
  {
  }

  /// <summary>
  /// 程序集清单
  /// </summary>
  [Serializable]
  public class AssemblyReadOnlyList : Phenix.Business.BusinessListBase<AssemblyReadOnlyList, AssemblyReadOnly>
  {
  }

  /// <summary>
  /// 程序集
  /// </summary>
  [Serializable]
  public class Assembly : Assembly<Assembly>
  {
    #region 属性
    
    /// <summary>
    /// 类信息
    /// </summary>
    public AssemblyClassList AssemblyClasses
    {
      get
      {
        AssemblyClassList result = FindCompositionDetail<AssemblyClassList, AssemblyClass>();
        if (result == null)
          AssemblyClassList.Fetch().CompositionFilter(Owner).TryGetValue(this, out result);
        return result;
      }
    }

    #endregion
  }

  /// <summary>
  /// 程序集清单
  /// </summary>
  [Serializable]
  public class AssemblyList : Phenix.Business.BusinessListBase<AssemblyList, Assembly>
  {
    /// <summary>
    /// 是否允许添加业务对象
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override bool AllowAddItem
    {
      get { return false; }
    }
  }

  /// <summary>
  /// 程序集
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_ASSEMBLY", FriendlyName = "程序集", DefaultCriteriaType = typeof(AssemblyCriteria)), System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("程序集")]
  public abstract class Assembly<T> : Phenix.Business.BusinessBase<T> where T : Assembly<T>
  {
    /// <summary>
    /// AS_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AS_IDProperty = RegisterProperty<long?>(c => c.AS_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "AS_ID", TableName = "PH_ASSEMBLY", ColumnName = "AS_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _AS_ID;
    /// <summary>
    /// AS_ID
    /// </summary>
    [System.ComponentModel.DisplayName("AS_ID")]
    public long? AS_ID
    {
      get { return GetProperty(AS_IDProperty, _AS_ID); }
      //internal set
      //{
      //  SetProperty(AS_IDProperty, ref _AS_ID, value);
      //}
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return AS_ID.ToString(); }
    }

    /// <summary>
    /// 名称
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "名称", Alias = "AS_NAME", TableName = "PH_ASSEMBLY", ColumnName = "AS_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
    private string _name;
    /// <summary>
    /// 名称
    /// </summary>
    [System.ComponentModel.DisplayName("名称")]
    public string Name
    {
      get { return GetProperty(NameProperty, _name); }
      //set { SetProperty(NameProperty, ref _name, value); }
    }

    /// <summary>
    /// 标签
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> CaptionProperty = RegisterProperty<string>(c => c.Caption);
    [Phenix.Core.Mapping.Field(FriendlyName = "标签", Alias = "AS_CAPTION", TableName = "PH_ASSEMBLY", ColumnName = "AS_CAPTION", NeedUpdate = true)]
    private string _caption;
    /// <summary>
    /// 标签
    /// </summary>
    [System.ComponentModel.DisplayName("标签")]
    public new string Caption
    {
      get { return GetProperty(CaptionProperty, _caption); }
      set { SetProperty(CaptionProperty, ref _caption, value); }
    }
  }
}
