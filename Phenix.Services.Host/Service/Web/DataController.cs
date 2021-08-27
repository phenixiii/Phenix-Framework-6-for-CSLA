using System;
using System.Net.Http;
using System.Web.Http;
using Phenix.Core.Data;
using Phenix.Core.Mapping;
using Phenix.Core.Security;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service.Web
{
  public sealed class DataController : ApiController
  {
    #region 方法

    internal static Type LoadDataType(string dataName)
    {
      Type result = Phenix.Core.Reflection.Utilities.LoadType(dataName);
      if (result == null)
        throw new InvalidOperationException(dataName + "在服务端未定义");
      if (result.IsAbstract || !result.IsPublic)
        throw new InvalidOperationException(dataName + "在服务端未公开");
      if (!typeof(IEntity).IsAssignableFrom(result) && !typeof(IEntityCollection).IsAssignableFrom(result) &&
        !typeof(IService).IsAssignableFrom(result))
        throw new InvalidOperationException(dataName + "需实现IEntity、IEntityCollection、IService接口");
      return result;
    }

    private static string[] UriParts(Uri uri)
    {
      return uri.Segments.Length == 4 && !String.IsNullOrEmpty(uri.Segments[3]) ? uri.Segments[3].Split(':') : null;
    }

    private static Type LoadDataType(HttpRequestMessage request, ExecuteAction action)
    {
      string[] uriParts = UriParts(request.RequestUri);
      if (uriParts != null && uriParts.Length >= 1)
      {
        Type result = LoadDataType(Uri.UnescapeDataString(uriParts[0]));
        return !UserIdentity.IsByDeny(UserIdentity.CurrentIdentity, result, action, true) ? result : null;
      }
      throw new InvalidOperationException(request.RequestUri + "缺失DataName资源路径");
    }
    
    private static Type LoadMasterType(HttpRequestMessage request)
    {
      string[] uriParts = UriParts(request.RequestUri);
      if (uriParts != null && uriParts.Length >= 2)
      {
        string masterName = Uri.UnescapeDataString(uriParts[1]);
        Type result = Phenix.Core.Reflection.Utilities.GetCoreType(Phenix.Core.Reflection.Utilities.LoadType(masterName));
        if (result == null)
          throw new InvalidOperationException(masterName + "在服务端未定义");
        if (!typeof(IEntity).IsAssignableFrom(result))
          throw new InvalidOperationException(masterName + "需实现IEntity接口");
        return result;
      }
      throw new InvalidOperationException(request.RequestUri + "缺失MasterName资源路径");
    }

    private static Type LoadCriteriaType(HttpRequestMessage request, string criteriaName)
    {
      if (String.IsNullOrEmpty(criteriaName))
      {
        Type dataType = LoadDataType(request, ExecuteAction.Fetch);
        ClassMapInfo classMapInfo = ClassMemberHelper.GetClassMapInfo(dataType);
        if (classMapInfo != null && classMapInfo.DefaultCriteriaType != null)
          return classMapInfo.DefaultCriteriaType;
      }
      else
      {
        Type result = Phenix.Core.Reflection.Utilities.LoadType(criteriaName);
        if (result == null)
        {
          Type dataType = LoadDataType(request, ExecuteAction.Fetch);
          result = dataType.Assembly.GetType(criteriaName, false);
          if (result == null)
          {
            result = LoadCriteriaType(request, null);
            if (result == null)
              throw new InvalidOperationException(criteriaName + "需定义在程序集" + dataType.Assembly.GetName().Name);
          }
        }
        if (!typeof(ICriteria).IsAssignableFrom(result))
          throw new InvalidOperationException(criteriaName + "需实现ICriteria接口");
        return result;
      }
      return null;
    }

    private static Type LoadCriteriaType(HttpRequestMessage request)
    {
      string[] uriParts = UriParts(request.RequestUri);
      if (uriParts != null && uriParts.Length >= 2)
      {
        Type result = LoadCriteriaType(request, Uri.UnescapeDataString(uriParts[uriParts.Length - 1]));
        if (result != null)
          return result;
      }
      throw new InvalidOperationException(request.RequestUri + "缺失CriteriaName资源路径");
    }

    /// <summary>
    /// 取Sequence/数据集
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public HttpResponseMessage Get()
    {
      if (UriParts(Request.RequestUri) != null)
        return Get(null, null, null);
      try
      {
        ServiceManager.CheckIn();
        return Phenix.Core.Web.Utilities.PackResponse(Request, DataHub.SequenceValue.ToString());
      }
      catch (Exception ex)
      {
        return Phenix.Core.Web.Utilities.PackErrorResponse(Request, ex);
      }
    }

    private static object DoGet(Criterions criterions, int? pageSize, int? pageNo)
    {
      if (pageSize.HasValue)
        criterions.PageSize = pageSize.Value;
      if (pageNo.HasValue)
        criterions.PageNo = pageNo.Value;
      return criterions.PageSize == 0 && criterions.PageNo == 0
        ? DataHub.GetRecordCount(criterions).ToString()
        : DataHub.FetchContent(criterions);
    }

    /// <summary>
    /// 取数据/数据集
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public HttpResponseMessage Get(string id, int? pageSize, int? pageNo)
    {
      try
      {
        ServiceManager.CheckIn();
        Criterions criterions;
        Type dataType = LoadDataType(Request, ExecuteAction.Fetch);
        if (String.IsNullOrEmpty(id))
          criterions = new Criterions(dataType);
        else
        {
          long primaryKeyValue;
          if (Int64.TryParse(id, out primaryKeyValue))
          {
            if (!typeof(IEntity).IsAssignableFrom(dataType))
              throw new InvalidOperationException(dataType.FullName + "需实现IEntity接口");
            object itself = Activator.CreateInstance(dataType, true);
            if (!EntityHelper.FillPrimaryKeyFieldValue(itself, primaryKeyValue))
              throw new InvalidOperationException(dataType.FullName + "无法填充主键值: " + primaryKeyValue);
            criterions = new Criterions(dataType, itself);
          }
          else
          {
            ICriteria criteria = (ICriteria)CriteriaHelper.JsonDeserialize(LoadCriteriaType(Request), Uri.UnescapeDataString(id));
            criterions = new Criterions(dataType, criteria);
          }
        }
        return Phenix.Core.Web.Utilities.PackResponse(Request, DoGet(criterions, pageSize, pageNo));
      }
      catch (Exception ex)
      {
        return Phenix.Core.Web.Utilities.PackErrorResponse(Request, ex);
      }
    }

    /// <summary>
    /// 取数据集
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public HttpResponseMessage Get(string masterId, string groupName, string id, int? pageSize, int? pageNo)
    {
      try
      {
        ServiceManager.CheckIn();
        if (String.IsNullOrEmpty(masterId))
          throw new InvalidOperationException("参数masterId不允许为空");
        Type masterType = LoadMasterType(Request);
        object master = Activator.CreateInstance(masterType, true);
        if (!EntityHelper.FillPrimaryKeyFieldValue(master, masterId))
          throw new InvalidOperationException(masterType.FullName + "无法填充主键值: " + masterId);
        ICriteria criteria = !String.IsNullOrEmpty(id) ? (ICriteria)CriteriaHelper.JsonDeserialize(LoadCriteriaType(Request), Uri.UnescapeDataString(id)) : null;
        Criterions criterions = new Criterions(LoadDataType(Request, ExecuteAction.Fetch), criteria, master, groupName);
        return Phenix.Core.Web.Utilities.PackResponse(Request, DoGet(criterions, pageSize, pageNo));
      }
      catch (Exception ex)
      {
        return Phenix.Core.Web.Utilities.PackErrorResponse(Request, ex);
      }
    }

    /// <summary>
    /// 提交数据
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public HttpResponseMessage Post()
    {
      try
      {
        ServiceManager.CheckIn();
        Type type = LoadDataType(Request, ExecuteAction.Update);
        return typeof(IService).IsAssignableFrom(type)
          ? Phenix.Core.Web.Utilities.ExecuteService(Request, type)
          : Phenix.Core.Web.Utilities.PackResponse(Request, DataHub.SaveContent(type, Request.Content.ReadAsStringAsync().Result).ToString());
      }
      catch (Exception ex)
      {
        return Phenix.Core.Web.Utilities.PackErrorResponse(Request, ex);
      }
    }

    #endregion
  }
}
