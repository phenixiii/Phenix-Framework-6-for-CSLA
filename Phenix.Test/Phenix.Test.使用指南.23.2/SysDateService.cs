using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using Phenix.Core.Data;
using Phenix.Core.Log;

namespace Phenix.Test.使用指南._23._2.Business
{
  /// <summary>
  /// 数据库时间
  /// </summary>
  [Serializable]
  public class SysDateService : ServiceBase<SysDateService>
  {
    protected override void DoExecute(DbConnection connection)
    {
      if (InAsync)
        while (!ExecuteResult.Stop)
        {
          RefreshMessage(connection);
          Thread.Sleep(1000);
        }
      else
        RefreshMessage(connection);
    }

    private void RefreshMessage(DbConnection connection)
    {
      using (SafeDataReader reader = new SafeDataReader(connection,
@"select sysdate
  from PH_SystemInfo",
  CommandBehavior.SingleRow, false))
      {
        if (reader.Read())
        {
          ExecuteResult.Message = reader.GetDateTime(0);
          EventLog.Save(this.GetType(), ExecuteResult.Message.ToString());
        }
      }
    }
  }
}
