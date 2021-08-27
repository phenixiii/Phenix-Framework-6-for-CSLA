using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phenix.Test.使用指南._15._6.Business
{

  [System.Serializable]
  public class DeliveryJobTicket : DeliveryJobTicket<DeliveryJobTicket>
  {
    private DeliveryJobTicket()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// 清单
  /// </summary>
  [System.Serializable]
  public class DeliveryJobTicketList : Phenix.Business.BusinessListBase<DeliveryJobTicketList, DeliveryJobTicket>
  {
    private DeliveryJobTicketList()
    {
      //禁止添加代码
    }
  }
}
