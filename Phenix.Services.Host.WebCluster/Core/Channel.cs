using System;
using System.Net.Http;
using System.Threading;

namespace Phenix.Services.Host.WebCluster.Core
{
  internal class Channel : IDisposable
  {
    [Newtonsoft.Json.JsonConstructor]
    public Channel(string hostUrl, int webApiPort, int webSocketPort)
    {
      _hostUrl = hostUrl;
      _webApiPort = webApiPort;
      _webSocketPort = webSocketPort;

      _httpClient = new HttpClient();
      _httpClient.BaseAddress = new Uri(String.Format("{0}:{1}", hostUrl, webApiPort));
    }

    #region 属性

    private readonly string _hostUrl;
    public string HostUrl
    {
      get { return _hostUrl; }
    }

    private readonly int _webApiPort;
    public int WebApiPort
    {
      get { return _webApiPort; }
    }

    private readonly int _webSocketPort;
    public int WebSocketPort
    {
      get { return _webSocketPort; }
    }

    private HttpClient _httpClient;
    [Newtonsoft.Json.JsonIgnore]
    public HttpClient HttpClient
    {
      get { return _httpClient; }
    }

    private DateTime _lastActionTime = DateTime.Now;
    [Newtonsoft.Json.JsonIgnore]
    public DateTime LastActionTime
    {
      get { return _lastActionTime; }
    }

    private long _responseTimes;
    [Newtonsoft.Json.JsonIgnore]
    public long ResponseTimes
    {
      get { return _responseTimes; }
    }

    private long _errorTimes;
    [Newtonsoft.Json.JsonIgnore]
    public long ErrorTimes
    {
      get { return _errorTimes; }
    }

    private Exception _lastError;
    [Newtonsoft.Json.JsonIgnore]
    public Exception LastError
    {
      get { return _lastError; }
    }

    private DateTime? _lastErrorTime;
    [Newtonsoft.Json.JsonIgnore]
    public DateTime? LastErrorTime
    {
      get { return _lastErrorTime; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 释放
    /// </summary>
    public void Dispose()
    {
      if (_httpClient != null)
      {
        _httpClient.Dispose();
        _httpClient = null;
      }
    }

    private void ResetTimes()
    {
      if (DateTime.Now.Subtract(_lastActionTime).TotalMinutes >= 1)
      {
        Interlocked.Exchange(ref _responseTimes, 0);
        Interlocked.Exchange(ref _errorTimes, 0);
      }
      _lastActionTime = DateTime.Now;
    }

    public void ResetStatus()
    {
      ResetTimes();
      Interlocked.Increment(ref _responseTimes);
    }

    public void ResetStatus(Exception error)
    {
      ResetTimes();
      Interlocked.Increment(ref _errorTimes);
      _lastError = error;
      _lastErrorTime = DateTime.Now;
    }

    public override string ToString()
    {
      return String.Format("{0}:{1} | {0}:{2}", HostUrl, WebApiPort, WebSocketPort);
    }

    #endregion
  }
}
