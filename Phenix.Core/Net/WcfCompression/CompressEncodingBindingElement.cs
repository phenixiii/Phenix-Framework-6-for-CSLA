using System;
using System.ServiceModel.Channels;
using System.Xml;

namespace Phenix.Core.Net.WcfCompression
{
  /// <summary>
  /// 用于指定对消息进行编码时所用消息版本的绑定元素
  /// </summary>
  public sealed class CompressEncodingBindingElement : MessageEncodingBindingElement
  {
    /// <summary>
    /// 用于指定对消息进行编码时所用消息版本的绑定元素
    /// </summary>
    public CompressEncodingBindingElement(MessageEncodingBindingElement innerMessageEncodingBindingElement)
    {
      _innerMessageEncodingBindingElement = innerMessageEncodingBindingElement;
      _readerQuotas = new XmlDictionaryReaderQuotas();
    }

    #region 属性

    private readonly MessageEncodingBindingElement _innerMessageEncodingBindingElement;
    /// <summary>
    /// 用于指定对消息进行编码时所用消息版本的绑定元素
    /// </summary>
    public MessageEncodingBindingElement InnerMessageEncodingBindingElement
    {
      get { return _innerMessageEncodingBindingElement; }
    }
    /// <summary>
    /// 获取或设置可由消息编码器工厂所生成消息编码器处理的消息版本
    /// </summary>
    public override MessageVersion MessageVersion
    {
      get { return _innerMessageEncodingBindingElement.MessageVersion; }
      set { _innerMessageEncodingBindingElement.MessageVersion = value; }
    }

    private readonly XmlDictionaryReaderQuotas _readerQuotas;

    #endregion

    #region 方法

    /// <summary>
    /// 在当前通道工厂之下为指定类型的通道生成内部通道工厂
    /// </summary>
    public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
    {
      if (context == null)
        throw new ArgumentNullException("context");
      context.BindingParameters.Add(this);
      return context.BuildInnerChannelFactory<TChannel>();
    }

    /// <summary>
    /// 生成用于侦听指定类型的通道的内部通道侦听器
    /// </summary>
    public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
    {
      if (context == null)
        throw new ArgumentNullException("context");
      context.BindingParameters.Add(this);
      return context.BuildInnerChannelListener<TChannel>();
    }

    /// <summary>
    /// 内部通道工厂是否可以生成指定类型的通道
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
    {
      if (context == null)
        throw new ArgumentNullException("context");
      context.BindingParameters.Add(this);
      return context.CanBuildInnerChannelFactory<TChannel>();
    }

    /// <summary>
    /// 是否可以生成内部通道侦听器来侦听指定类型的通道
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public override bool CanBuildChannelListener<TChannel>(BindingContext context)
    {
      if (context == null)
        throw new ArgumentNullException("context");
      context.BindingParameters.Add(this);
      return context.CanBuildInnerChannelListener<TChannel>();
    }

    /// <summary>
    /// 构建用于生成消息编码器的工厂
    /// </summary>
    public override MessageEncoderFactory CreateMessageEncoderFactory()
    {
      return new CompressEncoderFactory(_innerMessageEncodingBindingElement);
    }

    /// <summary>
    /// 从通道堆栈的适当层返回所请求的类型化对象（如果存在）
    /// </summary>
    public override T GetProperty<T>(BindingContext context)
    {
      if (typeof(T) == typeof(XmlDictionaryReaderQuotas))
        return _readerQuotas as T;
      return base.GetProperty<T>(context);
    }

    /// <summary>
    /// 克隆
    /// </summary>
    public override BindingElement Clone()
    {
      return new CompressEncodingBindingElement(_innerMessageEncodingBindingElement);
    }

    #endregion
  }
}