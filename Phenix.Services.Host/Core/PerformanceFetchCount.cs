using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Phenix.Core;
using Phenix.Core.Log;
using Phenix.Core.SyncCollections;

namespace Phenix.Services.Host.Core
{
  /// <summary>
  /// Fetch性能记录数
  /// </summary>
  internal class PerformanceFetchCount
  {
    public PerformanceFetchCount(string businessName)
    {
      _businessName = businessName;
      _maxCount = new PerformanceFetchMaxCount(businessName, 0, null);
    }

    #region 属性

    private readonly string _businessName;
    /// <summary>
    /// 业务类名
    /// </summary>
    public string BusinessName
    {
      get { return _businessName; }
    }

    //计数
    private long _tally;
    //按照10000条记录数递增计数
    private readonly SynchronizedDictionary<long, long> _tallyInterval = new SynchronizedDictionary<long, long>();
    //最大记录数
    private PerformanceFetchMaxCount _maxCount;

    #endregion

    #region 方法

    /// <summary>
    /// 检查最大记录数
    /// </summary>
    public bool CheckMaxCount(PerformanceFetchMaxCount newMaxCount)
    {
      try
      {
        _tally = _tally + 1;
        long scale = (newMaxCount.Value % 10000 > 0 ? newMaxCount.Value / 10000 + 1 : newMaxCount.Value / 10000) * 10000;
        _tallyInterval[scale] = (_tallyInterval.ContainsKey(scale) ? _tallyInterval[scale] : 0) + 1;

        if (newMaxCount.Value > _maxCount.Value)
        {
          _maxCount = newMaxCount;
          return true;
        }
        return false;
      }
      finally
      {
        if (_tally < 100 && _tally % 10 == 5 || _tally % 100 == 0)
          try
          {
            string directory = Path.Combine(AppConfig.BaseDirectory, "PerformanceFetch");
            if (!Directory.Exists(directory))
              Directory.CreateDirectory(directory);
            using (StreamWriter logFile = File.CreateText(Path.Combine(directory, String.Format("{0}.Count.Log", BusinessName))))
            {
              logFile.Write(ToString());
              logFile.Flush();
            }
          }
          catch (IOException)
          {
            EventLog.SaveLocal(ToString());
          }
      }
    }

    public override string ToString()
    {
      StringBuilder result = new StringBuilder();
      result.AppendLine(_businessName);
      result.AppendLine();
      result.Append("tally: ");
      result.AppendLine(_tally.ToString());
      result.AppendLine();
      foreach (KeyValuePair<long, long> kvp in _tallyInterval)
        result.AppendLine(String.Format("<= {0} rec: {1} in {2}%", kvp.Key, kvp.Value, kvp.Value*100/_tally));
      result.AppendLine();
      result.AppendLine(_maxCount.ToString());
      return result.ToString();
    }

    #endregion
  }
}
