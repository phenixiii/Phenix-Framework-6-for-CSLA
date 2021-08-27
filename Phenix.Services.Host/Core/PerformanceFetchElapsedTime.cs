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
  /// Fetch性能耗时
  /// </summary>
  internal class PerformanceFetchElapsedTime
  {
    public PerformanceFetchElapsedTime(string businessName)
    {
      _businessName = businessName;
      _maxElapsedTime = new PerformanceFetchMaxElapsedTime(businessName, 0, 0, null);
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
    //按照3秒耗时递增计数
    private readonly SynchronizedDictionary<double, long> _tallyInterval = new SynchronizedDictionary<double, long>();
    //最大耗时(秒)
    private PerformanceFetchMaxElapsedTime _maxElapsedTime;

    #endregion

    #region 方法

    /// <summary>
    /// 检查最大耗时
    /// </summary>
    public bool CheckMaxElapsedTime(PerformanceFetchMaxElapsedTime newMaxElapsedTime)
    {
      try
      {
        _tally = _tally + 1;
        double scale = Math.Ceiling(newMaxElapsedTime.Value / 3) * 3;
        _tallyInterval[scale] = (_tallyInterval.ContainsKey(scale) ? _tallyInterval[scale] : 0) + 1;

        if (newMaxElapsedTime.Value > _maxElapsedTime.Value)
        {
          _maxElapsedTime = newMaxElapsedTime;
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
            using (StreamWriter logFile = File.CreateText(Path.Combine(directory, String.Format("{0}.ElapsedTime.Log", BusinessName))))
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
      foreach (KeyValuePair<double, long> kvp in _tallyInterval)
        result.AppendLine(String.Format("<= {0} sec: {1} in {2}% ", kvp.Key, kvp.Value, kvp.Value*100/_tally));
      result.AppendLine();
      result.AppendLine(_maxElapsedTime.ToString());
      return result.ToString();
    }

    #endregion
  }
}
