using System;
using System.Collections.Generic;
using System.ServiceModel;
using Phenix.Core.Security;

namespace Phenix.Services.Contract.Wcf
{
  [ServiceContract]
  public interface IDataSecurity
  {
    [OperationContract]
    [UseNetDataContract]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    object GetAllowUserMultipleAddressLogin();

    [OperationContract]
    [UseNetDataContract]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    object GetSessionExpiresMinutes();

    [OperationContract]
    [UseNetDataContract]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    object GetEmptyRolesIsDeny();

    [OperationContract]
    [UseNetDataContract]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    object GetEasyAuthorization();

    [OperationContract]
    [UseNetDataContract]
    object GetRoleInfos(UserIdentity identity);
    
    [OperationContract]
    [UseNetDataContract]
    object GetGrantRoleInfos(UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object GetSectionInfos(UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object GetIdentities(long departmentId, IList<long> positionIds, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object CheckInIdentity(UserIdentity identity, bool reset);

    [OperationContract]
    [UseNetDataContract]
    object CheckIn(string localAddress, string servicesAddress, string userNumber, string timestamp, string signature, bool reset);

    [OperationContract]
    [UseNetDataContract]
    object LogOnVerify(string userNumber, string tag);

    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    void LogOff(UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object ChangePassword(string newPassword, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object UnlockPassword(string userNumber, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object ResetPassword(string userNumber, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object SetProcessLockInfo(string processName, string caption, bool toLocked, TimeSpan expiryTime, string remark, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object GetProcessLockInfo(string processName, UserIdentity identity);
  }
}
