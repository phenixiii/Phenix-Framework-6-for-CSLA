namespace Phenix.Test.使用指南._21._5
{
  [System.SerializableAttribute()]
  [Phenix.Core.Mapping.Class("Phenix.Test.使用指南._21._5.Business.Service", FriendlyName = "")]
  public class Service : Phenix.Core.Data.ServiceBase<Service>
  {
    private Assembly _assembly;
    public Assembly Assembly
    {
      get { return _assembly; }
      set { _assembly = value; }
    }

    private string _result;
    public string Result
    {
      get { return _result; }
      set { _result = value; }
    }
  }
}