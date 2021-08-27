using System;
using System.Collections.Generic;
using System.Data.Common;
using Phenix.Core.Mapping;

namespace Phenix.Business.Core
{
  internal interface IRefinedly
  {
    #region 方法
    
    IDictionary<string, Type> GetCacheTypes();

    void RecordHasChanged();

    void CascadingMarkNew(bool cascadingDelete);

    bool CascadingRefresh(IRefinedly source);

    void CascadingLinkTo(object link, string groupName, bool throwIfNotFound);

    void CascadingUnlink(object link, string groupName, bool throwIfNotFound);

    bool CascadingExistLink(Type masterType, string groupName);

    void CascadingFillIdenticalValues(IRefinedlyObject source, string[] sourcePropertyNames, ref List<IRefinedlyObject> ignoreLinks);

    List<IEntity> GetAllDirty(List<IEntity> entities);

    IRefinedly GetAllDirty(IRefinedlyObject masterBusiness);

    string CheckRules(bool onlyOldError, bool onlySelfDirty, ref List<IRefinedlyObject> ignoreLinks);

    bool GetIsDirty(ref List<IRefinedlyObject> ignoreLinks);

    bool GetIsValid(ref List<IRefinedlyObject> ignoreLinks);
    
    bool GetNeedRefresh(ref List<IRefinedlyObject> ignoreLinks);

    void SynchronizeEditLevel(IRefinedly source);

    void BeginEdit(int editLevel, ref List<IRefinedlyObject> ignoreLinks);

    bool CancelEdit(int editLevel, ref List<IRefinedlyObject> ignoreLinks);

    void ApplyEdit(int editLevel, ref List<IRefinedlyObject> ignoreLinks);

    void CompletelyApplyEdit(bool toMarkOld, bool inCascadingDelete, bool needCheckDirty, bool keepEnsemble, ref List<IRefinedlyObject> ignoreLinks);

    void ExecuteArchive(DbTransaction target, DbConnection source);

    void SaveSelf(DbTransaction transaction, ref List<IRefinedlyObject> ignoreLinks);

    void CascadingAggregate(DbTransaction transaction);

    void ExecuteFetchSelf(DbConnection connection, Criterions criterions);

    void ExecuteFetchSelf(DbTransaction transaction, Criterions criterions);

    #endregion
  }
}
