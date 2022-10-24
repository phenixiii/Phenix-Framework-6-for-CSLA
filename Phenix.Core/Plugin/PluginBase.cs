#if Top
using System.Threading.Tasks;
#endif

using System;
using System.Collections.Generic;

namespace Phenix.Core.Plugin
{
  /// <summary>
  /// 插件基类
  /// </summary>
  public abstract class PluginBase<T> : PluginBase
    where T : PluginBase<T>
  {
    /// <summary>
    /// 插件基类
    /// </summary>
    protected PluginBase()
      : base()
    {
      Default = this as T;
    }

    #region 属性

    /// <summary>
    /// 缺省插件
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Default { get; private set; }

    #endregion
  }

  /// <summary>
  /// 插件抽象类
  /// </summary>
  public abstract class PluginBase : BaseDisposable, IPlugin
  {
    #region 工厂

    internal static PluginBase New(Type pluginType, PluginHost owner, string key)
    {
      PluginBase result = (PluginBase)Activator.CreateInstance(pluginType, true);
      result.Owner = owner;
      result.Key = key;
      result.State = PluginState.Created;
      return result;
    }
    
    #endregion

    #region 属性

    /// <summary>
    /// 插件容器
    /// </summary>
    protected PluginHost Owner { get; private set; }

    /// <summary>
    /// 唯一键
    /// </summary>
    public string Key { get; private set; }

    /// <summary>
    /// 插件状态
    /// </summary>
    public PluginState State { get; private set; }

    /// <summary>
    /// 设置中
    /// </summary>
    public bool Setuping { get; private set; }

    #endregion

    #region 方法

    #region 实现 BaseDisposable 抽象函数

    /// <summary>
    /// 释放托管资源
    /// </summary>
    protected override void DisposeManagedResources()
    {
      if (Owner != null)
      {
        Owner.DoFinalized(new PluginEventArgs(this));
        Owner = null;
      }
    }

    /// <summary>
    /// 释放非托管资源
    /// </summary>
    protected override void DisposeUnmanagedResources()
    {
    }

    #endregion

    #region IPlugin 成员

    /// <summary>
    /// 初始化
    /// 由 PluginHost 调用
    /// </summary>
    protected virtual IList<MessageNotifyEventArgs> Initialization()
    {
      return null;
    }
    IList<MessageNotifyEventArgs> IPlugin.Initialization()
    {
      IList<MessageNotifyEventArgs> result = Initialization();
      State = PluginState.Initialized;
      return result;
    }

    /// <summary>
    /// 终止化
    /// 由 PluginHost 调用
    /// </summary>
    protected virtual void Finalization()
    {
    }
    void IPlugin.Finalization()
    {
      State = PluginState.Finalizing;
      Finalization();
      Dispose();
    }

    /// <summary>
    /// 设置
    /// 由 PluginHost 调用
    /// </summary>
    /// <param name="sender">发起对象</param>
    /// <returns>按需返回</returns>
    public virtual object Setup(object sender)
    {
      return this;
    }
    object IPlugin.Setup(object sender)
    {
      try
      {
        Setuping = true;
        return Setup(sender);
      }
      finally
      {
        Setuping = false;
      }
    }

    /// <summary>
    /// 启动
    /// 由 PluginHost 调用
    /// </summary>
    /// <returns>确定启动</returns>
    protected virtual bool Start()
    {
      return true;
    }
    bool IPlugin.Start()
    {
      bool result = Start();
      if (result)
        State = PluginState.Started;
      return result;
    }

    /// <summary>
    /// 暂停
    /// 由 PluginHost 调用
    /// </summary>
    /// <returns>确定停止</returns>
    protected virtual bool Suspend()
    {
      return true;
    }
    bool IPlugin.Suspend()
    {
      bool result = Suspend();
      if (result)
        State = PluginState.Suspended;
      return result;
    }

    /// <summary>
    /// 分析消息
    /// 由 PluginHost 调用
    /// </summary>
    /// <param name="message">消息</param>
    /// <returns>按需返回</returns>
    public virtual object AnalyseMessage(object message)
    {
      return this;
    }

#if Top
    
    /// <summary>
    /// 分析消息(异步)
    /// 由 PluginHost 调用
    /// </summary>
    /// <param name="message">消息</param>
    /// <returns>按需返回</returns>
    public virtual Task<object> AnalyseMessageAsync(object message)
    {
      return Task.Run(() => (object)this);
    }

#endif

    #endregion

    /// <summary>
    /// 发送给容器消息
    /// </summary>
    /// <param name="message">消息</param>
    public void SendMessage(object message)
    {
      Owner.DoMessage(new PluginEventArgs(this, message));
    }

    #endregion
  }
}