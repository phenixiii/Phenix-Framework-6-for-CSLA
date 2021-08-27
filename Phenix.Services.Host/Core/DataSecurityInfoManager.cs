using System;
using System.Collections.Generic;
using System.Threading;
using Phenix.Core;
using Phenix.Core.SyncCollections;

namespace Phenix.Services.Host.Core
{
  internal class DataSecurityInfoManager : BaseDisposable
  {
    #region 单例

    private DataSecurityInfoManager(Action<DataSecurityEventArgs> doRefresh) 
    {
      _refresh = doRefresh;
      DoInitialize();
    }

    private static readonly object _defaultLock = new object();
    private static DataSecurityInfoManager _default;
    public static DataSecurityInfoManager Run(Action<DataSecurityEventArgs> doRefresh)
    {
      if (_default == null)
        lock (_defaultLock)
          if (_default == null)
          {
            _default = new DataSecurityInfoManager(doRefresh);
          }
      return _default;
    }
    public static void Stop()
    {
      DataSecurityInfoManager manager = _default;
      if (manager != null)
        manager.Dispose();
    }

    #endregion

    #region 属性

    private Timer _timer;

    private readonly SynchronizedDictionary<string, DataSecurityEventArgs> _dataSecurityEventArgsCache =
      new SynchronizedDictionary<string, DataSecurityEventArgs>(StringComparer.Ordinal);

    #endregion

    #region 事件

    private event Action<DataSecurityEventArgs> _refresh;
    private void OnRefresh(DataSecurityEventArgs e)
    {
      if (_refresh != null)
        _refresh(e);
    }

    #endregion

    #region 方法

    private void DoInitialize()
    {
      Phenix.Services.Host.Service.DataSecurity.Changed += new Action<DataSecurityEventArgs>(Changed);
      Phenix.Services.Host.Service.Wcf.DataSecurity.Changed += new Action<DataSecurityEventArgs>(Changed);
#if Top
      Phenix.Services.Host.Service.Web.SecurityController.Changed += new Action<DataSecurityEventArgs>(Changed);
#endif
      //Phenix.Services.Host.Service.Data.DataSecurityChanged += new Action<DataSecurityEventArgs>(Changed);
      //Phenix.Services.Host.Service.Wcf.Data.DataSecurityChanged += new Action<DataSecurityEventArgs>(Changed);
      //Phenix.Services.Host.Service.DataPortal.DataSecurityChanged += new Action<DataSecurityEventArgs>(Changed);
      //Phenix.Services.Host.Service.Wcf.DataPortal.DataSecurityChanged += new Action<DataSecurityEventArgs>(Changed);

      _timer = new Timer(new TimerCallback(Refresher), null, new TimeSpan(0, 3, 0), new TimeSpan(0, 3, 0));
    }

    #region 实现 BaseDisposable 抽象函数

    protected override void DisposeManagedResources()
    {
      if (_default == this)
        lock (_defaultLock)
          if (_default == this)
          {
            _default = null;
          }
      if (_timer != null)
      {
        _timer.Dispose();
        _timer = null;
      }

      Phenix.Services.Host.Service.DataSecurity.Changed -= new Action<DataSecurityEventArgs>(Changed);
      Phenix.Services.Host.Service.Wcf.DataSecurity.Changed -= new Action<DataSecurityEventArgs>(Changed);
#if Top
      Phenix.Services.Host.Service.Web.SecurityController.Changed -= new Action<DataSecurityEventArgs>(Changed);
#endif
      //Phenix.Services.Host.Service.Data.DataSecurityChanged -= new Action<DataSecurityEventArgs>(Changed);
      //Phenix.Services.Host.Service.Wcf.Data.DataSecurityChanged -= new Action<DataSecurityEventArgs>(Changed);
      //Phenix.Services.Host.Service.DataPortal.DataSecurityChanged -= new Action<DataSecurityEventArgs>(Changed);
      //Phenix.Services.Host.Service.Wcf.DataPortal.DataSecurityChanged -= new Action<DataSecurityEventArgs>(Changed);
    }

    protected override void DisposeUnmanagedResources()
    {
    }

    #endregion

    private void Changed(DataSecurityEventArgs e)
    {
      DataSecurityEventArgs args;
      if (String.IsNullOrEmpty(e.LocalAddress) || !e.LogOn)
        if (_dataSecurityEventArgsCache.TryGetValue(e.UserNumber, out args))
          e.LocalAddress = args.LocalAddress;
      _dataSecurityEventArgsCache[e.UserNumber] = e;
    }

    private void Refresher(Object stateInfo)
    {
      foreach (KeyValuePair<string, DataSecurityEventArgs> kvp in _dataSecurityEventArgsCache)
        OnRefresh(kvp.Value);
    }

    #endregion
  }
}
