#if Top
using System.Collections.ObjectModel;
#endif

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Phenix.Core.Data;
using Phenix.Core.Security;

namespace Phenix.Services.Library
{
  internal class Message : Phenix.Core.Message.IMessage
  {
    public void Send(string receiver, string content, UserIdentity identity)
    {
      DefaultDatabase.Execute(ExecuteSend, receiver, content, identity);
    }

    private static void ExecuteSend(DbTransaction transaction, string receiver, string content, UserIdentity identity)
    {
      string userNumber;
      long userId;
      if (Int64.TryParse(receiver, out userId))
        userNumber = null;
      else
      {
        userNumber = receiver;
        userId = Int64.MinValue;
      }
      long id = Sequence.Value;
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_Message
  (MS_ID, MS_Send_UserNumber, MS_Receive_UserNumber, MS_CreatedTime)
  select :MS_ID, :MS_Send_UserNumber, US_UserNumber, sysdate
  from PH_User
  where US_UserNumber = :US_UserNumber or US_ID = :US_ID"))
      {
        DbCommandHelper.CreateParameter(command, "MS_ID", id);
        DbCommandHelper.CreateParameter(command, "MS_Send_UserNumber", identity.UserNumber);
        DbCommandHelper.CreateParameter(command, "US_UserNumber", userNumber);
        DbCommandHelper.CreateParameter(command, "US_ID", userId);
        if (DbCommandHelper.ExecuteNonQuery(command, false) == 0)
          throw new InvalidOperationException(String.Format("未搜索到消息接收人{0}(登录工号/ID)!", receiver));
      }
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_Message set
    MS_Content = :MS_Content
  where MS_ID = :MS_ID"))
      {
        DbCommandHelper.CreateParameter(command, "MS_Content", content);
        DbCommandHelper.CreateParameter(command, "MS_ID", id);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    public IDictionary<long, string> Receive(UserIdentity identity)
    {
      return DefaultDatabase.ExecuteGet(ExecuteReceive, identity);
    }

    private static IDictionary<long, string> ExecuteReceive(DbTransaction transaction, UserIdentity identity)
    {
      Dictionary<long, string> result = new Dictionary<long, string>();
      using (DataReader reader = new DataReader(transaction,
@"select MS_ID, MS_Content
  from PH_Message
  where MS_ReceivedTime is null and MS_Receive_UserNumber = :MS_Receive_UserNumber
  order by MS_CreatedTime",
        CommandBehavior.SingleResult, false))
      {
        reader.CreateParameter("MS_Receive_UserNumber", identity.UserNumber);
        while (reader.Read())
          result.Add(reader.GetInt64(0), reader.GetNullableString(1));
      }
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_Message set
    MS_SendedTime = sysdate
  where MS_ID = :MS_ID"))
      {
        foreach (KeyValuePair<long, string> kvp in result)
        {
          DbCommandHelper.CreateParameter(command, "MS_ID", kvp.Key);
          DbCommandHelper.ExecuteNonQuery(command, false);
        }
      }
#if Top
      return new ReadOnlyDictionary<long, string>(result);
#else
      return result;
#endif
    }

    public void AffirmReceived(long id, bool burn, UserIdentity identity)
    {
      DefaultDatabase.Execute(ExecuteAffirmReceived, id, burn, identity);
    }

    private static void ExecuteAffirmReceived(DbTransaction transaction, long id, bool burn, UserIdentity identity)
    {
      if (burn)
        using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"delete PH_Message
  where MS_ID = :MS_ID"))
        {
          DbCommandHelper.CreateParameter(command, "MS_ID", id);
          DbCommandHelper.ExecuteNonQuery(command, false);
        }
      else
        using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_Message set
    MS_ReceivedTime = sysdate
  where MS_ID = :MS_ID"))
        {
          DbCommandHelper.CreateParameter(command, "MS_ID", id);
          DbCommandHelper.ExecuteNonQuery(command, false);
        }
    }
  }
}
