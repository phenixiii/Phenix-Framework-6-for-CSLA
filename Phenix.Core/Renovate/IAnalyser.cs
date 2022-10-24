using System.Collections;
using System.Data;
using Phenix.Core.Mapping;

namespace Phenix.Core.Renovate
{
  /// <summary>
  /// 分析者
  /// </summary>
  public interface IAnalyser : IList
  {
    #region 方法

    /// <summary>
    /// 分析数据集键资料
    /// </summary>
    /// <param name="dataKeyCount">数据集键数</param>
    void AnalyseDataKeyInfo(int dataKeyCount);

    /// <summary>
    /// 分析数据集资料
    /// </summary>
    /// <param name="data">数据集</param>
    bool AnalyseDataInfo(DataTable data);

    /// <summary>
    /// 分析动态刷新资料
    /// </summary>
    /// <param name="action">执行动作</param>
    /// <param name="values">值数组</param>
    IEntity AnalyseRenovateInfo(ExecuteAction action, object[] values);

    /// <summary>
    /// 加载数据
    /// </summary>
    void LoadData();

    #endregion
  }
}
