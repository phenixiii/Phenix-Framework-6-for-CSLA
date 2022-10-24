using System.ServiceModel.Channels;

namespace Phenix.Core.Net.WcfCompression
{
  /// <summary>
  /// 用于生成消息编码器的工厂
  /// </summary>
  public class CompressEncoderFactory : MessageEncoderFactory
  {
    /// <summary>
    /// 用于生成消息编码器的工厂
    /// </summary>
    public CompressEncoderFactory(MessageEncodingBindingElement innerMessageEncodingBindingElement)
    {
      _innerMessageEncodingBindingElement = innerMessageEncodingBindingElement;
      _encoder = new CompressEncoder(this);
    }

    #region 属性

    private readonly MessageEncodingBindingElement _innerMessageEncodingBindingElement;
    /// <summary>
    /// 内部对消息进行编码时所用消息版本的绑定元素
    /// </summary>
    public MessageEncodingBindingElement InnerMessageEncodingBindingElement
    {
      get { return _innerMessageEncodingBindingElement; }
    }
    /// <summary>
    /// 可由消息编码器工厂所生成消息编码器处理的消息版本
    /// </summary>
    public override MessageVersion MessageVersion
    {
      get { return _innerMessageEncodingBindingElement.MessageVersion; }
    }

    private readonly MessageEncoder _encoder;
    /// <summary>
    /// 编码器是用于将消息写入到流中并从流中读取消息的组件
    /// </summary>
    public override MessageEncoder Encoder
    {
      get { return _encoder; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 返回一个消息编码器
    /// 可用于关联基于会话的交换中的消息
    /// </summary>
    public override MessageEncoder CreateSessionEncoder()
    {
      return base.CreateSessionEncoder();
    }

    #endregion
  }
}