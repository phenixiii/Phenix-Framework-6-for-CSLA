using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phenix.Test.使用指南._21._5.Business
{
  /// <summary>
  /// </summary>
  [System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("")]
  public class AssemblyCriteria2 : Phenix.Business.CriteriaBase
  {
    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "AS_NAME", Logical = Phenix.Core.Mapping.CriteriaLogical.And, Operate = Phenix.Core.Mapping.CriteriaOperate.Equal, TableName = "PH_ASSEMBLY", ColumnName = "AS_NAME")]
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
  }
}
