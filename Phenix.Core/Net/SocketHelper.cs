using System.Collections.Generic;
using System.Text;

namespace Phenix.Core.Net
{
  /// <summary>
  /// Socket助手
  /// </summary>
  public static class SocketHelper
  {
    /// <summary>
    /// 分解消息
    /// </summary>
    public static IList<byte[]> SplitMessages(string s, int bufferSize)
    {
      List<byte[]> result = new List<byte[]>();
      for (int i = 0; i < s.Length / bufferSize; i++)
        result.Add(Encoding.Default.GetBytes(s.Substring(
          bufferSize * i,
          bufferSize)));
      if (s.Length % bufferSize > 0)
        result.Add(Encoding.Default.GetBytes(s.Substring(
          bufferSize * (s.Length / bufferSize),
          s.Length % bufferSize)));
      return result;
    }
  }
}
