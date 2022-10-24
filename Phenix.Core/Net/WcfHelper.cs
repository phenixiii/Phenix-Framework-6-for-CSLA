using System;
using System.ServiceModel.Channels;
using Phenix.Core.Net.WcfCompression;

namespace Phenix.Core.Net
{
  /// <summary>
  /// WCF助手
  /// </summary>
  public static class WcfHelper
  {
    #region 方法

    #region CreateBinding

    /// <summary>
    /// CreateBinding
    /// </summary>
    public static Binding CreateBinding()
    {
      switch (WcfConfig.ServicesProtocolType)
      {
        case WcfProtocolType.BasicHttp:
          return CreateBasicHttpBinding();
        case WcfProtocolType.NetTcp:
          return CreateNetTcpBinding();
        default:
          throw new InvalidOperationException();
      }
    }

    /// <summary>
    /// CreateBinding
    /// </summary>
    public static Binding CreateBasicHttpBinding()
    {
      HttpTransportBindingElement bindingElement = new HttpTransportBindingElement();
      bindingElement.MaxReceivedMessageSize = Int32.MaxValue;   // default is 65536
      BinaryMessageEncodingBindingElement binaryBindingElement = new BinaryMessageEncodingBindingElement();
      binaryBindingElement.ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
      {
        MaxBytesPerRead = Int32.MaxValue,         // default is 4096
        MaxDepth = Int32.MaxValue,                // default is 32
        MaxArrayLength = Int32.MaxValue,          // default is 16384
        MaxStringContentLength = Int32.MaxValue,  // default is 8192
        MaxNameTableCharCount = Int32.MaxValue    // default is 16384
      };
      return new CustomBinding(new CompressEncodingBindingElement(binaryBindingElement), bindingElement);  
    }

    /// <summary>
    /// CreateBinding
    /// </summary>
    public static Binding CreateNetTcpBinding()
    {
      TcpTransportBindingElement bindingElement = new TcpTransportBindingElement();
      bindingElement.MaxReceivedMessageSize = Int32.MaxValue;   // default is 65536
      BinaryMessageEncodingBindingElement binaryBindingElement = new BinaryMessageEncodingBindingElement();
      binaryBindingElement.ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
      {
        MaxBytesPerRead = Int32.MaxValue,         // default is 4096
        MaxDepth = Int32.MaxValue,                // default is 32
        MaxArrayLength = Int32.MaxValue,          // default is 16384
        MaxStringContentLength = Int32.MaxValue,  // default is 8192
        MaxNameTableCharCount = Int32.MaxValue    // default is 16384
      };
      return new CustomBinding(new CompressEncodingBindingElement(binaryBindingElement), bindingElement);
    }

    #endregion

    #region CreateUrl

    /// <summary>
    /// CreateUrl
    /// </summary>
    public static Uri CreateUrl(string host, string path)
    {
      switch (WcfConfig.ServicesProtocolType)
      {
        case WcfProtocolType.BasicHttp:
          return CreateBasicHttpUrl(host, path);
        case WcfProtocolType.NetTcp:
          return CreateNetTcpUrl(host, path);
        default:
          throw new InvalidOperationException();
      }
    }

    /// <summary>
    /// CreateUrl
    /// </summary>
    public static Uri CreateBasicHttpUrl(string host, string path)
    {
      return CreateBasicHttpUrl(host, NetConfig.LinkBackupServices ? WcfConfig.BasicHttpPort + 1 : WcfConfig.BasicHttpPort, path);
    }

    /// <summary>
    /// CreateUrl
    /// </summary>
    public static Uri CreateNetTcpUrl(string host, string path)
    {
      return CreateNetTcpUrl(host, NetConfig.LinkBackupServices ? WcfConfig.NetTcpPort + 1 : WcfConfig.NetTcpPort, path);
    }

    /// <summary>
    /// CreateUrl
    /// </summary>
    public static Uri CreateUrl(string host, int port, string path)
    {
      switch (WcfConfig.ServicesProtocolType)
      {
        case WcfProtocolType.BasicHttp:
          return CreateBasicHttpUrl(host, port, path);
        case WcfProtocolType.NetTcp:
          return CreateNetTcpUrl(host, port, path);
        default:
          throw new InvalidOperationException();
      }
    }

    /// <summary>
    /// CreateUrl
    /// </summary>
    public static Uri CreateBasicHttpUrl(string host, int port, string path)
    {
      if (host.IndexOf(':') > 0)
        return new Uri(String.Format(@"http://{0}/{1}", host, path));
      else
        return new Uri(String.Format(@"http://{0}:{1}/{2}", host, port, path));
    }

    /// <summary>
    /// CreateUrl
    /// </summary>
    public static Uri CreateNetTcpUrl(string host, int port, string path)
    {
      if (host.IndexOf(':') > 0)
        return new Uri(String.Format(@"net.tcp://{0}/{1}", host, path));
      else
        return new Uri(String.Format(@"net.tcp://{0}:{1}/{2}", host, port, path));
    }

    #endregion

    #endregion
  }
}