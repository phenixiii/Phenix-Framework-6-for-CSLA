using System;

namespace Phenix.Core.Net.RemotingCompression
{
  /// <summary>
  /// Marks the class as an exempt from compression.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes")]
  public class NonCompressibleAttribute : Attribute
  {
  }
}
