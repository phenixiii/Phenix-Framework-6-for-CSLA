using Phenix.Core.Cache;
using Phenix.Core.Data;
using Phenix.Core.Dictionary;
using Phenix.Core.Log;
using Phenix.Core.Message;
using Phenix.Core.Rule;
using Phenix.Core.Security;
using Phenix.Core.Workflow;

namespace Phenix.Services.Library
{
  /// <summary>
  /// 注册器
  /// </summary>
  public static class Registration
  {
    #region 方法

    /// <summary>
    /// 注册实施者
    /// </summary>
    public static void RegisterWorker()
    {
      DataDictionaryHub.Worker = new DataDictionary();
      DataSecurityHub.Worker = new DataSecurity();
      DataHub.Worker = new Data();
      DataRuleHub.Worker = new DataRule();
      PermanentLogHub.Worker = new PermanentLog();
      ObjectCache.Worker = new ObjectCacheSynchro();
      WorkflowHub.Worker = new Workflow();
      MessageHub.Worker = new Message();
    }

    #endregion
  }
}