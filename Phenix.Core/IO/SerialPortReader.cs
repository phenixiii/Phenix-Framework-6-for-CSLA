using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Phenix.Core.IO
{
  /// <summary>
  /// SerialPort速读者(MessageNotify事件不再推送报文, 需自建线程循环Read报文, 可设置MaxReceivedCache、ReceivedCacheCheckInterval以控制收到报文缓存大小)
  /// 调用者可自行Dispose曾Listen进来的SerialPort对象(建议事先Unlisten以免产生死锁)
  /// Dispose本对象时会自动Dispose所有Listen进来的有效SerialPort对象
  /// </summary>
  public class SerialPortFastReader : SerialPortReader<SerialPortFastReader>
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public SerialPortFastReader()
    {
      _checkReceivedCacheThread = new Thread(ExecuteCheckReceivedCache);
      _checkReceivedCacheThread.IsBackground = true;
      _checkReceivedCacheThread.Start();
    }

    #region 属性

    private int _maxReceivedCache = Int16.MaxValue;
    /// <summary>
    /// 收到报文缓存极限大小
    /// 缺省为 Int16.MaxValue
    /// </summary>
    public int MaxReceivedCache
    {
      get { return _maxReceivedCache; }
      set { _maxReceivedCache = value; }
    }

    private int _receivedCacheCheckInterval = 3;
    /// <summary>
    /// 收到报文缓存检查间隔(秒)
    /// 缺省为 3
    /// </summary>
    public int ReceivedCacheCheckInterval
    {
      get { return _receivedCacheCheckInterval; }
      set { _receivedCacheCheckInterval = value > 0 ? value : 3; }
    }

    private Thread _checkReceivedCacheThread;

    //PortName->报文缓存
    private readonly Phenix.Core.SyncCollections.SynchronizedDictionary<string, Phenix.Core.SyncCollections.SynchronizedQueue<string>> _receivedCache =
      new Phenix.Core.SyncCollections.SynchronizedDictionary<string, Phenix.Core.SyncCollections.SynchronizedQueue<string>>();

    #endregion

    #region 事件

    /// <summary>
    /// 触发消息通报事件
    /// </summary>
    protected override void OnMessageNotify(MessageNotifyEventArgs e)
    {
      if (e.MessageNotifyType == MessageNotifyType.Information)
        _receivedCache.GetValue(e.Title, () => new Phenix.Core.SyncCollections.SynchronizedQueue<string>(), true).Enqueue(e.Message);
      else
        base.OnMessageNotify(e);
    }

    #endregion

    #region 方法

    #region 实现 BaseDisposable 抽象函数

    /// <summary>
    /// 释放托管资源
    /// </summary>
    protected override void DisposeManagedResources()
    {
      if (_checkReceivedCacheThread != null)
      {
        _checkReceivedCacheThread.Abort();
        _checkReceivedCacheThread = null;
      }
      base.DisposeManagedResources();
    }

    #endregion

    /// <summary>
    /// 读取
    /// </summary>
    public string[] Read(SerialPort serialPort)
    {
      if (serialPort == null)
        throw new ArgumentNullException("serialPort");
      return Read(serialPort.PortName);
    }

    /// <summary>
    /// 读取
    /// </summary>
    public string[] Read(string portName)
    {
      Phenix.Core.SyncCollections.SynchronizedQueue<string> queue;
      if (_receivedCache.TryGetValue(portName, out queue))
        return queue.ToArray(true);
      return null;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void ExecuteCheckReceivedCache()
    {
      try
      {
        while (true)
          try
          {
            foreach (KeyValuePair<string, Phenix.Core.SyncCollections.SynchronizedQueue<string>> kvp in _receivedCache)
              while (kvp.Value.Count > MaxReceivedCache)
                kvp.Value.Dequeue();
            Thread.Sleep(ReceivedCacheCheckInterval * 1000);
          }
          catch (ObjectDisposedException)
          {
            return;
          }
          catch (ThreadAbortException)
          {
            Thread.ResetAbort();
            return;
          }
          catch (Exception)
          {
            Thread.Sleep(1000);
          }
      }
      finally
      {
        _checkReceivedCacheThread = null;
      }
    }

    #endregion
  }

  /// <summary>
  /// SerialPort直读者(经MessageNotify事件读取报文, 事件函数内的代码务必高效, 以免报文堆积)
  /// 调用者可自行Dispose曾Listen进来的SerialPort对象(建议事先Unlisten以免产生死锁)
  /// Dispose本对象时会自动Dispose所有Listen进来的有效SerialPort对象
  /// </summary>
  public class SerialPortDirectReader : SerialPortReader<SerialPortDirectReader>
  {
  }

  /// <summary>
  /// SerialPort读者基类
  /// 调用者可自行Dispose曾Listen进来的SerialPort对象(建议事先Unlisten以免产生死锁)
  /// Dispose本对象时会自动Dispose所有Listen进来的有效SerialPort对象
  /// </summary>
  public abstract class SerialPortReader<T> : Phenix.Core.BaseDisposable<T>
   where T : SerialPortReader<T>
  {
    #region 属性

    //PortName->SerialPort
    private readonly Phenix.Core.SyncCollections.SynchronizedDictionary<string, SerialPort> _serialPorts =
      new Phenix.Core.SyncCollections.SynchronizedDictionary<string, SerialPort>(StringComparer.Ordinal);

    /// <summary>
    /// 按PortName返回SerialPort
    /// </summary>
    protected SerialPort this[string portName]
    {
      get { return _serialPorts[portName]; }
    }

    /// <summary>
    /// 监听中
    /// </summary>
    public bool Listening
    {
      get { return _serialPorts.Count > 0; }
    }

    private class Location { public long _value; }
    //PortName->Location
    private readonly Phenix.Core.SyncCollections.SynchronizedDictionary<string, Location> _readingLocations =
      new Phenix.Core.SyncCollections.SynchronizedDictionary<string, Location>(StringComparer.Ordinal);

    #endregion

    #region 事件

    private EventHandler<MessageNotifyEventArgs> _messageNotify;
    /// <summary>
    /// 消息通报事件
    /// 本事件是在辅助线程上进行的, 而不是主线程, 试图修改主线程中的某些元素, 例如UI元素, 可能会导致线程异常
    /// MessageNotifyType.Information: Title = PortName, Message = ReceivedData
    /// MessageNotifyType.Warning: Title = PortName, Message = String.Empty
    /// MessageNotifyType.Error: Title = PortName, Error = Exception / Message = EventType
    /// </summary>
    public event EventHandler<MessageNotifyEventArgs> MessageNotify
    {
      add { _messageNotify = (EventHandler<MessageNotifyEventArgs>)Delegate.Combine(_messageNotify, value); }
      remove { _messageNotify = (EventHandler<MessageNotifyEventArgs>)Delegate.Remove(_messageNotify, value); }
    }
    /// <summary>
    /// 触发消息通报事件
    /// </summary>
    protected virtual void OnMessageNotify(MessageNotifyEventArgs e)
    {
      EventHandler<MessageNotifyEventArgs> handler = _messageNotify;
      if (handler != null)
        handler(this, e);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void OnMessageNotify(SerialPort serialPort, MessageNotifyEventArgs e)
    {
      Location location;
      if (_readingLocations.TryGetValue(serialPort.PortName, out location))
      {
        Interlocked.Increment(ref location._value);
        try
        {
          if (!Disposing)
            OnMessageNotify(e);
        }
        catch (Exception ex)
        {
          Phenix.Core.Log.EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
        }
        finally
        {
          Interlocked.Decrement(ref location._value);
        }
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      SerialPort serialPort = (SerialPort)sender;
      if (_serialPorts.ContainsValue(serialPort))
        try
        {
          int count = serialPort.BytesToRead;
          if (count > 0)
          {
            byte[] readBuffer = new byte[count];
            if (serialPort.Read(readBuffer, 0, count) > 0)
            {
              OnMessageNotify(serialPort, new MessageNotifyEventArgs(MessageNotifyType.Information, serialPort.PortName, Encoding.ASCII.GetString(readBuffer)));
              return;
            }
          }
          OnMessageNotify(serialPort, new MessageNotifyEventArgs(MessageNotifyType.Warning, serialPort.PortName, String.Empty));
        }
        catch (Exception ex)
        {
          OnMessageNotify(serialPort, new MessageNotifyEventArgs(MessageNotifyType.Error, serialPort.PortName, ex));
        }
    }

    /// <summary>
    /// 接收错误事件
    /// </summary>
    private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
    {
      SerialPort serialPort = (SerialPort)sender;
      if (_serialPorts.ContainsValue(serialPort))
        OnMessageNotify(serialPort, new MessageNotifyEventArgs(MessageNotifyType.Error, serialPort.PortName, e.EventType.ToString()));
    }

    private void SerialPort_Disposed(object sender, EventArgs e)
    {
      SerialPort serialPort = (SerialPort)sender;
      if (_serialPorts.ContainsValue(serialPort))
        Unlisten(serialPort);
    }

    #endregion

    #region 方法

    #region 实现 BaseDisposable 抽象函数

    /// <summary>
    /// 释放托管资源
    /// </summary>
    protected override void DisposeManagedResources()
    {
      _readingLocations.Clear();
      base.DisposeManagedResources();
    }

    /// <summary>
    /// 释放非托管资源
    /// </summary>
    protected override void DisposeUnmanagedResources()
    {
      _serialPorts.Clear(serialPorts =>
      {
        foreach (KeyValuePair<string, SerialPort> kvp in serialPorts)
          kvp.Value.Dispose();
      });
      base.DisposeUnmanagedResources();
    }

    #endregion

    /// <summary>
    /// 注册监听
    /// </summary>
    /// <param name="serialPort">设置好参数的SerialPort对象</param>
    public virtual bool Listen(SerialPort serialPort)
    {
      if (serialPort == null)
        throw new ArgumentNullException("serialPort");

      if (!_serialPorts.ContainsKey(serialPort.PortName))
        lock (_serialPorts)
          if (!_serialPorts.ContainsKey(serialPort.PortName))
          {
            _serialPorts.Add(serialPort.PortName, serialPort);
            _readingLocations.Add(serialPort.PortName, new Location());

            serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(SerialPort_ErrorReceived);
            serialPort.Disposed += new EventHandler(SerialPort_Disposed);
            if (!serialPort.IsOpen)
              serialPort.Open();
            return true;
          }
      return false;
    }

    /// <summary>
    /// 撤销监听
    /// </summary>
    public void Unlisten(SerialPort serialPort)
    {
      Unlisten(serialPort.PortName);
    }

    /// <summary>
    /// 撤销监听
    /// </summary>
    public virtual void Unlisten(string portName)
    {
      if (!Disposing)
      {
        _serialPorts.Remove(portName);
        _readingLocations.Remove(portName);
      }
    }

    /// <summary>
    /// 读取中
    /// </summary>
    public bool Reading(SerialPort serialPort)
    {
      if (serialPort == null)
        throw new ArgumentNullException("serialPort");
      return Reading(serialPort.PortName);
    }

    /// <summary>
    /// 读取中
    /// </summary>
    public bool Reading(string portName)
    {
      Location location;
      if (_readingLocations.TryGetValue(portName, out location))
        return Interlocked.Read(ref location._value) != 0;
      return false;
    }

    #endregion
  }
}