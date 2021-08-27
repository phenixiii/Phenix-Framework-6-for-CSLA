namespace Phenix.Test.使用指南._21._7
{
  class LocalAssembly
  {
    /// <summary>
    /// 新增状态
    /// </summary>
    public bool IsNew { get; set; }

    /// <summary>
    /// 删除状态
    /// </summary>
    public bool IsSelfDeleted { get; set; }

    /// <summary>
    /// 更新状态
    /// </summary>
    public bool IsSelfDirty { get; set; }

    public long? AS_ID { get; set; }

    public string Name { get; set; }

    public string Caption { get; set; }
  }
}
