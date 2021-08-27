using System;
using System.Collections.Generic;
using Phenix.Core.Message;
using Phenix.Core.Security;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service
{
  public sealed class Message : MarshalByRefObject, IMessage
  {
    #region ·½·¨

    public void Send(string receiver, string content, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      MessageHub.Send(receiver, content, context.Identity);
    }

    public IDictionary<long, string> Receive(UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return MessageHub.Receive(context.Identity);
    }

    public void AffirmReceived(long id, bool burn, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      MessageHub.AffirmReceived(id, burn, context.Identity);
    }

    #endregion
  }
}