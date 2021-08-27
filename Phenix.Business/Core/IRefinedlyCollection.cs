using System.Collections.Generic;
using System.Data.Common;

namespace Phenix.Business.Core
{
  internal interface IRefinedlyCollection : IRefinedly
  {
    #region 方法

    void AddDeleted(IRefinedlyObject item);

    bool ClearRemove(IRefinedlyObject item);
    
    bool DeleteSelf(DbTransaction transaction, IRefinedlyObject masterBusiness, ref List<IRefinedlyObject> ignoreLinks);

    #endregion
  }
}