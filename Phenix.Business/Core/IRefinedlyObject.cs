using System.Collections.Generic;
using System.Data.Common;
using Phenix.Core.Mapping;

namespace Phenix.Business.Core
{
  internal interface IRefinedlyObject : IRefinedly
  {
    #region 方法

    void FillAggregateValues(FieldAggregateMapInfo fieldAggregateMapInfo);

    bool DeleteSelf(DbTransaction transaction, ref List<IRefinedlyObject> ignoreLinks);

    #endregion
  }
}
