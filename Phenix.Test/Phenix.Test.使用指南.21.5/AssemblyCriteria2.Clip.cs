using System;
using Phenix.Core.Data;

namespace Phenix.Test.使用指南._21._5
{
  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.Class("Phenix.Test.使用指南._21._5.Business.AssemblyCriteria2", FriendlyName = "")]
  public class AssemblyCriteria2 : CriteriaBase
  {
    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "AS_NAME")]
    private string _name;
    /// <summary>
    /// AS_NAME
    /// </summary>
    public string Name
    {
      get { return _name; }
      set { _name = value; PropertyHasChanged(); }
    }
  }
}