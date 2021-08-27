﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phenix.Test.使用指南._21._5.Business
{
  /// <summary>
  /// </summary>
  [System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("")]
  public class AssemblyCriteria : Phenix.Business.CriteriaBase
  {
    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "AS_NAME", Logical = Phenix.Core.Mapping.CriteriaLogical.Or, Operate = Phenix.Core.Mapping.CriteriaOperate.Like, TableName = "PH_ASSEMBLY", ColumnName = "AS_NAME")]
    private string _name;
    /// <summary>
    /// AS_NAME
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "AS_NAME")]
    [System.ComponentModel.DisplayName("AS_NAME")]
    public string Name
    {
      get { return _name; }
      set { _name = value; PropertyHasChanged(); }
    }

    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "AS_CAPTION", Logical = Phenix.Core.Mapping.CriteriaLogical.Or, Operate = Phenix.Core.Mapping.CriteriaOperate.Like, TableName = "PH_ASSEMBLY", ColumnName = "AS_CAPTION")]
    private string _caption;
    /// <summary>
    /// AS_CAPTION
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "AS_CAPTION")]
    [System.ComponentModel.DisplayName("AS_CAPTION")]
    public string Caption
    {
      get { return _caption; }
      set { _caption = value; PropertyHasChanged(); }
    }
  }
}
